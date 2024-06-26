﻿using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Module.Dev.Domain.CodeGen;

namespace ZhonTai.Admin.Repositories;

public class CodeGenRepository : AdminRepositoryBase<CodeGenEntity>, ICodeGenRepository
{
    public CodeGenRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {

    }
}