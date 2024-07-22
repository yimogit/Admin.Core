﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Domain.Pkg;
using ZhonTai.Admin.Domain.PkgPermission;
using ZhonTai.Admin.Domain.TenantPkg;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.RolePermission;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.Org;
using ZhonTai.Admin.Resources;
using ZhonTai.Admin.Services.Pkg.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;

namespace ZhonTai.Admin.Services.Pkg;

/// <summary>
/// 套餐服务
/// </summary>
[Order(51)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class PkgService : BaseService, IDynamicApi
{
    private readonly IPkgRepository _pkgRep;
    private readonly Lazy<ITenantRepository> _tenantRep;
    private readonly ITenantPkgRepository _tenantPkgRep;
    private readonly Lazy<IPkgPermissionRepository> _pkgPermissionRep;
    private readonly Lazy<IRolePermissionRepository> _rolePermissionRep;
    private readonly Lazy<IUserRepository> _userRep;
    private readonly AdminLocalizer _adminLocalizer;

    public PkgService(
        IPkgRepository pkgRep,
        Lazy<ITenantRepository> tenantRep,
        ITenantPkgRepository tenantPkgRep,
        Lazy<IPkgPermissionRepository> pkgPermissionRep,
        Lazy<IRolePermissionRepository> rolePermissionRep,
        Lazy<IUserRepository> userRep,
        AdminLocalizer adminLocalizer
    )
    {
        _pkgRep = pkgRep;
        _tenantRep = tenantRep;
        _tenantPkgRep = tenantPkgRep;
        _pkgPermissionRep = pkgPermissionRep;
        _rolePermissionRep = rolePermissionRep;
        _userRep = userRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 清除租户下所有用户权限缓存
    /// </summary>
    /// <param name="tenantIds"></param>
    [NonAction]
    public async Task ClearUserPermissionsAsync(List<long> tenantIds)
    {
        using var _ = _userRep.Value.DataFilter.Disable(FilterNames.Tenant);
        var userIds = await _userRep.Value.Select.Where(a => tenantIds.Contains(a.TenantId.Value)).ToListAsync(a => a.Id);
        if (userIds.Any())
        {
            foreach (var userId in userIds)
            {
                await Cache.DelAsync(CacheKeys.UserPermission + userId);
            }
        }
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PkgGetOutput> GetAsync(long id)
    {
        return await _pkgRep.Select
        .WhereDynamic(id)
        .ToOneAsync<PkgGetOutput>();
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<PkgGetListOutput>> GetListAsync([FromQuery]PkgGetListInput input)
    {
        var list = await _pkgRep.Select
        .WhereIf(input.Name.NotNull(), a => a.Name.Contains(input.Name))
        .OrderBy(a => new {a.ParentId, a.Sort})
        .ToListAsync<PkgGetListOutput>();

        return list;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<PkgGetPageOutput>> GetPageAsync(PageInput<PkgGetPageDto> input)
    {
        var key = input.Filter?.Name;

        var list = await _pkgRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync<PkgGetPageOutput>();

        var data = new PageOutput<PkgGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 查询套餐租户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<PkgGetPkgTenantListOutput>> GetPkgTenantListAsync([FromQuery] PkgGetPkgTenantListInput input)
    {
        using var _ = _tenantRep.Value.DataFilter.Disable(FilterNames.Tenant);

        var list = await _tenantRep.Value.Select.From<TenantPkgEntity, OrgEntity>((s, b, c) => s
            .InnerJoin(a => a.Id == b.TenantId)
            .InnerJoin(a => a.OrgId == c.Id))
            .Where((a, b, c) => b.PkgId == input.PkgId)
            .WhereIf(input.TenantName.NotNull(), (a, b, c) => c.Name.Contains(input.TenantName))
            .OrderByDescending((a, b, c) => b.Id)
            .ToListAsync((a, b, c) => new PkgGetPkgTenantListOutput { Id = a.Id, Name = c.Name, Code = c.Code });

        return list;
    }

    /// <summary>
    /// 查询套餐租户分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<PkgGetPkgTenantListOutput>> GetPkgTenantPageAsync(PageInput<PkgGetPkgTenantListInput> input)
    {
        using var _ = _tenantRep.Value.DataFilter.Disable(FilterNames.Tenant);

        var list = await _tenantRep.Value.Select.From<TenantPkgEntity, OrgEntity>((s, b, c) => s
            .InnerJoin(a => a.Id == b.TenantId)
            .InnerJoin(a => a.OrgId == c.Id))
            .Where((a, b, c) => b.PkgId == input.Filter.PkgId)
            .WhereIf(input.Filter.TenantName.NotNull(), (a, b, c) => c.Name.Contains(input.Filter.TenantName))
            .Count(out var total)
            .OrderByDescending((a, b, c) => b.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync((a, b, c) => new PkgGetPkgTenantListOutput { Id = a.Id, Name = c.Name, Code = c.Code });

        var data = new PageOutput<PkgGetPkgTenantListOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 查询套餐权限列表
    /// </summary>
    /// <param name="pkgId">套餐编号</param>
    /// <returns></returns>
    public async Task<List<long>> GetPkgPermissionListAsync(long pkgId)
    {
        var permissionIds = await _pkgPermissionRep.Value
            .Select.Where(d => d.PkgId == pkgId)
            .ToListAsync(a => a.PermissionId);

        return permissionIds;
    }

    /// <summary>
    /// 设置套餐权限
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SetPkgPermissionsAsync(PkgSetPkgPermissionsInput input)
    {
        //查询套餐权限
        var permissionIds = await _pkgPermissionRep.Value.Select.Where(d => d.PkgId == input.PkgId).ToListAsync(m => m.PermissionId);

        //批量删除套餐权限
        var deleteIds = permissionIds.Where(d => !input.PermissionIds.Contains(d));
        if (deleteIds.Any())
        {
            //删除套餐权限
            await _pkgPermissionRep.Value.DeleteAsync(m => m.PkgId == input.PkgId && deleteIds.Contains(m.PermissionId));
            //删除套餐下关联的角色权限
            await _rolePermissionRep.Value.DeleteAsync(a => deleteIds.Contains(a.PermissionId));
        }

        //批量插入套餐权限
        var pkgPermissions = new List<PkgPermissionEntity>();
        var insertPermissionIds = input.PermissionIds.Where(d => !permissionIds.Contains(d));
        if (insertPermissionIds.Any())
        {
            foreach (var permissionId in insertPermissionIds)
            {
                pkgPermissions.Add(new PkgPermissionEntity()
                {
                    PkgId = input.PkgId,
                    PermissionId = permissionId,
                });
            }
            await _pkgPermissionRep.Value.InsertAsync(pkgPermissions);
        }

        
        var tenantIds = await _tenantPkgRep.Select.Where(a => a.PkgId == input.PkgId).ToListAsync(a => a.TenantId);
        //清除租户下所有用户权限缓存
        await ClearUserPermissionsAsync(tenantIds);
    }

    /// <summary>
    /// 添加套餐租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task AddPkgTenantAsync(PkgAddPkgTenantListInput input)
    {
        var pkgId = input.PkgId;
        var tenantIds = await _tenantPkgRep.Select.Where(a => a.PkgId == pkgId).ToListAsync(a => a.TenantId);
        var insertTenantIds = input.TenantIds.Except(tenantIds);
        if (insertTenantIds != null && insertTenantIds.Any())
        {
            var tenantPkgList = insertTenantIds.Select(tenantId => new TenantPkgEntity 
            { 
                TenantId = tenantId, 
                PkgId = pkgId 
            }).ToList();
            await _tenantPkgRep.InsertAsync(tenantPkgList);

            //清除租户下所有用户权限缓存
            await ClearUserPermissionsAsync(insertTenantIds.ToList());
        }
    }

    /// <summary>
    /// 移除套餐租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task RemovePkgTenantAsync(PkgAddPkgTenantListInput input)
    {
        var tenantIds = input.TenantIds;
        if (tenantIds != null && tenantIds.Any())
        {
            await _tenantPkgRep.Where(a => a.PkgId == input.PkgId && input.TenantIds.Contains(a.TenantId)).ToDelete().ExecuteAffrowsAsync();

            //清除租户下所有用户权限缓存
            await ClearUserPermissionsAsync(tenantIds.ToList());
        }
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(PkgAddInput input)
    {
        if (await _pkgRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["此套餐名已存在"]);
        }

        if (input.Code.NotNull() && await _pkgRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["此套餐编码已存在"]);
        }

        var entity = Mapper.Map<PkgEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _pkgRep.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _pkgRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(PkgUpdateInput input)
    {
        var entity = await _pkgRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["套餐不存在"]);
        }

        if (await _pkgRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["此套餐名已存在"]);
        }

        if (input.Code.NotNull() && await _pkgRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["此套餐编码已存在"]);
        }

        Mapper.Map(input, entity);
        await _pkgRep.UpdateAsync(entity);
    }    

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var pkgIdList = await _pkgRep.GetChildIdListAsync(id);
        var tenantIds = await _tenantPkgRep.Select.Where(a => pkgIdList.Contains(a.PkgId)).ToListAsync(a => a.TenantId);

        //删除租户套餐
        await _tenantPkgRep.DeleteAsync(a => a.TenantId == id);
        //删除套餐权限
        await _pkgPermissionRep.Value.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        //删除套餐
        await _pkgRep.DeleteAsync(a => pkgIdList.Contains(a.Id));

        //清除租户下所有用户权限缓存
        await ClearUserPermissionsAsync(tenantIds);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        var pkgIdList = await _pkgRep.GetChildIdListAsync(ids);
        var tenantIds = await _tenantPkgRep.Select.Where(a => pkgIdList.Contains(a.PkgId)).ToListAsync(a => a.TenantId);

        //删除租户套餐
        await _tenantPkgRep.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        //删除套餐权限
        await _pkgPermissionRep.Value.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        //删除套餐
        await _pkgRep.Where(a => pkgIdList.Contains(a.Id)).AsTreeCte().ToDelete().ExecuteAffrowsAsync();

        //清除租户下所有用户权限缓存
        await ClearUserPermissionsAsync(tenantIds);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        var pkgIdList = await _pkgRep.GetChildIdListAsync(id);
        var tenantIds = await _tenantPkgRep.Select.Where(a => pkgIdList.Contains(a.PkgId)).ToListAsync(a => a.TenantId);
        await _tenantPkgRep.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        await _pkgPermissionRep.Value.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        await _pkgRep.SoftDeleteRecursiveAsync(a => pkgIdList.Contains(a.Id));

        //清除租户下所有用户权限缓存
        await ClearUserPermissionsAsync(tenantIds);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        var pkgIdList = await _pkgRep.GetChildIdListAsync(ids);
        var tenantIds = await _tenantPkgRep.Select.Where(a => ids.Contains(a.PkgId)).ToListAsync(a => a.TenantId);
        await _tenantPkgRep.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        await _pkgPermissionRep.Value.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        await _pkgRep.SoftDeleteRecursiveAsync(a => pkgIdList.Contains(a.Id));

        //清除租户下所有用户权限缓存
        await ClearUserPermissionsAsync(tenantIds);
    }
}