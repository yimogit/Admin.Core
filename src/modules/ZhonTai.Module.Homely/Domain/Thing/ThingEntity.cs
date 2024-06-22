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
        
        public string Name { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        /// <remarks></remarks>
        
        public string? ImageUrl { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        /// <remarks></remarks>
        
        public DateTime? AvailableDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <remarks></remarks>
        
        public string? Remark { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks></remarks>
        
        public int? Sort { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        /// <remarks></remarks>
        
        public long? CategoryId { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        /// <remarks></remarks>
        
        public string? TagIds { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        /// <remarks></remarks>
        [Column(StringLength=64)]
        public string? PhoneUrl { get; set; }
    }

}

#pragma warning restore CS8632

