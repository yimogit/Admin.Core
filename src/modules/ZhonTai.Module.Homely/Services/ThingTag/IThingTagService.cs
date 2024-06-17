using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using ZhonTai.Module.Homely.Services.ThingTag.Dto;

namespace ZhonTai.Module.Homely.Services.ThingTag
{
    public interface IThingTagService
    {
        Task<ThingTagGetOutput> GetAsync(long id);
        
        Task<PageOutput<ThingTagGetPageOutput>> GetPageAsync(PageInput<ThingTagGetPageInput> input);
        
        Task<long> AddAsync(ThingTagAddInput input);

        Task UpdateAsync(ThingTagUpdateInput input);

        Task<bool> DeleteAsync(long id);
        
        Task<bool> SoftDeleteAsync(long id);
        
        Task<bool> BatchSoftDeleteAsync(long[] ids);
    }
}

namespace ZhonTai.Module.Homely.Services.ThingTag.Dto
{
    /// <summary>标签查询结果输出</summary>
    public partial class ThingTagGetOutput {
        public long Id { get; set; }
        /// <summary>标签名称</summary>
        public string? Name { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
    }

    /// <summary>标签分页查询结果输出</summary>
    public partial class ThingTagGetPageOutput {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        /// <summary>标签名称</summary>
        public string? Name { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
    }

    /// <summary>标签分页查询条件输入</summary>
    public partial class ThingTagGetPageInput {

    }
    
    /// <summary>标签新增输入</summary>
    public partial class ThingTagAddInput {
        /// <summary>标签名称</summary>
        public string? Name { get; set; }                                                    
        /// <summary>排序</summary>
        public int? Sort { get; set; }                                                    
    }


    /// <summary>标签更新数据输入</summary>
    public partial class ThingTagUpdateInput {
        public long Id { get; set; }
        /// <summary>标签名称</summary>
        public string? Name { get; set; }
        /// <summary>排序</summary>
        public int? Sort { get; set; }
    }


}