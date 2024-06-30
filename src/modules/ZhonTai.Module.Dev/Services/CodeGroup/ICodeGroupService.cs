using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using ZhonTai.Module.Dev.Services.CodeGroup.Dto;

namespace ZhonTai.Module.Dev.Services.CodeGroup
{
    /// <summary>
    /// 模板组服务
    /// </summary>
    public interface ICodeGroupService
    {
        /// <summary>
        /// 查询
        /// </summary>
        Task<CodeGroupGetOutput> GetAsync(long id);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        Task<PageOutput<CodeGroupGetPageOutput>> GetPageAsync(PageInput<CodeGroupGetPageInput> input);
        
        
        /// <summary>
        /// 列表查询
        /// </summary>
        Task<IEnumerable<CodeGroupGetListOutput>> GetListAsync(CodeGroupGetListInput input);
    
        /// <summary>
        /// 新增
        /// </summary>
        Task<long> AddAsync(CodeGroupAddInput input);
        
        /// <summary>
        /// 编辑
        /// </summary>
        Task UpdateAsync(CodeGroupUpdateInput input);
        
        /// <summary>
        /// 删除
        /// </summary>
        Task<bool> DeleteAsync(long id);

        /// <summary>
        /// 软删除
        /// </summary>
        Task<bool> SoftDeleteAsync(long id);
    }
}

namespace ZhonTai.Module.Dev.Services.CodeGroup.Dto
{
    
    /// <summary>模板组列表查询结果输出</summary>
    public partial class CodeGroupGetListOutput {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        /// <summary>模板组名称</summary>
        public string Name { get; set; }
        /// <summary>备注</summary>
        public string? Remark { get; set; }
    }
    /// <summary>模板组列表查询条件输入</summary>
    public partial class CodeGroupGetListInput : CodeGroupGetPageInput {
    }

    /// <summary>模板组查询结果输出</summary>
    public partial class CodeGroupGetOutput {
        public long Id { get; set; }
        /// <summary>模板组名称</summary>
        public string Name { get; set; }
        /// <summary>备注</summary>
        public string? Remark { get; set; }
    }

    /// <summary>模板组分页查询结果输出</summary>
    public partial class CodeGroupGetPageOutput {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        /// <summary>模板组名称</summary>
        public string Name { get; set; }
        /// <summary>备注</summary>
        public string? Remark { get; set; }
    }

    /// <summary>模板组分页查询条件输入</summary>
    public partial class CodeGroupGetPageInput {

        /// <summary>模板组名称</summary>       
        public string? Name { get; set; }
    }
    
    /// <summary>模板组新增输入</summary>
    public partial class CodeGroupAddInput {
        /// <summary>模板组名称</summary>
        [Required(ErrorMessage = "模板组名称不能为空")]
        public string Name { get; set; }                                                    
        /// <summary>备注</summary>
        public string? Remark { get; set; }                                                    
    }


    /// <summary>模板组更新数据输入</summary>
    public partial class CodeGroupUpdateInput {
        public long Id { get; set; }
        /// <summary>模板组名称</summary>
        [Required(ErrorMessage = "模板组名称不能为空")]
        public string Name { get; set; }
        /// <summary>备注</summary>
        public string? Remark { get; set; }
    }


}