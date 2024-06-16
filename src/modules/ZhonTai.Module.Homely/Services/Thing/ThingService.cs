
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

using ZhonTai.Module.Homely.Domain.Thing;
using ZhonTai.Module.Homely.Services.Thing.Dto;


namespace ZhonTai.Module.Homely.Services.Thing
{
    /// <summary>
    /// 物品服务
    /// </summary>
    [DynamicApi(Area = "homely")]
    public class ThingService : BaseService, IThingService, IDynamicApi
    {
        private IThingRepository _thingRepository => LazyGetRequiredService<IThingRepository>();

        public ThingService()
        {
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> AddAsync(ThingAddInput input)
        {
            var entity = Mapper.Map<ThingEntity>(input);
            var id = (await _thingRepository.InsertAsync(entity)).Id;

            return id;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ThingGetOutput> GetAsync(long id)
        {
            var output = await _thingRepository.GetAsync<ThingGetOutput>(id);
            return output;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageOutput<ThingGetPageOutput>> GetPageAsync(PageInput<ThingGetPageInput> input)
        {
            var filter = input.Filter;
            var list = await _thingRepository.Select
                .WhereDynamicFilter(input.DynamicFilter)
                .WhereIf(filter !=null && !string.IsNullOrEmpty(filter.Name), a=> a.Name != null && a.Name.Contains(filter.Name))
                .WhereIf(filter !=null && !string.IsNullOrEmpty(filter.Remark), a=> a.Remark != null && a.Remark.Contains(filter.Remark))
                .Count(out var total)
                .OrderByDescending(c => c.Id)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<ThingGetPageOutput>();
        

            var data = new PageOutput<ThingGetPageOutput> { List = list, Total = total };
        
            return data;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateAsync(ThingUpdateInput input)
        {
            var entity = await _thingRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                throw ResultOutput.Exception("物品不存在！");
            }

            Mapper.Map(input, entity);
            await _thingRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(long id)
        {
            return await _thingRepository.DeleteAsync(id) > 0;
        }

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ThingGetListOutput>> GetListAsync(ThingGetListInput input)
        {
            var list = await _thingRepository.Select
                .WhereIf(!string.IsNullOrEmpty(input.Name), a=>a.Name == input.Name)
                .WhereIf(!string.IsNullOrEmpty(input.Remark), a=>a.Remark == input.Remark)
                .OrderByDescending(a => a.Id)
                .ToListAsync<ThingGetListOutput>();
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
            return await _thingRepository.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> BatchSoftDeleteAsync(long[] ids)
        {
            return await _thingRepository.SoftDeleteAsync(ids);
        }
    }
}