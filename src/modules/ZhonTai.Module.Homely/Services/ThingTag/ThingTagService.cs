
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

using ZhonTai.Module.Homely.Domain.ThingTag;
using ZhonTai.Module.Homely.Services.ThingTag.Dto;
using ZhonTai.Module.Homely.Core.Consts;


namespace ZhonTai.Module.Homely.Services.ThingTag
{
    /// <summary>
    /// 标签服务
    /// </summary>
    [DynamicApi(Area = HomelyConsts.AreaName)]
    public class ThingTagService : BaseService, IThingTagService, IDynamicApi
    {
        private IThingTagRepository _thingTagRepository => LazyGetRequiredService<IThingTagRepository>();

        public ThingTagService()
        {
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> AddAsync(ThingTagAddInput input)
        {
            var entity = Mapper.Map<ThingTagEntity>(input);
            var id = (await _thingTagRepository.InsertAsync(entity)).Id;

            return id;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ThingTagGetOutput> GetAsync(long id)
        {
            var output = await _thingTagRepository.GetAsync<ThingTagGetOutput>(id);
            return output;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageOutput<ThingTagGetPageOutput>> GetPageAsync(PageInput<ThingTagGetPageInput> input)
        {
            var filter = input.Filter;
            var list = await _thingTagRepository.Select
                .WhereDynamicFilter(input.DynamicFilter)
                .Count(out var total)
                .OrderByDescending(c => c.Id)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<ThingTagGetPageOutput>();
        

            var data = new PageOutput<ThingTagGetPageOutput> { List = list, Total = total };
        
            return data;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateAsync(ThingTagUpdateInput input)
        {
            var entity = await _thingTagRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                throw ResultOutput.Exception("标签不存在！");
            }

            Mapper.Map(input, entity);
            await _thingTagRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(long id)
        {
            return await _thingTagRepository.DeleteAsync(id) > 0;
        }



        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> SoftDeleteAsync(long id)
        {
            return await _thingTagRepository.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> BatchSoftDeleteAsync(long[] ids)
        {
            return await _thingTagRepository.SoftDeleteAsync(ids);
        }
    }
}