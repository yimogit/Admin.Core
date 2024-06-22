using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using ZhonTai.Module.Homely.Services.ThingCategory.Dto;

namespace ZhonTai.Module.Homely.Services.ThingCategory
{
    /// <summary>
    /// 物品分类服务
    /// </summary>
    public interface IThingCategoryService
    {
        /// <summary>
        /// 查询
        /// </summary>
        Task<ThingCategoryGetOutput> GetAsync(long id);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        Task<PageOutput<ThingCategoryGetPageOutput>> GetPageAsync(PageInput<ThingCategoryGetPageInput> input);
        
        
        /// <summary>
        /// 列表查询
        /// </summary>
        Task<IEnumerable<ThingCategoryGetListOutput>> GetListAsync(ThingCategoryGetListInput input);
    
        /// <summary>
        /// 新增
        /// </summary>
        Task<long> AddAsync(ThingCategoryAddInput input);
        
        /// <summary>
        /// 编辑
        /// </summary>
        Task UpdateAsync(ThingCategoryUpdateInput input);
        
        /// <summary>
        /// 删除
        /// </summary>
        Task<bool> DeleteAsync(long id);

        /// <summary>
        /// 软删除
        /// </summary>
        Task<bool> SoftDeleteAsync(long id);
        /// <summary>
        /// 批量软删除
        /// </summary>
        Task<bool> BatchSoftDeleteAsync(long[] ids);
    }
}

namespace ZhonTai.Module.Homely.Services.ThingCategory.Dto
{
    
    /// <summary>物品分类列表查询结果输出</summary>
    public partial class ThingCategoryGetListOutput {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        /// <summary>分类名称</summary>
        public string Name { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
    }
    /// <summary>物品分类列表查询条件输入</summary>
    public partial class ThingCategoryGetListInput : ThingCategoryGetPageInput {
    }

    /// <summary>物品分类查询结果输出</summary>
    public partial class ThingCategoryGetOutput {
        public long Id { get; set; }
        /// <summary>分类名称</summary>
        public string Name { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
    }

    /// <summary>物品分类分页查询结果输出</summary>
    public partial class ThingCategoryGetPageOutput {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        /// <summary>分类名称</summary>
        public string Name { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
    }

    /// <summary>物品分类分页查询条件输入</summary>
    public partial class ThingCategoryGetPageInput {

        /// <summary>分类名称</summary>       
        public string? Name { get; set; }
    }
    
    /// <summary>物品分类新增输入</summary>
    public partial class ThingCategoryAddInput {
        /// <summary>分类名称</summary>
        [Required(ErrorMessage = "分类名称不能为空")]
        public string Name { get; set; }                                                    
        /// <summary>排序</summary>
        public int? Sort { get; set; }                                                    
    }


    /// <summary>物品分类更新数据输入</summary>
    public partial class ThingCategoryUpdateInput {
        public long Id { get; set; }
        /// <summary>分类名称</summary>
        [Required(ErrorMessage = "分类名称不能为空")]
        public string Name { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
    }


}