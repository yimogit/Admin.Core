using ZhonTai.Module.Homely.Domain.ThingTag;
using ZhonTai.Module.Homely.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;

namespace ZhonTai.Module.Homely.Repositories.ThingTag
{
    public class ThingTagRepository : RepositoryBase<ThingTagEntity>, IThingTagRepository
    {
        public ThingTagRepository(UnitOfWorkManagerCloud muowm) : base(DbKeys.AppDb, muowm)
        {
        }
    }
}
