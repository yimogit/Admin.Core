using System;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;

#pragma warning disable CS8632

namespace ZhonTai.Module.Homely.Domain.ThingCategory
{
    /// <summary>
    /// 物品分类 实体类
    /// </summary>
    /// <remarks></remarks>
    [Table(Name="homely_thing_category")]
    public partial class ThingCategoryEntity: EntityTenant
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        /// <remarks></remarks>
        [Column(Name="Name", Position=1, DbType="NVARCHAR", StringLength=300)]
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks></remarks>
        [Column(Name="Sort", Position=2, DbType="INTEGER")]
        public int? Sort { get; set; }
    }

}

#pragma warning restore CS8632

