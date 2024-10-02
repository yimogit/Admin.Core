﻿using System;
using System.Linq;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnceMi.AspNetCore.OSS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ZhonTai.Admin.Domain;
using ZhonTai.Admin.Services.Dto;
using ZhonTai.Admin.Core.Helpers;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Dto;
using ZhonTai.Common.Files;
using ZhonTai.Common.Helpers;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Resources;

namespace ZhonTai.Admin.Services;

/// <summary>
/// 文件服务
/// </summary>
[Order(110)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class FileService : BaseService, IFileService, IDynamicApi
{
    private readonly IFileRepository _fileRep;
    private readonly IOSSServiceFactory _oSSServiceFactory;
    private readonly OSSConfig _oSSConfig;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AdminLocalizer _adminLocalizer;

    public FileService(
        IFileRepository fileRep,
        IOSSServiceFactory oSSServiceFactory,
        IOptions<OSSConfig> oSSConfig,
        IHttpContextAccessor httpContextAccessor,
        AdminLocalizer adminLocalizer
    )
    {
        _fileRep = fileRep;
        _oSSServiceFactory = oSSServiceFactory;
        _oSSConfig = oSSConfig.Value;
        _httpContextAccessor = httpContextAccessor;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<FileGetPageOutput>> GetPageAsync(PageInput<FileGetPageDto> input)
    {
        var fileName = input.Filter?.FileName;

        var list = await _fileRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(fileName.NotNull(), a => a.FileName.Contains(fileName))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync(a => new FileGetPageOutput { ProviderName = a.Provider.ToString() });

        var data = new PageOutput<FileGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task DeleteAsync(FileDeleteInput input)
    {
        var file = await _fileRep.GetAsync(input.Id);
        if (file == null)
        {
            return;
        }

        var shareFile = await _fileRep.Where(a=>a.Id != input.Id && a.LinkUrl == file.LinkUrl).AnyAsync();
        if (!shareFile)
        {
            if(file.Provider.HasValue)
            {
                var oSSService = _oSSServiceFactory.Create(file.Provider.ToString());
                var oSSOptions = _oSSConfig.OSSConfigs.Where(a => a.Enable && a.Provider == file.Provider).FirstOrDefault();
                var enableOss = oSSOptions != null && oSSOptions.Enable;
                if (enableOss)
                {
                    var filePath = Path.Combine(file.FileDirectory, file.SaveFileName + file.Extension).ToPath();
                    await oSSService.RemoveObjectAsync(file.BucketName, filePath);
                }
            }
            else
            {
                var env = LazyGetRequiredService<IWebHostEnvironment>();
                var filePath = Path.Combine(env.WebRootPath, file.FileDirectory, file.SaveFileName + file.Extension).ToPath();
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        await _fileRep.DeleteAsync(file.Id);
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="file">文件</param>
    /// <param name="fileDirectory">文件目录</param>
    /// <param name="fileReName">文件重命名</param>
    /// <returns></returns>
    public async Task<FileEntity> UploadFileAsync([Required] IFormFile file, string fileDirectory = "", bool fileReName = true)
    {
        var localUploadConfig = _oSSConfig.LocalUploadConfig;

        var extention = Path.GetExtension(file.FileName).ToLower();
        var hasIncludeExtension = localUploadConfig.IncludeExtension?.Length > 0;
        if (hasIncludeExtension && !localUploadConfig.IncludeExtension.Contains(extention))
        {
            throw new Exception(_adminLocalizer["不允许上传{0}文件格式", extention]);
        }
        var hasExcludeExtension = localUploadConfig.ExcludeExtension?.Length > 0;
        if (hasExcludeExtension && localUploadConfig.ExcludeExtension.Contains(extention))
        {
            throw new Exception(_adminLocalizer["不允许上传{0}文件格式", extention]);
        }

        var fileLenth = file.Length;
        if (fileLenth > localUploadConfig.MaxSize)
        {
            throw new Exception(_adminLocalizer["文件大小不能超过{0}", new FileSize(localUploadConfig.MaxSize)]);
        }

        var oSSOptions = _oSSConfig.OSSConfigs.Where(a => a.Enable && a.Provider == _oSSConfig.Provider).FirstOrDefault();
        var enableOss = oSSOptions != null && oSSOptions.Enable;
        var enableMd5 = enableOss ? oSSOptions.Md5 : localUploadConfig.Md5;
        var md5 = string.Empty;
        if (enableMd5)
        {
            md5 = MD5Encrypt.GetHash(file.OpenReadStream());
            var md5FileEntity = await _fileRep.WhereIf(enableOss, a => a.Provider == oSSOptions.Provider).Where(a => a.Md5 == md5).FirstAsync();
            if (md5FileEntity != null)
            {
                var sameFileEntity = new FileEntity
                {
                    Provider = md5FileEntity.Provider,
                    BucketName = md5FileEntity.BucketName,
                    FileGuid = FreeUtil.NewMongodbId(),
                    SaveFileName = md5FileEntity.SaveFileName,
                    FileName = Path.GetFileNameWithoutExtension(file.FileName),
                    Extension = extention,
                    FileDirectory = md5FileEntity.FileDirectory,
                    Size = md5FileEntity.Size,
                    SizeFormat = md5FileEntity.SizeFormat,
                    LinkUrl = md5FileEntity.LinkUrl,
                    Md5 = md5,
                };
                sameFileEntity = await _fileRep.InsertAsync(sameFileEntity);
                return sameFileEntity;
            }
        }

        if (fileDirectory.IsNull())
        {
            fileDirectory = localUploadConfig.Directory;
            if (localUploadConfig.DateTimeDirectory.NotNull())
            {
                fileDirectory = Path.Combine(fileDirectory, DateTime.Now.ToString(localUploadConfig.DateTimeDirectory)).ToPath();
            }
        }

        var fileSize = new FileSize(fileLenth);
        var fileEntity = new FileEntity
        {
            Provider = oSSOptions?.Provider,
            BucketName = oSSOptions?.BucketName,
            FileGuid = FreeUtil.NewMongodbId(),
            FileName = Path.GetFileNameWithoutExtension(file.FileName),
            Extension = extention,
            FileDirectory = fileDirectory,
            Size = fileSize.Size,
            SizeFormat = fileSize.ToString(),
            Md5 = md5
        };
        fileEntity.SaveFileName = fileReName ? fileEntity.FileGuid.ToString() : fileEntity.FileName;

        var filePath = Path.Combine(fileDirectory, fileEntity.SaveFileName + fileEntity.Extension).ToPath();
        var url = string.Empty;
        if (enableOss)
        {
            url = oSSOptions.Url;
            if (url.IsNull())
            {
                url = oSSOptions.Provider switch
                {
                    OSSProvider.Minio => $"{oSSOptions.Endpoint}/{oSSOptions.BucketName}",
                    OSSProvider.Aliyun => $"{oSSOptions.BucketName}.{oSSOptions.Endpoint}",
                    OSSProvider.QCloud => $"{oSSOptions.BucketName}-{oSSOptions.Endpoint}.cos.{oSSOptions.Region}.myqcloud.com",
                    OSSProvider.Qiniu => $"{oSSOptions.BucketName}.{oSSOptions.Region}.qiniucs.com",
                    OSSProvider.HuaweiCloud => $"{oSSOptions.BucketName}.{oSSOptions.Endpoint}",
                    _ => ""
                };

                if (url.IsNull())
                {
                    throw ResultOutput.Exception(_adminLocalizer["请配置{0}的Url参数", oSSOptions.Provider]);
                }

                var urlProtocol = oSSOptions.IsEnableHttps ? "https" : "http";
                fileEntity.LinkUrl = $"{urlProtocol}://{url}/{filePath}";
            }
            else
            {
                fileEntity.LinkUrl = $"{url}/{filePath}";
            }
        }
        else
        {
            string scheme = _httpContextAccessor.HttpContext.Request.Scheme;
            string host = _httpContextAccessor.HttpContext.Request.Host.Value;
            string httpHost = $"{scheme}://{host}";

            if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Original", out var original))
            {
                httpHost = original.FirstOrDefault();
            }
            else if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-Proto", out var forwardedProto) &&
                _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-Host", out var forwardedHost))
            {
                httpHost = $"{forwardedProto.FirstOrDefault()}://{forwardedHost.FirstOrDefault()}";
            }

            fileEntity.LinkUrl = $"{(httpHost.EndsWith("/") ? httpHost : httpHost + "/")}{filePath}";
        }

        if (enableOss)
        {
            var oSSService = _oSSServiceFactory.Create(_oSSConfig.Provider.ToString());
            await oSSService.PutObjectAsync(oSSOptions.BucketName, filePath, file.OpenReadStream());
        }
        else
        {
            var uploadHelper = LazyGetRequiredService<UploadHelper>();
            var env = LazyGetRequiredService<IWebHostEnvironment>();
            fileDirectory = Path.Combine(env.WebRootPath, fileDirectory).ToPath();
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            filePath = Path.Combine(env.WebRootPath, filePath).ToPath();
            await uploadHelper.SaveAsync(file, filePath);
        }

        fileEntity = await _fileRep.InsertAsync(fileEntity);

        return fileEntity;
    }

    /// <summary>
    /// 上传多文件
    /// </summary>
    /// <param name="files">文件列表</param>
    /// <param name="fileDirectory">文件目录</param>
    /// <param name="fileReName">文件重命名</param>
    /// <returns></returns>
    public async Task<List<FileEntity>> UploadFilesAsync([Required] IFormFileCollection files, string fileDirectory = "", bool fileReName = true)
    {
        var fileList = new List<FileEntity>();
        foreach (var file in files)
        {
            fileList.Add(await UploadFileAsync(file, fileDirectory, fileReName));
        }
        return fileList;
    }
}