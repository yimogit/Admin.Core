using System;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;
using ZhonTai.Module.Dev.Domain.CodeGroup;    

#pragma warning disable CS8632

namespace ZhonTai.Module.Dev.Domain.CodeGroupDetail
{
    /// <summary>
    /// 模板明细 实体类
    /// </summary>
    /// <remarks></remarks>
    [Table(Name="dev_code_groupdetail")]
    public partial class CodeGroupDetailEntity: EntityBase
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=1, StringLength=200)]
        public string Name { get; set; }
        /// <summary>
        /// 模板分组
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=2, Precision = 64)]
        public long GroupId { get; set; }
        /// <summary>
        /// 生成路径
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=3, StringLength=200)]
        public string? Path { get; set; }
        /// <summary>
        /// 模板分组
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=4, StringLength=200)]
        public string? GroupIds { get; set; }
        /// <summary>
        /// 模板内容
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=5, StringLength=4000)]
        public string Content { get; set; }
    }

}

#pragma warning restore CS8632

