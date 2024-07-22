﻿using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using Yitter.IdGenerator;

namespace ZhonTai.Admin.Core.Configs;

/// <summary>
/// 应用配置
/// </summary>
public class AppConfig
{
    public AppType AppType { get; set; } = AppType.Controllers;

    /// <summary>
    /// Api地址，默认 http://*:8000
    /// </summary>
    public string[] Urls { get; set; }

    /// <summary>
    /// 跨域地址，默认 http://*:9000
    /// </summary>
    public string[] CorUrls { get; set; }

    /// <summary>
    /// 程序集名称
    /// </summary>
    public string[] AssemblyNames { get; set; }

    /// <summary>
    /// 租户类型
    /// </summary>
    public bool Tenant { get; set; } = false;

    /// <summary>
    /// 分布式事务唯一标识
    /// </summary>
    public string DistributeKey { get; set; }

    /// <summary>
    /// Swagger文档
    /// </summary>
    public SwaggerConfig Swagger { get; set; } = new SwaggerConfig();

    /// <summary>
    /// 新版Api文档
    /// </summary>
    public ApiUIConfig ApiUI { get; set; } = new ApiUIConfig();

    /// <summary>
    /// MiniProfiler性能分析器
    /// </summary>
    public bool MiniProfiler { get; set; } = false;

    /// <summary>
    /// 统一认证授权服务器
    /// </summary>
    public IdentityServer IdentityServer { get; set; } = new IdentityServer();

    /// <summary>
    /// Aop配置
    /// </summary>
    public AopConfig Aop { get; set; } = new AopConfig();

    /// <summary>
    /// 日志配置
    /// </summary>
    public LogConfig Log { get; set; } = new LogConfig();

    /// <summary>
    /// 验证配置
    /// </summary>
    public ValidateConfig Validate { get; set; } = new ValidateConfig();

    /// <summary>
    /// 限流
    /// </summary>
    public bool RateLimit { get; set; } = false;

    /// <summary>
    /// 验证码配置
    /// </summary>
    public VarifyCodeConfig VarifyCode { get; set; } = new VarifyCodeConfig();

    /// <summary>
    /// 默认密码
    /// </summary>
    public string DefaultPassword { get; set; } = "123asd";

    /// <summary>
    /// 动态Api配置
    /// </summary>
    public DynamicApiConfig DynamicApi { get; set; } = new DynamicApiConfig();

    /// <summary>
    /// 实现标准标识密码哈希
    /// </summary>
    public bool PasswordHasher { get; set; } = false;

    /// <summary>
    /// 最大请求大小
    /// </summary>
    [Obsolete("请使用 Kestrel: { MaxRequestBodySize: 104857600 }配置")]
    public long? MaxRequestBodySize { get; set; } = 104857600;

    /// <summary>
    /// Kestrel服务器
    /// </summary>
    public KestrelConfig Kestrel { get; set; } = new KestrelConfig();

    /// <summary>
    /// 健康检查配置
    /// </summary>
    public HealthChecksConfig HealthChecks { get; set; } = new HealthChecksConfig();

    /// <summary>
    /// 指定跨域访问时预检等待时间，以秒为单位，默认30分钟
    /// </summary>
    public int PreflightMaxAge { get; set; }

    /// <summary>
    /// 任务调度管理界面配置
    /// </summary>
    public TaskSchedulerUIConfig TaskSchedulerUI { get; set; } = new TaskSchedulerUIConfig();

    /// <summary>
    /// Id生成器配置
    /// </summary>
    public IdGeneratorConfig IdGenerator { get; set; } = new IdGeneratorConfig();

    /// <summary>
    /// 语言配置
    /// </summary>
    public LangConfig Lang { get; set; }
}

/// <summary>
/// Kestrel服务器配置
/// </summary>
public class KestrelConfig
{
    /// <summary>
    /// HTTP连接保活最长时间，单位秒
    /// </summary>
    public double KeepAliveTimeout { get; set; } = 130;

    /// <summary>
    /// 发送请求头最长时间，单位秒
    /// </summary>
    public double RequestHeadersTimeout { get; set; } = 30;

    /// <summary>
    /// 最大请求大小，单位bytes
    /// </summary>
    public long? MaxRequestBodySize { get; set; } = 30000000;
}

/// <summary>
/// 语言配置
/// </summary>
public class LangConfig
{
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = true;

    /// <summary>
    /// 默认语言
    /// </summary>
    public string DefaultLang { get; set; } = "zh";

    /// <summary>
    /// 语言列表
    /// </summary>
    public string[] Langs { get; set; }

