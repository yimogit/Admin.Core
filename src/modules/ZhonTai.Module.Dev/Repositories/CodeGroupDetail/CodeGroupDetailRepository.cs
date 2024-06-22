using ZhonTai.Module.Dev.Domain.CodeGroupDetail;
using ZhonTai.Module.Dev.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;

namespace ZhonTai.Module.Dev.Repositories.CodeGroupDetail
{
    public class CodeGroupDetailRepository : RepositoryBase<CodeGroupDetailEntity>, ICodeGroupDetailRepository
    {
        public CodeGroupDetailRepository(UnitOfWorkManagerCloud muowm) : base(DbKeys.AppDb, muowm)
        {
        }
    }
}
