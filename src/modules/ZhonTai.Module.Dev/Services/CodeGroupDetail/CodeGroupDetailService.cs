
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

using ZhonTai.Module.Dev.Domain.CodeGroupDetail;
using ZhonTai.Module.Dev.Services.CodeGroupDetail.Dto;
using ZhonTai.Module.Dev.Core.Consts;


namespace ZhonTai.Module.Dev.Services.CodeGroupDetail
{
    /// <summary>
    /// 模板明细服务
    /// </summary>
    [DynamicApi(Area = DevConsts.AreaName)]
    public class CodeGroupDetailService : BaseService, ICodeGroupDetailService, IDynamicApi
    {
        private ICodeGroupDetailRepository _codeGroupDetailRepository => LazyGetRequiredService<ICodeGroupDetailRepository>();

        public CodeGroupDetailService()
        {
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CodeGroupDetailGetOutput> GetAsync(long id)
        {
            var output = await _codeGroupDetailRepository.GetAsync<CodeGroupDetailGetOutput>(id);
            return output;
        }
        
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<CodeGroupDetailGetListOutput>> GetListAsync(CodeGroupDetailGetListInput input)
        {
            var list = await _codeGroupDetailRepository.Select
                .WhereIf(!string.IsNullOrEmpty(input.Name), a=>a.Name == input.Name)
                .WhereIf(input.GroupId != null, a=>a.GroupId == input.GroupId)
                .OrderByDescending(a => a.Id)
                .ToListAsync<CodeGroupDetailGetListOutput>();
            return list;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageOutput<CodeGroupDetailGetPageOutput>> GetPageAsync(PageInput<CodeGroupDetailGetPageInput> input)
        {
            var filter = input.Filter;
            var list = await _codeGroupDetailRepository.Select
                .WhereDynamicFilter(input.DynamicFilter)
                .WhereIf(filter !=null && !string.IsNullOrEmpty(filter.Name), a=> a.Name != null && a.Name.Contains(filter.Name))
                .WhereIf(filter !=null && filter.GroupId != null, a=>a.GroupId == filter.GroupId)
                .Count(out var total)
                .OrderByDescending(c => c.Id)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<CodeGroupDetailGetPageOutput>();
        

            //关联查询代码
            //数据转换-单个关联
            var groupIdRows = list.Where(s => s.GroupId > 0).ToList();
            if (groupIdRows.Any())
            {
                var groupIdRepo = LazyGetRequiredService<Domain.CodeGroup.ICodeGroupRepository>();
                var groupIdRowsIds = groupIdRows.Select(s => s.GroupId).Distinct().ToList();
                var groupIdRowsIdsData = await groupIdRepo.Where(s => groupIdRowsIds.Contains(s.Id)).ToListAsync(s => new { s.Id, s.Name });
                groupIdRows.ForEach(s =>
                {
                    s.GroupId_Text = groupIdRowsIdsData.FirstOrDefault(s2 => s2.Id == s.GroupId)?.Name;
                });
            }
            //数据转换-多个关联
            var groupIdsRows = list.Where(s => s.GroupIds_Values != null && s.GroupIds_Values.Any()).ToList();
            if (groupIdsRows.Any())
            {
                var groupIdsRepo = LazyGetRequiredService<Domain.CodeGroup.ICodeGroupRepository>();
                var groupIdsRowsIds =groupIdsRows.SelectMany(s => s.GroupIds_Values).Select(s => long.TryParse(s, out long s2) ? s2 : 0).Distinct().ToList();
                var groupIdsRowsIdsData = await groupIdsRepo.Where(s => groupIdsRowsIds.Contains(s.Id)).ToListAsync(s => new { s.Id, s.Name });
                groupIdsRows.ForEach(s =>
                {
                    s.GroupIds_Texts = groupIdsRowsIdsData.Where(s2 => s.GroupIds_Values.Contains(s2.Id.ToString())).OrderBy(s2 => s.GroupIds_Values.IndexOf(s2.Id.ToString())).Select(s2 => s2.Name).ToList();
                });
            }

            var data = new PageOutput<CodeGroupDetailGetPageOutput> { List = list, Total = total };
        
            return data;
        }
        

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> AddAsync(CodeGroupDetailAddInput input)
        {
            var entity = Mapper.Map<CodeGroupDetailEntity>(input);
            var id = (await _codeGroupDetailRepository.InsertAsync(entity)).Id;

            return id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateAsync(CodeGroupDetailUpdateInput input)
        {
            var entity = await _codeGroupDetailRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                throw ResultOutput.Exception("模板明细不存在！");
            }

            Mapper.Map(input, entity);
            await _codeGroupDetailRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(long id)
        {
            return await _codeGroupDetailRepository.DeleteAsync(id) > 0;
        }



        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> SoftDeleteAsync(long id)
        {
            return await _codeGroupDetailRepository.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> BatchSoftDeleteAsync(long[] ids)
        {
            return await _codeGroupDetailRepository.SoftDeleteAsync(ids);
        }
    }
}