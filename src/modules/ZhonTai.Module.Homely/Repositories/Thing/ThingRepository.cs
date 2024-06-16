using ZhonTai.Module.Homely.Domain.Thing;
using ZhonTai.Module.Homely.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;

namespace ZhonTai.Module.Homely.Repositories.Thing
{
    public class ThingRepository : RepositoryBase<ThingEntity>, IThingRepository
    {
        public ThingRepository(UnitOfWorkManagerCloud muowm) : base(DbKeys.AppDb, muowm)
        {
        }
    }
}
