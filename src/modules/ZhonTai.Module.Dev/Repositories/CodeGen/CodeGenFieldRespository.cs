using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Module.Dev.Domain.CodeGen;

namespace ZhonTai.Admin.Repositories;

public class CodeGenFieldRepository : AdminRepositoryBase<CodeGenFieldEntity>, ICodeGenFieldRepository
{
    public CodeGenFieldRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {

    }
}