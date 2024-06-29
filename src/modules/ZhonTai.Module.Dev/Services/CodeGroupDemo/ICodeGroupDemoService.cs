using System.ComponentModel.DataAnnotations;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using ZhonTai.Module.Dev.Services.CodeGroupDemo.Dto;

namespace ZhonTai.Module.Dev.Services.CodeGroupDemo
{
    /// <summary>
    /// 模板演示服务
    /// </summary>
    public interface ICodeGroupDemoService
    {
        /// <summary>
        /// 查询
        /// </summary>
        Task<CodeGroupDemoGetOutput> GetAsync(long id);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        Task<PageOutput<CodeGroupDemoGetPageOutput>> GetPageAsync(PageInput<CodeGroupDemoGetPageInput> input);
        
        
        /// <summary>
        /// 列表查询
        /// </summary>
        Task<IEnumerable<CodeGroupDemoGetListOutput>> GetListAsync(CodeGroupDemoGetListInput input);
    
        /// <summary>
        /// 新增
        /// </summary>
        Task<long> AddAsync(CodeGroupDemoAddInput input);
        
        /// <summary>
        /// 编辑
        /// </summary>
        Task UpdateAsync(CodeGroupDemoUpdateInput input);
        
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

namespace ZhonTai.Module.Dev.Services.CodeGroupDemo.Dto
{
    
    /// <summary>模板演示列表查询结果输出</summary>
    public partial class CodeGroupDemoGetListOutput {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        /// <summary>文本框</summary>
        public string InputText { get; set; }
        /// <summary>数字框</summary>
        public int? InputNumber { get; set; }
        /// <summary>文本域</summary>
        public string? InputTextArea { get; set; }
        /// <summary>日期</summary>
        public DateTime? InputDate { get; set; }
        /// <summary>开关</summary>
        public bool? InputSwitch { get; set; }
        /// <summary>下拉框</summary>
        public string InputSelectCustom { get; set; }
        /// <summary>复选框</summary>
        public bool InputCheckbox { get; set; }
        /// <summary>字典</summary>
        public string InputSelectDict { get; set; }
        /// <summary>字典名称</summary>
        public string? InputSelectDictDictName { get; set; }
        /// <summary>模块业务单选</summary>
        public long? InputBussinessSingle { get; set; }
        ///<summary>模块业务单选显示文本</summary>
        public string? InputBussinessSingle_Text { get; set; }
        /// <summary>模块业务多选</summary>
        public string? InputBussinessMultiple { get; set; }
        ///<summary>模块业务多选显示文本</summary>
        public List<string>? InputBussinessMultiple_Texts { get; set; }
        ///<summary>页面使用的模块业务多选数组</summary>
        public List<string>? InputBussinessMultiple_Values { get { return InputBussinessMultiple?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(); } }
        /// <summary>图片上传</summary>
        public string? InputImage { get; set; }
        /// <summary>编辑器</summary>
        public string? InputEditor { get; set; }
    }
    /// <summary>模板演示列表查询条件输入</summary>
    public partial class CodeGroupDemoGetListInput : CodeGroupDemoGetPageInput {
    }

    /// <summary>模板演示查询结果输出</summary>
    public partial class CodeGroupDemoGetOutput {
        public long Id { get; set; }
        /// <summary>文本框</summary>
        public string InputText { get; set; }
        /// <summary>数字框</summary>
        public int? InputNumber { get; set; }
        /// <summary>文本域</summary>
        public string? InputTextArea { get; set; }
        /// <summary>日期</summary>
        public DateTime? InputDate { get; set; }
        /// <summary>开关</summary>
        public bool? InputSwitch { get; set; }
        /// <summary>下拉框</summary>
        public string InputSelectCustom { get; set; }
        /// <summary>复选框</summary>
        public bool InputCheckbox { get; set; }
        /// <summary>字典</summary>
        public string InputSelectDict { get; set; }
        /// <summary>模块业务单选</summary>
        public long? InputBussinessSingle { get; set; }
        ///<summary>模块业务单选显示文本</summary>
        public string? InputBussinessSingle_Text { get; set; }
        /// <summary>模块业务多选</summary>
        public string? InputBussinessMultiple { get; set; }
        ///<summary>模块业务多选显示文本</summary>
        public List<string>? InputBussinessMultiple_Texts { get; set; }
        ///<summary>页面使用的模块业务多选数组</summary>
        public List<string>? InputBussinessMultiple_Values { get { return InputBussinessMultiple?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(); } }
        /// <summary>图片上传</summary>
        public string? InputImage { get; set; }
        /// <summary>编辑器</summary>
        public string? InputEditor { get; set; }
    }

