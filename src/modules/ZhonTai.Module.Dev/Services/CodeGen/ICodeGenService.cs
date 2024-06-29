using ZhonTai.Module.Dev.Dto;

namespace ZhonTai.Module.Dev;

/// <summary>
/// 代码生成接口
/// </summary>
public interface ICodeGenService
{
    Task<BaseDataGetOutput> GetBaseDataAsync();
    Task<IEnumerable<CodeGenGetOutput>> GetTablesAsync(String dbkey);
    Task<IEnumerable<CodeGenGetOutput>> GetListAsync(String dbkey,String tableName);
    Task<CodeGenGetOutput> GetAsync(long id);
    Task UpdateAsync(CodeGenUpdateInput input);
    Task DeleteAsync(long id);
    Task GenerateAsync(long id);
    Task<string> CompileAsync(long id);
    Task GenMenu(long id);
}