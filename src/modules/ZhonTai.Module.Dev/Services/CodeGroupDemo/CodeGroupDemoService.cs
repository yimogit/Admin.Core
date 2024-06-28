
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

using ZhonTai.Module.Dev.Domain.CodeGroupDemo;
using ZhonTai.Module.Dev.Services.CodeGroupDemo.Dto;
using ZhonTai.Module.Dev.Core.Consts;


namespace ZhonTai.Module.Dev.Services.CodeGroupDemo
{
    /// <summary>
    /// 模板演示服务
    /// </summary>
    [DynamicApi(Area = DevConsts.AreaName)]
    public class CodeGroupDemoService : BaseService, ICodeGroupDemoService, IDynamicApi
    {
        private ICodeGroupDemoRepository _codeGroupDemoRepository => LazyGetRequiredService<ICodeGroupDemoRepository>();

        public CodeGroupDemoService()
        {
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CodeGroupDemoGetOutput> GetAsync(long id)
        {
            var output = await _codeGroupDemoRepository.GetAsync<CodeGroupDemoGetOutput>(id);
            return output;
        }
        
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<CodeGroupDemoGetListOutput>> GetListAsync(CodeGroupDemoGetListInput input)
        {
            var list = await _codeGroupDemoRepository.Select
                .WhereIf(!string.IsNullOrEmpty(input.InputText), a=>a.InputText == input.InputText)
                .WhereIf(input.InputBussinessSingle != null, a=>a.InputBussinessSingle == input.InputBussinessSingle)
                .OrderByDescending(a => a.Id)
                .ToListAsync<CodeGroupDemoGetListOutput>();
            var dictRepo = LazyGetRequiredService<IDictRepository>();
            var dictList = await dictRepo.Where(w => new string[] { "sex" }
                .Contains(w.DictType.Code)).ToListAsync();
            return list.Select(s =>
            {
                s.InputSelectDictDictName = dictList.FirstOrDefault(f => f.DictType.Code == "sex" && f.Code == s.InputSelectDict)?.Name;
               return s;
            });
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageOutput<CodeGroupDemoGetPageOutput>> GetPageAsync(PageInput<CodeGroupDemoGetPageInput> input)
        {
            var filter = input.Filter;
            var list = await _codeGroupDemoRepository.Select
                .WhereDynamicFilter(input.DynamicFilter)
                .WhereIf(filter !=null && !string.IsNullOrEmpty(filter.InputText), a=> a.InputText != null && a.InputText.Contains(filter.InputText))
                .WhereIf(filter !=null && filter.InputBussinessSingle != null, a=>a.InputBussinessSingle == filter.InputBussinessSingle)
                .Count(out var total)
                .OrderByDescending(c => c.Id)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<CodeGroupDemoGetPageOutput>();
        
            var dictRepo = LazyGetRequiredService<IDictRepository>();
            var dictList = await dictRepo.Where(w => new string[] { "sex" }
                .Contains(w.DictType.Code)).ToListAsync();
            
            list = list.Select(s =>
            {
                s.InputSelectDictDictName = dictList.FirstOrDefault(f => f.DictType.Code == "sex" && f.Code == s.InputSelectDict)?.Name;
            
               return s;
            }).ToList();

            //关联查询代码
            //数据转换-单个关联
            var inputBussinessSingleRows = list.Where(s => s.InputBussinessSingle > 0).ToList();
            if (inputBussinessSingleRows.Any())
            {
                var inputBussinessSingleRepo = LazyGetRequiredService<Domain.CodeGroup.ICodeGroupRepository>();
                var inputBussinessSingleRowsIds = inputBussinessSingleRows.Select(s => s.InputBussinessSingle).Distinct().ToList();
                var inputBussinessSingleRowsIdsData = await inputBussinessSingleRepo.Where(s => inputBussinessSingleRowsIds.Contains(s.Id)).ToListAsync(s => new { s.Id, s.Title });
                inputBussinessSingleRows.ForEach(s =>
                {
                    s.InputBussinessSingle_Text = inputBussinessSingleRowsIdsData.FirstOrDefault(s2 => s2.Id == s.InputBussinessSingle)?.Title;
                });
            }
            //数据转换-多个关联
            var inputBussinessMultipleRows = list.Where(s => s.InputBussinessMultiple_Values != null && s.InputBussinessMultiple_Values.Any()).ToList();
            if (inputBussinessMultipleRows.Any())
            {
                var inputBussinessMultipleRepo = LazyGetRequiredService<Domain.CodeGroup.ICodeGroupRepository>();
                var inputBussinessMultipleRowsIds =inputBussinessMultipleRows.SelectMany(s => s.InputBussinessMultiple_Values).Select(s => long.TryParse(s, out long s2) ? s2 : 0).Distinct().ToList();
                var inputBussinessMultipleRowsIdsData = await inputBussinessMultipleRepo.Where(s => inputBussinessMultipleRowsIds.Contains(s.Id)).ToListAsync(s => new { s.Id, s.Title });
                inputBussinessMultipleRows.ForEach(s =>
                {
                    s.InputBussinessMultiple_Texts = inputBussinessMultipleRowsIdsData.Where(s2 => s.InputBussinessMultiple_Values.Contains(s2.Id.ToString())).OrderBy(s2 => s.InputBussinessMultiple_Values.IndexOf(s2.Id.ToString())).Select(s2 => s2.Title).ToList();
                });
            }

            var data = new PageOutput<CodeGroupDemoGetPageOutput> { List = list, Total = total };
        
            return data;
        }
        

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> AddAsync(CodeGroupDemoAddInput input)
        {
            var entity = Mapper.Map<CodeGroupDemoEntity>(input);
            var id = (await _codeGroupDemoRepository.InsertAsync(entity)).Id;

            return id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateAsync(CodeGroupDemoUpdateInput input)
        {
            var entity = await _codeGroupDemoRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                throw ResultOutput.Exception("模板演示不存在！");
            }

            Mapper.Map(input, entity);
            await _codeGroupDemoRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(long id)
        {
            return await _codeGroupDemoRepository.DeleteAsync(id) > 0;
        }



        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> SoftDeleteAsync(long id)
        {
            return await _codeGroupDemoRepository.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> BatchSoftDeleteAsync(long[] ids)
        {
            return await _codeGroupDemoRepository.SoftDeleteAsync(ids);
        }
    }
}