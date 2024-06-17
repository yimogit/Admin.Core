using System;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;

#pragma warning disable CS8632

namespace ZhonTai.Module.Homely.Domain.ThingTag
{
    /// <summary>
    /// 标签 实体类
    /// </summary>
    /// <remarks></remarks>
    [Table(Name="homely_thing_tag")]
    public partial class ThingTagEntity: EntityBase
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        /// <remarks></remarks>
        [Column(StringLength=300)]
        public string? Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks></remarks>
        [Column(Precision = 64)]
        public int? Sort { get; set; }
    }

}

#pragma warning restore CS8632

