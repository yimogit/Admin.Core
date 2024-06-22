using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using ZhonTai.Module.Homely.Services.ThingTag.Dto;

namespace ZhonTai.Module.Homely.Services.ThingTag
{
    /// <summary>
    /// 物品标签服务
    /// </summary>
    public interface IThingTagService
    {
        /// <summary>
        /// 查询
        /// </summary>
        Task<ThingTagGetOutput> GetAsync(long id);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        Task<PageOutput<ThingTagGetPageOutput>> GetPageAsync(PageInput<ThingTagGetPageInput> input);
        
        
        /// <summary>
        /// 列表查询
        /// </summary>
        Task<IEnumerable<ThingTagGetListOutput>> GetListAsync(ThingTagGetListInput input);
    
        /// <summary>
        /// 新增
        /// </summary>
        Task<long> AddAsync(ThingTagAddInput input);
        
        /// <summary>
        /// 编辑
        /// </summary>
        Task UpdateAsync(ThingTagUpdateInput input);
        
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

namespace ZhonTai.Module.Homely.Services.ThingTag.Dto
{
    
    /// <summary>物品标签列表查询结果输出</summary>
    public partial class ThingTagGetListOutput {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        /// <summary>标签名称</summary>
        public string Name { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
    }
    /// <summary>物品标签列表查询条件输入</summary>
    public partial class ThingTagGetListInput : ThingTagGetPageInput {
    }

    /// <summary>物品标签查询结果输出</summary>
    public partial class ThingTagGetOutput {
        public long Id { get; set; }
        /// <summary>标签名称</summary>
        public string Name { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
    }

    /// <summary>物品标签分页查询结果输出</summary>
    public partial class ThingTagGetPageOutput {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        /// <summary>标签名称</summary>
        public string Name { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
    }

    /// <summary>物品标签分页查询条件输入</summary>
    public partial class ThingTagGetPageInput {

    }
    
    /// <summary>物品标签新增输入</summary>
    public partial class ThingTagAddInput {
        /// <summary>标签名称</summary>
        [Required(ErrorMessage = "标签名称不能为空")]
        public string Name { get; set; }                                                    
        /// <summary>排序</summary>
        public int? Sort { get; set; }                                                    
    }


    /// <summary>物品标签更新数据输入</summary>
    public partial class ThingTagUpdateInput {
        public long Id { get; set; }
        /// <summary>标签名称</summary>
        [Required(ErrorMessage = "标签名称不能为空")]
        public string Name { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
    }


}