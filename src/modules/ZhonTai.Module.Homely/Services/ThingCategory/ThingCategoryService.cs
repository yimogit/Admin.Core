
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

using ZhonTai.Module.Homely.Domain.ThingCategory;
using ZhonTai.Module.Homely.Services.ThingCategory.Dto;
using ZhonTai.Module.Homely.Core.Consts;


namespace ZhonTai.Module.Homely.Services.ThingCategory
{
    /// <summary>
    /// 物品分类服务
    /// </summary>
    [DynamicApi(Area = HomelyConsts.AreaName)]
    public class ThingCategoryService : BaseService, IThingCategoryService, IDynamicApi
    {
        private IThingCategoryRepository _thingCategoryRepository => LazyGetRequiredService<IThingCategoryRepository>();

        public ThingCategoryService()
        {
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> AddAsync(ThingCategoryAddInput input)
        {
            var entity = Mapper.Map<ThingCategoryEntity>(input);
            var id = (await _thingCategoryRepository.InsertAsync(entity)).Id;

            return id;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ThingCategoryGetOutput> GetAsync(long id)
        {
            var output = await _thingCategoryRepository.GetAsync<ThingCategoryGetOutput>(id);
            return output;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageOutput<ThingCategoryGetPageOutput>> GetPageAsync(PageInput<ThingCategoryGetPageInput> input)
        {
            var filter = input.Filter;
            var list = await _thingCategoryRepository.Select
                .WhereDynamicFilter(input.DynamicFilter)
                .WhereIf(filter !=null && !string.IsNullOrEmpty(filter.Name), a=> a.Name != null && a.Name.Contains(filter.Name))
                .Count(out var total)
                .OrderByDescending(c => c.Id)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<ThingCategoryGetPageOutput>();
        

            var data = new PageOutput<ThingCategoryGetPageOutput> { List = list, Total = total };
        
            return data;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateAsync(ThingCategoryUpdateInput input)
        {
            var entity = await _thingCategoryRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                throw ResultOutput.Exception("物品分类不存在！");
            }

            Mapper.Map(input, entity);
            await _thingCategoryRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(long id)
        {
            return await _thingCategoryRepository.DeleteAsync(id) > 0;
        }

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ThingCategoryGetListOutput>> GetListAsync(ThingCategoryGetListInput input)
        {
            var list = await _thingCategoryRepository.Select
                .WhereIf(!string.IsNullOrEmpty(input.Name), a=>a.Name == input.Name)
                .OrderByDescending(a => a.Id)
                .ToListAsync<ThingCategoryGetListOutput>();
            return list;
        }


        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> SoftDeleteAsync(long id)
        {
            return await _thingCategoryRepository.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> BatchSoftDeleteAsync(long[] ids)
        {
            return await _thingCategoryRepository.SoftDeleteAsync(ids);
        }
    }
}