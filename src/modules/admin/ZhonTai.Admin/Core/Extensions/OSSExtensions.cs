﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using OnceMi.AspNetCore.OSS;
using System.Linq;
using ZhonTai.Admin.Core.Configs;
using OSSOptions = ZhonTai.Admin.Core.Configs.OSSOptions;

namespace ZhonTai.Admin.Core.Extensions;

public static class OSSExtensions
{
    private static void CreateBucketName(IOSSServiceFactory oSSServiceFactory, OSSOptions oSSOptions)
    {
        var oSSService = oSSServiceFactory.Create(oSSOptions.Provider.ToString());
        if (!oSSService.BucketExistsAsync(oSSOptions.BucketName).Result)
        {
            oSSService.CreateBucketAsync(oSSOptions.BucketName).Wait();
        }

        //设置Minio存储桶权限
        if (oSSOptions.Provider == OSSProvider.Minio)
        {
            var bucketName = oSSOptions.BucketName;
            var minioClient = new MinioClient()
                .WithEndpoint(oSSOptions.Endpoint)
                .WithCredentials(oSSOptions.AccessKey, oSSOptions.SecretKey);

            if (oSSOptions.Region.NotNull())
            {
                minioClient.WithRegion(oSSOptions.Region);
            }

            minioClient = minioClient.Build();
            //查看存储桶权限
            //var policy = minioClient.GetPolicyAsync(new GetPolicyArgs().WithBucket(bucketName)).Result;
            //设置存储桶权限，存储桶内的所有文件可以通过链接永久访问
            var policy = $@"{{""Version"":""2012-10-17"",""Statement"":[{{""Effect"":""Allow"",""Principal"":{{""AWS"":[""*""]}},""Action"":[""s3:GetBucketLocation""],""Resource"":[""arn:aws:s3:::{bucketName}""]}},{{""Effect"":""Allow"",""Principal"":{{""AWS"":[""*""]}},""Action"":[""s3:GetObject""],""Resource"":[""arn:aws:s3:::{bucketName}/*.*""]}}]}}";
            var setPolicyArgs = new SetPolicyArgs().WithBucket(bucketName).WithPolicy(policy);
            minioClient.SetPolicyAsync(setPolicyArgs).Wait();
        }
    }

    public static IServiceCollection AddOSS(this IServiceCollection services)
    {
        var oSSConfig = services.BuildServiceProvider().GetRequiredService<IOptions<OSSConfig>>().Value;

        if (oSSConfig.OSSConfigs != null && oSSConfig.OSSConfigs.Any(s => s.Enable))
        {
            foreach (var oSSOptions in oSSConfig.OSSConfigs)
            {
                if (oSSOptions.Enable)
                {
                    services.AddOSSService(oSSOptions.Provider.ToString(), option =>
                    {
                        option.Provider = oSSOptions.Provider;
                        option.Endpoint = oSSOptions.Endpoint;
                        option.Region = oSSOptions.Region;
                        option.AccessKey = oSSOptions.AccessKey;
                        option.SecretKey = oSSOptions.SecretKey;
                        option.IsEnableHttps = oSSOptions.IsEnableHttps;
                        option.IsEnableCache = oSSOptions.IsEnableCache;
                    });

                    var oSSServiceFactory = services.BuildServiceProvider().GetRequiredService<IOSSServiceFactory>();
                    CreateBucketName(oSSServiceFactory, oSSOptions);
                }
            }
        }
        else
        {
            //未启用OSS注入
            services.AddOSSService(option =>
            {
                option.Provider = OSSProvider.Invalid;
            });
        }

        return services;
    }
}
