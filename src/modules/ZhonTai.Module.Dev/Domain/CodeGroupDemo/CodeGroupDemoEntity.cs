using System;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;

#pragma warning disable CS8632

namespace ZhonTai.Module.Dev.Domain.CodeGroupDemo
{
    /// <summary>
    /// 模板演示 实体类
    /// </summary>
    /// <remarks></remarks>
    [Table(Name="dev_code_demo")]
    public partial class CodeGroupDemoEntity: EntityBase
    {
        /// <summary>
        /// 文本框
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=1)]
        public string InputText { get; set; }
        /// <summary>
        /// 数字框
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=2)]
        public int? InputNumber { get; set; }
        /// <summary>
        /// 文本域
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=3)]
        public string? InputTextArea { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=4)]
        public DateTime? InputDate { get; set; }
        /// <summary>
        /// 开关
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=5)]
        public bool? InputSwitch { get; set; }
        /// <summary>
        /// 下拉框
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=6)]
        public string InputSelectCustom { get; set; }
        /// <summary>
        /// 复选框
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=7)]
        public bool InputCheckbox { get; set; }
        /// <summary>
        /// 字典
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=8)]
        public string InputSelectDict { get; set; }
        /// <summary>
        /// 模块业务单选
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=9)]
        public long? InputBussinessSingle { get; set; }
        /// <summary>
        /// 模块业务多选
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=10)]
        public string? InputBussinessMultiple { get; set; }
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=11)]
        public string? InputImage { get; set; }
        /// <summary>
        /// 编辑器
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=12)]
        public string? InputEditor { get; set; }
    }

}

#pragma warning restore CS8632

