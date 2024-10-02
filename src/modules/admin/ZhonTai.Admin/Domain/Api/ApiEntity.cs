﻿using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using ZhonTai.Admin.Domain.PermissionApi;
using ZhonTai.Admin.Domain.Permission;
using ZhonTai.Admin.Core.Entities;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Domain.Api;

/// <summary>
/// 接口管理
/// </summary>
[Table(Name = "ad_api")]
[Index("idx_{tablename}_01", nameof(ParentId) + "," + nameof(Path), true)]
public partial class ApiEntity : EntityBase, IChilds<ApiEntity>
{
    /// <summary>
    /// 所属模块
    /// </summary>
	public long ParentId { get; set; }

    /// <summary>
    /// 接口命名
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; }

    /// <summary>
    /// 接口名称
    /// </summary>
    [Column(StringLength = 500)]
    public string Label { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    [Column(StringLength = 500)]
    public string Path { get; set; }

    /// <summary>
    /// 接口提交方法
    /// </summary>
    [Column(StringLength = 50)]
    public string HttpMethods { get; set; }

    /// <summary>
    /// 启用请求参数
    /// </summary>
    public bool EnabledParams { get; set; } = false;

    /// <summary>
    /// 启用响应结果
    /// </summary>
    public bool EnabledResult { get; set; } = false;

    /// <summary>
    /// 说明
    /// </summary>
    [Column(StringLength = 500)]
    public string Description { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    [Navigate(nameof(ParentId))]
    public List<ApiEntity> Childs { get; set; }

    [NotGen]
    [Navigate(ManyToMany = typeof(PermissionApiEntity))]
    public ICollection<PermissionEntity> Permissions { get; set; }
}