    /// <summary>模板演示分页查询结果输出</summary>
    public partial class CodeGroupDemoGetPageOutput {
        public long Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedTime { get; set; }
        /// <summary>文本框</summary>
        public string InputText { get; set; }
        /// <summary>数字框</summary>
        public int? InputNumber { get; set; }
        /// <summary>文本域</summary>
        public string? InputTextArea { get; set; }
        /// <summary>日期</summary>
        public DateTime? InputDate { get; set; }
        /// <summary>开关</summary>
        public bool? InputSwitch { get; set; }
        /// <summary>下拉框</summary>
        public string InputSelectCustom { get; set; }
        /// <summary>复选框</summary>
        public bool InputCheckbox { get; set; }
        /// <summary>字典</summary>
        public string InputSelectDict { get; set; }
        /// <summary>字典名称</summary>
        public string? InputSelectDictDictName { get; set; }
        /// <summary>模块业务单选</summary>
        public long? InputBussinessSingle { get; set; }
        ///<summary>模块业务单选显示文本</summary>
        public string? InputBussinessSingle_Text { get; set; }
        /// <summary>模块业务多选</summary>
        public string? InputBussinessMultiple { get; set; }
        ///<summary>模块业务多选显示文本</summary>
        public List<string>? InputBussinessMultiple_Texts { get; set; }
        ///<summary>页面使用的模块业务多选数组</summary>
        public List<string>? InputBussinessMultiple_Values { get { return InputBussinessMultiple?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(); } }
        /// <summary>图片上传</summary>
        public string? InputImage { get; set; }
        /// <summary>编辑器</summary>
        public string? InputEditor { get; set; }
    }

    /// <summary>模板演示分页查询条件输入</summary>
    public partial class CodeGroupDemoGetPageInput {

        /// <summary>文本框</summary>       
        public string? InputText { get; set; }
        /// <summary>数字框</summary>       
        public int? InputNumber { get; set; }
        /// <summary>日期</summary>       
        public DateTime? InputDate { get; set; }
        /// <summary>开关</summary>       
        public bool? InputSwitch { get; set; }
        /// <summary>下拉框</summary>       
        public string? InputSelectCustom { get; set; }
        /// <summary>字典</summary>       
        public string? InputSelectDict { get; set; }
        /// <summary>模块业务单选</summary>       
        public long? InputBussinessSingle { get; set; }
    }
    
    /// <summary>模板演示新增输入</summary>
    public partial class CodeGroupDemoAddInput {
        /// <summary>文本框</summary>
        [Required(ErrorMessage = "文本框不能为空")]
        public string InputText { get; set; }                                                    
        /// <summary>数字框</summary>
        public int? InputNumber { get; set; }                                                    
        /// <summary>文本域</summary>
        public string? InputTextArea { get; set; }                                                    
        /// <summary>日期</summary>
        public DateTime? InputDate { get; set; }                                                    
        /// <summary>开关</summary>
        public bool? InputSwitch { get; set; }                                                    
        /// <summary>下拉框</summary>
        [Required(ErrorMessage = "下拉框不能为空")]
        public string InputSelectCustom { get; set; }                                                    
        /// <summary>复选框</summary>
        [Required(ErrorMessage = "复选框不能为空")]
        public bool InputCheckbox { get; set; }                                                    
        /// <summary>字典</summary>
        [Required(ErrorMessage = "字典不能为空")]
        public string InputSelectDict { get; set; }                                                    
        /// <summary>模块业务单选</summary>
        public long? InputBussinessSingle { get; set; }                                                    
        /// <summary>模块业务多选</summary>
        public string? InputBussinessMultiple { get { return string.Join(',', InputBussinessMultiple_Values ?? new List<string>()); } }
        ///<summary>页面提交的模块业务多选数组</summary>
        public List<string>? InputBussinessMultiple_Values { get; set; }                                                    
        /// <summary>图片上传</summary>
        public string? InputImage { get; set; }                                                    
        /// <summary>编辑器</summary>
        public string? InputEditor { get; set; }                                                    
    }


    /// <summary>模板演示更新数据输入</summary>
    public partial class CodeGroupDemoUpdateInput {
        public long Id { get; set; }
        /// <summary>文本框</summary>
        [Required(ErrorMessage = "文本框不能为空")]
        public string InputText { get; set; }
        /// <summary>数字框</summary>
        public int? InputNumber { get; set; }
        /// <summary>文本域</summary>
        public string? InputTextArea { get; set; }
        /// <summary>日期</summary>
        public DateTime? InputDate { get; set; }
        /// <summary>开关</summary>
        public bool? InputSwitch { get; set; }
        /// <summary>下拉框</summary>
        [Required(ErrorMessage = "下拉框不能为空")]
        public string InputSelectCustom { get; set; }
        /// <summary>复选框</summary>
        [Required(ErrorMessage = "复选框不能为空")]
        public bool InputCheckbox { get; set; }
        /// <summary>字典</summary>
        [Required(ErrorMessage = "字典不能为空")]
        public string InputSelectDict { get; set; }
        /// <summary>模块业务单选</summary>
        public long? InputBussinessSingle { get; set; }
        /// <summary>模块业务多选</summary>
        public string? InputBussinessMultiple { get { return string.Join(',', InputBussinessMultiple_Values ?? new List<string>()); } }
        ///<summary>页面提交的模块业务多选数组</summary>
        public List<string>? InputBussinessMultiple_Values { get; set; }
        /// <summary>图片上传</summary>
        public string? InputImage { get; set; }
        /// <summary>编辑器</summary>
        public string? InputEditor { get; set; }
    }


}