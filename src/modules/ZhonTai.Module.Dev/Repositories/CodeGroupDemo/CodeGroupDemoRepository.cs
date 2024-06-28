using ZhonTai.Module.Dev.Domain.CodeGroupDemo;
using ZhonTai.Module.Dev.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;

namespace ZhonTai.Module.Dev.Repositories.CodeGroupDemo
{
    public class CodeGroupDemoRepository : RepositoryBase<CodeGroupDemoEntity>, ICodeGroupDemoRepository
    {
        public CodeGroupDemoRepository(UnitOfWorkManagerCloud muowm) : base(DbKeys.AppDb, muowm)
        {
        }
    }
}
