﻿using FreeSql;
using System;
using ZhonTai.Admin.Core.Consts;

namespace ZhonTai.Admin.Tools.TaskScheduler;

/// <summary>
/// TaskScheduler配置
/// </summary>
public class TaskSchedulerOptions
{
    /// <summary>
    /// 数据库键
    /// </summary>
    public string DbKey { get; set; } = DbKeys.AppDb;

    /// <summary>
    /// 数据库实例
    /// </summary>
    public IFreeSql FreeSql { get; set; }

    /// <summary>
    /// 多库实例
    /// </summary>
    public FreeSqlCloud FreeSqlCloud { get; set; }

    /// <summary>
    /// 配置FreeSql
    /// </summary>
    public Action<IFreeSql> ConfigureFreeSql { get; set; }

    /// <summary>
    /// 配置FreeSql
    /// </summary>
    public Action<FreeSchedulerBuilder> ConfigureFreeSchedulerBuilder { get; set; }
}