using System;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;

#pragma warning disable CS8632

namespace ZhonTai.Module.Homely.Domain.Thing
{
    /// <summary>
    /// 物品 实体类
    /// </summary>
    /// <remarks></remarks>
    [Table(Name="homely_thing")]
    public partial class ThingEntity: EntityBase
    {
        /// <summary>
        /// 物品名称
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=1)]
        public string Name { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=2)]
        public long? CategoryId { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=3)]
        public DateTime? AvailableDate { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=4)]
        public int? Sort { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=5)]
        public string? TagIds { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=6)]
        public string? Remark { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        /// <remarks></remarks>
        [Column(Position=7)]
        public string? ImageUrl { get; set; }
    }

}

#pragma warning restore CS8632

