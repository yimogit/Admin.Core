using ZhonTai.Module.Homely.Domain.ThingCategory;
using ZhonTai.Module.Homely.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;

namespace ZhonTai.Module.Homely.Repositories.ThingCategory
{
    public class ThingCategoryRepository : RepositoryBase<ThingCategoryEntity>, IThingCategoryRepository
    {
        public ThingCategoryRepository(UnitOfWorkManagerCloud muowm) : base(DbKeys.AppDb, muowm)
        {
        }
    }
}
