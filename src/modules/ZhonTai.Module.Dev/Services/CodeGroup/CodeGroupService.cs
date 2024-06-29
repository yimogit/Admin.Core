
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;


using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Domain.Dict;

using ZhonTai.Module.Dev.Domain.CodeGroup;
using ZhonTai.Module.Dev.Services.CodeGroup.Dto;
using ZhonTai.Module.Dev.Core.Consts;


namespace ZhonTai.Module.Dev.Services.CodeGroup
{
    /// <summary>
    /// 模板组服务
    /// </summary>
    [DynamicApi(Area = DevConsts.AreaName)]
    public class CodeGroupService : BaseService, ICodeGroupService, IDynamicApi
    {
        private ICodeGroupRepository _codeGroupRepository => LazyGetRequiredService<ICodeGroupRepository>();

        public CodeGroupService()
        {
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CodeGroupGetOutput> GetAsync(long id)
        {
            var output = await _codeGroupRepository.GetAsync<CodeGroupGetOutput>(id);
            return output;
        }
        
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<CodeGroupGetListOutput>> GetListAsync(CodeGroupGetListInput input)
        {
            var list = await _codeGroupRepository.Select
                .WhereIf(!string.IsNullOrEmpty(input.Name), a=>a.Name == input.Name)
                .OrderByDescending(a => a.Id)
                .ToListAsync<CodeGroupGetListOutput>();
            return list;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageOutput<CodeGroupGetPageOutput>> GetPageAsync(PageInput<CodeGroupGetPageInput> input)
        {
            var filter = input.Filter;
            var list = await _codeGroupRepository.Select
                .WhereDynamicFilter(input.DynamicFilter)
                .WhereIf(filter !=null && !string.IsNullOrEmpty(filter.Name), a=> a.Name != null && a.Name.Contains(filter.Name))
                .Count(out var total)
                .OrderByDescending(c => c.Id)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<CodeGroupGetPageOutput>();
        

            //关联查询代码

            var data = new PageOutput<CodeGroupGetPageOutput> { List = list, Total = total };
        
            return data;
        }
        

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> AddAsync(CodeGroupAddInput input)
        {
            var entity = Mapper.Map<CodeGroupEntity>(input);
            var id = (await _codeGroupRepository.InsertAsync(entity)).Id;

            return id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateAsync(CodeGroupUpdateInput input)
        {
            var entity = await _codeGroupRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                throw ResultOutput.Exception("模板组不存在！");
            }

            Mapper.Map(input, entity);
            await _codeGroupRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(long id)
        {
            return await _codeGroupRepository.DeleteAsync(id) > 0;
        }



        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> SoftDeleteAsync(long id)
        {
            return await _codeGroupRepository.SoftDeleteAsync(id);
        }

    }
}