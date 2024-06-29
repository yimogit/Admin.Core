using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using ZhonTai.Module.Dev.Domain.CodeGroup;    
using ZhonTai.Module.Dev.Services.CodeGroupDetail.Dto;

namespace ZhonTai.Module.Dev.Services.CodeGroupDetail
{
    /// <summary>
    /// 模板明细服务
    /// </summary>
    public interface ICodeGroupDetailService
    {
        /// <summary>
        /// 查询
        /// </summary>
        Task<CodeGroupDetailGetOutput> GetAsync(long id);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        Task<PageOutput<CodeGroupDetailGetPageOutput>> GetPageAsync(PageInput<CodeGroupDetailGetPageInput> input);
        
        
        /// <summary>
        /// 列表查询
        /// </summary>
        Task<IEnumerable<CodeGroupDetailGetListOutput>> GetListAsync(CodeGroupDetailGetListInput input);
    
        /// <summary>
        /// 新增
        /// </summary>
        Task<long> AddAsync(CodeGroupDetailAddInput input);
        
        /// <summary>
        /// 编辑
        /// </summary>
        Task UpdateAsync(CodeGroupDetailUpdateInput input);
        
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

namespace ZhonTai.Module.Dev.Services.CodeGroupDetail.Dto
{
    
    /// <summary>模板明细列表查询结果输出</summary>
    public partial class CodeGroupDetailGetListOutput {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        /// <summary>模板名称</summary>
        public string Name { get; set; }
        /// <summary>模板分组</summary>
        public long GroupId { get; set; }
        ///<summary>模板分组显示文本</summary>
        public string? GroupId_Text { get; set; }
        /// <summary>生成路径</summary>
        public string? Path { get; set; }
        /// <summary>模板分组</summary>
        public string? GroupIds { get; set; }
        ///<summary>模板分组显示文本</summary>
        public List<string>? GroupIds_Texts { get; set; }
        ///<summary>页面使用的模板分组数组</summary>
        public List<string>? GroupIds_Values { get { return GroupIds?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(); } }
        /// <summary>模板内容</summary>
        public string Content { get; set; }
    }
    /// <summary>模板明细列表查询条件输入</summary>
    public partial class CodeGroupDetailGetListInput : CodeGroupDetailGetPageInput {
    }

    /// <summary>模板明细查询结果输出</summary>
    public partial class CodeGroupDetailGetOutput {
        public long Id { get; set; }
        /// <summary>模板名称</summary>
        public string Name { get; set; }
        /// <summary>模板分组</summary>
        public long GroupId { get; set; }
        ///<summary>模板分组显示文本</summary>
        public string? GroupId_Text { get; set; }
        /// <summary>生成路径</summary>
        public string? Path { get; set; }
        /// <summary>模板分组</summary>
        public string? GroupIds { get; set; }
        ///<summary>模板分组显示文本</summary>
        public List<string>? GroupIds_Texts { get; set; }
        ///<summary>页面使用的模板分组数组</summary>
        public List<string>? GroupIds_Values { get { return GroupIds?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(); } }
        /// <summary>模板内容</summary>
        public string Content { get; set; }
    }

    /// <summary>模板明细分页查询结果输出</summary>
    public partial class CodeGroupDetailGetPageOutput {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        /// <summary>模板名称</summary>
        public string Name { get; set; }
        /// <summary>模板分组</summary>
        public long GroupId { get; set; }
        ///<summary>模板分组显示文本</summary>
        public string? GroupId_Text { get; set; }
        /// <summary>生成路径</summary>
        public string? Path { get; set; }
        /// <summary>模板分组</summary>
        public string? GroupIds { get; set; }
        ///<summary>模板分组显示文本</summary>
        public List<string>? GroupIds_Texts { get; set; }
        ///<summary>页面使用的模板分组数组</summary>
        public List<string>? GroupIds_Values { get { return GroupIds?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(); } }
        /// <summary>模板内容</summary>
        public string Content { get; set; }
    }

    /// <summary>模板明细分页查询条件输入</summary>
    public partial class CodeGroupDetailGetPageInput {

        /// <summary>模板分组</summary>       
        public long? GroupId { get; set; }
    }
    
    /// <summary>模板明细新增输入</summary>
    public partial class CodeGroupDetailAddInput {
        /// <summary>模板名称</summary>
        [Required(ErrorMessage = "模板名称不能为空")]
        public string Name { get; set; }                                                    
        /// <summary>模板分组</summary>
        [Required(ErrorMessage = "模板分组不能为空")]
        public long GroupId { get; set; }                                                    
        /// <summary>生成路径</summary>
        public string? Path { get; set; }                                                    
        /// <summary>模板分组</summary>
        public string? GroupIds { get { return string.Join(',', GroupIds_Values ?? new List<string>()); } }
        ///<summary>页面提交的模板分组数组</summary>
        public List<string>? GroupIds_Values { get; set; }                                                    
        /// <summary>模板内容</summary>
        [Required(ErrorMessage = "模板内容不能为空")]
        public string Content { get; set; }                                                    
    }


    /// <summary>模板明细更新数据输入</summary>
    public partial class CodeGroupDetailUpdateInput {
        public long Id { get; set; }
        /// <summary>模板名称</summary>
        [Required(ErrorMessage = "模板名称不能为空")]
        public string Name { get; set; }
        /// <summary>模板分组</summary>
        [Required(ErrorMessage = "模板分组不能为空")]
        public long GroupId { get; set; }
        /// <summary>生成路径</summary>
        public string? Path { get; set; }
        /// <summary>模板分组</summary>
        public string? GroupIds { get { return string.Join(',', GroupIds_Values ?? new List<string>()); } }
        ///<summary>页面提交的模板分组数组</summary>
        public List<string>? GroupIds_Values { get; set; }
        /// <summary>模板内容</summary>
        [Required(ErrorMessage = "模板内容不能为空")]
        public string Content { get; set; }
    }


}