    /// <summary>
    /// 语言请求解析列表
    /// </summary>
    public string[] RequestCultureProviders { get; set; }
}

/// <summary>
/// Swagger配置
/// </summary>
public class SwaggerConfig
{
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = false;

    /// <summary>
    /// 启用枚举架构过滤器
    /// </summary>
    public bool EnableEnumSchemaFilter { get; set; } = true;

    /// <summary>
    /// 启用接口排序文档过滤器
    /// </summary>
    public bool EnableOrderTagsDocumentFilter { get; set; } = true;

    /// <summary>
    /// 启用枚举属性名
    /// </summary>
    public bool EnableJsonStringEnumConverter { get; set; } = false;

    /// <summary>
    /// 启用SchemaId命名空间
    /// </summary>
    public bool EnableSchemaIdNamespace { get; set; } = false;

    /// <summary>
    /// 程序集列表
    /// </summary>
    public string[] AssemblyNameList { get; set; }

    private string _RoutePrefix = "swagger";
    /// <summary>
    /// 访问地址
    /// </summary>
    public string RoutePrefix { get => Regex.Replace(_RoutePrefix, "^\\/+|\\/+$", ""); set => _RoutePrefix = value; }

    /// <summary>
    /// 地址
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 项目列表
    /// </summary>
    public List<ProjectConfig> Projects { get; set; }
}

/// <summary>
///新版Api文档配置
/// </summary>
public class ApiUIConfig
{
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = false;


    private string _RoutePrefix="";
    /// <summary>
    /// 访问地址
    /// </summary>
    public string RoutePrefix { get => Regex.Replace(_RoutePrefix, "^\\/+|\\/+$", ""); set => _RoutePrefix = value; }

    public SwaggerFooterConfig Footer { get; set; } = new SwaggerFooterConfig();
}

/// <summary>
/// Swagger页脚配置
/// </summary>
public class SwaggerFooterConfig
{
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = false;

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }
}

/// <summary>
/// 统一认证授权服务器配置
/// </summary>
public class IdentityServer
{
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = false;

    /// <summary>
    /// 地址
    /// </summary>
    public string Url { get; set; } = "https://localhost:5000";

    /// <summary>
    /// 启用Https
    /// </summary>
    public bool RequireHttpsMetadata { get; set; } = false;

    /// <summary>
    /// 受众
    /// </summary>
    public string Audience { get; set; } = "admin.server.api";
}

/// <summary>
/// Aop配置
/// </summary>
public class AopConfig
{
    /// <summary>
    /// 事务
    /// </summary>
    public bool Transaction { get; set; } = true;
}

/// <summary>
/// 日志配置
/// </summary>
public class LogConfig
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public bool Operation { get; set; } = true;
}

/// <summary>
/// 验证配置
/// </summary>
public class ValidateConfig
{
    /// <summary>
    /// 登录
    /// </summary>
    public bool Login { get; set; } = true;

    /// <summary>
    /// 接口权限
    /// </summary>
    public bool Permission { get; set; } = true;

    /// <summary>
    /// 数据权限
    /// </summary>
    public bool DataPermission { get; set; } = true;

    /// <summary>
    /// 接口数据权限
    /// </summary>
    public bool ApiDataPermission { get; set; } = false;
}

/// <summary>
/// 验证码配置
/// </summary>
public class VarifyCodeConfig
{
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = true;

    /// <summary>
    /// 操作日志
    /// </summary>
    public string[] Fonts { get; set; }// = new[] { "Times New Roman", "Verdana", "Arial", "Gungsuh", "Impact" };
}

/// <summary>
/// 项目配置
/// </summary>
public class ProjectConfig
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
}

/// <summary>
/// 动态api配置
/// </summary>
public class DynamicApiConfig
{
    /// <summary>
    /// 结果格式化
    /// </summary>
    public bool FormatResult { get; set; } = true;
}

/// <summary>
/// 健康检查配置
/// </summary>
public class HealthChecksConfig
{
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = true;

    /// <summary>
    /// 访问路径
    /// </summary>
    public string Path { get; set; } = "/health";
}

/// <summary>
/// 任务调度管理界面
/// </summary>
public class TaskSchedulerUIConfig
{
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; } = false;

    /// <summary>
    /// 访问路径
    /// </summary>
    public string Path { get; set; } = "/task";
}

public class IdGeneratorConfig: IdGeneratorOptions
{
    public string CachePrefix { get; set; } = "zhontai:workerid:";
}

/// <summary>
/// 应用程序类型
/// </summary>
public enum AppType
{
    Controllers,
    ControllersWithViews,
    MVC
}