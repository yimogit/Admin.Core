using ZhonTai.Module.Dev.Domain.CodeGroup;
using ZhonTai.Module.Dev.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;

namespace ZhonTai.Module.Dev.Repositories.CodeGroup
{
    public class CodeGroupRepository : RepositoryBase<CodeGroupEntity>, ICodeGroupRepository
    {
        public CodeGroupRepository(UnitOfWorkManagerCloud muowm) : base(DbKeys.AppDb, muowm)
        {
        }
    }
}
