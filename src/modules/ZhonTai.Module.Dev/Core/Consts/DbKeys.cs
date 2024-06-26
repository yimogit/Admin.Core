
using ZhonTai.Admin.Core.Attributes;
using System.ComponentModel;

namespace ZhonTai.Module.Dev.Core.Consts;

/// <summary>
/// 数据库键名
/// </summary>
public class DbKeys
{
    /// <summary>
    /// 数据库注册键
    /// </summary>
    [Description("数据库注册键")]
    public static string AppDb { get; set; } = Admin.Core.Consts.DbKeys.AppDb;//"admindb";

}