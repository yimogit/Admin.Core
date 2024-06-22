using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using ZhonTai.Module.Homely.Services.Thing.Dto;

namespace ZhonTai.Module.Homely.Services.Thing
{
    /// <summary>
    /// 物品服务
    /// </summary>
    public interface IThingService
    {
        /// <summary>
        /// 查询
        /// </summary>
        Task<ThingGetOutput> GetAsync(long id);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        Task<PageOutput<ThingGetPageOutput>> GetPageAsync(PageInput<ThingGetPageInput> input);
        
        
        /// <summary>
        /// 列表查询
        /// </summary>
        Task<IEnumerable<ThingGetListOutput>> GetListAsync(ThingGetListInput input);
    
        /// <summary>
        /// 新增
        /// </summary>
        Task<long> AddAsync(ThingAddInput input);
        
        /// <summary>
        /// 编辑
        /// </summary>
        Task UpdateAsync(ThingUpdateInput input);
        
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

namespace ZhonTai.Module.Homely.Services.Thing.Dto
{
    
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
        ///<summary>分类显示文本</summary>
        public string? CategoryId_Text { get; set; }
        /// <summary>标签</summary>
        public string? TagIds { get; set; }
        ///<summary>标签显示文本</summary>
        public List<string>? TagIds_Texts { get; set; }
        ///<summary>页面使用的标签数组</summary>
        public List<string>? TagIds_Values { get { return TagIds?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(); } }
        /// <summary>头像</summary>
        public string? PhoneUrl { get; set; }
    }
    /// <summary>物品列表查询条件输入</summary>
    public partial class ThingGetListInput : ThingGetPageInput {
    }

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
        ///<summary>分类显示文本</summary>
        public string? CategoryId_Text { get; set; }
        /// <summary>标签</summary>
        public string? TagIds { get; set; }
        ///<summary>标签显示文本</summary>
        public List<string>? TagIds_Texts { get; set; }
        ///<summary>页面使用的标签数组</summary>
        public List<string>? TagIds_Values { get { return TagIds?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(); } }
        /// <summary>头像</summary>
        public string? PhoneUrl { get; set; }
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
        ///<summary>分类显示文本</summary>
        public string? CategoryId_Text { get; set; }
        /// <summary>标签</summary>
        public string? TagIds { get; set; }
        ///<summary>标签显示文本</summary>
        public List<string>? TagIds_Texts { get; set; }
        ///<summary>页面使用的标签数组</summary>
        public List<string>? TagIds_Values { get { return TagIds?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(); } }
        /// <summary>头像</summary>
        public string? PhoneUrl { get; set; }
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
        public string? TagIds { get { return string.Join(',', TagIds_Values ?? new List<string>()); } }
        ///<summary>页面提交的标签数组</summary>
        public List<string>? TagIds_Values { get; set; }                                                    
        /// <summary>头像</summary>
        public string? PhoneUrl { get; set; }                                                    
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
        public string? TagIds { get { return string.Join(',', TagIds_Values ?? new List<string>()); } }
        ///<summary>页面提交的标签数组</summary>
        public List<string>? TagIds_Values { get; set; }
        /// <summary>头像</summary>
        public string? PhoneUrl { get; set; }
    }


}