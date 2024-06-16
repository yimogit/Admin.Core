using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using ZhonTai.Module.Homely.Services.ThingCategory.Dto;

namespace ZhonTai.Module.Homely.Services.ThingCategory
{
    public interface IThingCategoryService
    {
        Task<ThingCategoryGetOutput> GetAsync(long id);
        
        Task<PageOutput<ThingCategoryGetPageOutput>> GetPageAsync(PageInput<ThingCategoryGetPageInput> input);
        
        Task<long> AddAsync(ThingCategoryAddInput input);

        Task UpdateAsync(ThingCategoryUpdateInput input);

        Task<bool> DeleteAsync(long id);
        
        Task<IEnumerable<ThingCategoryGetListOutput>> GetListAsync(ThingCategoryGetListInput input);
        
        Task<bool> SoftDeleteAsync(long id);
        
        Task<bool> BatchSoftDeleteAsync(long[] ids);
    }
}

namespace ZhonTai.Module.Homely.Services.ThingCategory.Dto
{
    /// <summary>物品分类查询结果输出</summary>
    public partial class ThingCategoryGetOutput {
        public long Id { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
        /// <summary>分类名称</summary>
        public string? Name { get; set; }
        /// <summary>租户</summary>
        public long? TenantId { get; set; }
    }

    /// <summary>物品分类分页查询结果输出</summary>
    public partial class ThingCategoryGetPageOutput {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
        /// <summary>分类名称</summary>
        public string? Name { get; set; }
    }

    /// <summary>物品分类分页查询条件输入</summary>
    public partial class ThingCategoryGetPageInput {

        /// <summary>分类名称</summary>       
        public string? Name { get; set; }
    }
    
    /// <summary>物品分类新增输入</summary>
    public partial class ThingCategoryAddInput {
        /// <summary>排序</summary>
        public int? Sort { get; set; }                                                    
        /// <summary>分类名称</summary>
        public string? Name { get; set; }                                                    
    }


    /// <summary>物品分类更新数据输入</summary>
    public partial class ThingCategoryUpdateInput {
        public long Id { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
        /// <summary>分类名称</summary>
        public string? Name { get; set; }
    }

    /// <summary>物品分类列表查询结果输出</summary>
    public partial class ThingCategoryGetListOutput {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
        /// <summary>分类名称</summary>
        public string? Name { get; set; }
        /// <summary>租户</summary>
        public long? TenantId { get; set; }
    }
    /// <summary>物品分类列表查询条件输入</summary>
    public partial class ThingCategoryGetListInput : ThingCategoryGetPageInput {
    }

}