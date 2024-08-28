using MyApp.Api.Core.Repositories;
using MyApp.Api.Domain.Module;
using ZhonTai.Admin.Core.Db.Transaction;

namespace MyApp.Api.Repositories;

/// <summary>
/// ģ��ִ�
/// </summary>
public class ModuleRepository : AppRepositoryBase<ModuleEntity>, IModuleRepository
{
    public ModuleRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}