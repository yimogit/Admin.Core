using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using ZhonTai.Module.Homely.Services.Thing.Dto;

namespace ZhonTai.Module.Homely.Services.Thing
{
    public interface IThingService
    {
        Task<ThingGetOutput> GetAsync(long id);
        
        Task<PageOutput<ThingGetPageOutput>> GetPageAsync(PageInput<ThingGetPageInput> input);
        
        Task<long> AddAsync(ThingAddInput input);

        Task UpdateAsync(ThingUpdateInput input);

        Task<bool> DeleteAsync(long id);
        
        Task<IEnumerable<ThingGetListOutput>> GetListAsync(ThingGetListInput input);
        
        Task<bool> SoftDeleteAsync(long id);
        
        Task<bool> BatchSoftDeleteAsync(long[] ids);
    }
}

namespace ZhonTai.Module.Homely.Services.Thing.Dto
{
    /// <summary>物品查询结果输出</summary>
    public partial class ThingGetOutput {
        public long Id { get; set; }
        /// <summary>物品名称</summary>
        public string Name { get; set; }
        /// <summary>图片</summary>
        public string? ImageUrl { get; set; }
        /// <summary>有效期</summary>
        public DateTime? AvailableDate { get; set; }
        /// <summary>备注</summary>
        public string? Remark { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
        /// <summary>分类</summary>
        public long? CategoryId { get; set; }
        /// <summary>标签</summary>
        public string? Tags { get; set; }
    }

    /// <summary>物品分页查询结果输出</summary>
    public partial class ThingGetPageOutput {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        /// <summary>物品名称</summary>
        public string Name { get; set; }
        /// <summary>图片</summary>
        public string? ImageUrl { get; set; }
        /// <summary>有效期</summary>
        public DateTime? AvailableDate { get; set; }
        /// <summary>备注</summary>
        public string? Remark { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
        /// <summary>分类</summary>
        public long? CategoryId { get; set; }
        /// <summary>标签</summary>
        public string? Tags { get; set; }
    }

    /// <summary>物品分页查询条件输入</summary>
    public partial class ThingGetPageInput {

        /// <summary>物品名称</summary>       
        public string? Name { get; set; }
        /// <summary>有效期</summary>       
        public DateTime? AvailableDate { get; set; }
        /// <summary>备注</summary>       
        public string? Remark { get; set; }
    }
    
    /// <summary>物品新增输入</summary>
    public partial class ThingAddInput {
        /// <summary>物品名称</summary>
        [Required(ErrorMessage = "物品名称不能为空")]
        public string Name { get; set; }                                                    
        /// <summary>图片</summary>
        public string? ImageUrl { get; set; }                                                    
        /// <summary>有效期</summary>
        public DateTime? AvailableDate { get; set; }                                                    
        /// <summary>备注</summary>
        public string? Remark { get; set; }                                                    
        /// <summary>排序</summary>
        public int? Sort { get; set; }                                                    
        /// <summary>分类</summary>
        public long? CategoryId { get; set; }                                                    
        /// <summary>标签</summary>
        public string? Tags { get; set; }                                                    
    }


    /// <summary>物品更新数据输入</summary>
    public partial class ThingUpdateInput {
        public long Id { get; set; }
        /// <summary>物品名称</summary>
        [Required(ErrorMessage = "物品名称不能为空")]
        public string Name { get; set; }
        /// <summary>图片</summary>
        public string? ImageUrl { get; set; }
        /// <summary>有效期</summary>
        public DateTime? AvailableDate { get; set; }
        /// <summary>备注</summary>
        public string? Remark { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
        /// <summary>分类</summary>
        public long? CategoryId { get; set; }
        /// <summary>标签</summary>
        public string? Tags { get; set; }
    }

    /// <summary>物品列表查询结果输出</summary>
    public partial class ThingGetListOutput {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        /// <summary>物品名称</summary>
        public string Name { get; set; }
        /// <summary>图片</summary>
        public string? ImageUrl { get; set; }
        /// <summary>有效期</summary>
        public DateTime? AvailableDate { get; set; }
        /// <summary>备注</summary>
        public string? Remark { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
        /// <summary>分类</summary>
        public long? CategoryId { get; set; }
        /// <summary>标签</summary>
        public string? Tags { get; set; }
    }
    /// <summary>物品列表查询条件输入</summary>
    public partial class ThingGetListInput : ThingGetPageInput {
    }

}