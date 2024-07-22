using System.Threading.Tasks;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Db.Data;
using FreeSql;
using System.Linq;
using System;
using ZhonTai.Admin.Core.Entities;
using ZhonTai.Module.Dev.Core.Consts;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Module.Dev.Domain.CodeGen;
using ZhonTai.Module.Dev.Domain.CodeGroupDemo;

namespace ZhonTai.Module.Dev.Repositories;

public class CustomSyncData : SyncData, ISyncData
{
    /// <summary>
    /// 同步InitData/Dev/*.json数据
    /// </summary>
    /// <param name="db"></param>
    /// <param name="dbConfig"></param>
    /// <param name="appConfig"></param>
    /// <returns></returns>
    public virtual async Task SyncDataAsync(IFreeSql db, DbConfig dbConfig = null, AppConfig appConfig = null)
    {
        using var unitOfWork = db.CreateUnitOfWork();

        try
        {
            //读取数据目录
            var readPath = DevConsts.InitFilePath;
            await SyncEntityAsync<CodeGenEntity>(db, unitOfWork, dbConfig, appConfig, readPath);
            await SyncEntityAsync<CodeGenFieldEntity>(db, unitOfWork, dbConfig, appConfig, readPath);
            await SyncEntityAsync<CodeGroupDemoEntity>(db, unitOfWork, dbConfig, appConfig, readPath);

            unitOfWork.Commit();
        }
        catch (Exception)
        {
            unitOfWork.Rollback();
            //throw;
        }
    }
}
