using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Core.Db.Data;
using ZhonTai.Admin.Core.Entities;
using ZhonTai.Module.Dev.Core.Consts;
using ZhonTai.Module.Dev.Domain.CodeGen;
using ZhonTai.Module.Dev.Domain.CodeGroupDemo;

namespace ZhonTai.Module.Dev.Repositories;

public class CustomGenerateData : GenerateData, IGenerateData
{
    /// <summary>
    /// 保存实体数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="appConfig">应用配置</param>
    /// <param name="outPath">输出路径 InitData/xxx </param>
    /// <returns></returns>
    protected virtual async Task SaveEntityAsync<T>(IFreeSql db, AppConfig appConfig, string outPath) where T : EntityBase, new()
    {
        var modules = await db.Queryable<T>().ToListAsync();
        //是否多租户
        var isTenant = appConfig.Tenant && typeof(T).IsAssignableFrom(typeof(EntityTenant));
        if (isTenant)
        {
            var tenantIds = await db.Queryable<TenantEntity>().ToListAsync(a => a.Id);
            SaveDataToJsonFile<T>(modules.Where(a => (a as EntityTenant).TenantId > 0 && tenantIds.Contains((a as EntityTenant).TenantId.Value)), isTenant, outPath);
        }

        SaveDataToJsonFile<T>(modules, isTenant, outPath);
    }

    /// <summary>
    /// 生成数据到InitData/Dev 手动新建Dev目录
    /// </summary>
    /// <param name="db"></param>
    /// <param name="appConfig"></param>
    /// <returns></returns>
    public virtual async Task GenerateDataAsync(IFreeSql db, AppConfig appConfig)
    {
        //生成数据目录
        var outPath = DevConsts.InitFilePath;
        await SaveEntityAsync<CodeGenEntity>(db, appConfig, outPath);
        await SaveEntityAsync<CodeGenFieldEntity>(db, appConfig, outPath);
        await SaveEntityAsync<CodeGroupDemoEntity>(db, appConfig, outPath);
    }
}
