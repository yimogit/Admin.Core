using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Module.Dev.Core.Consts;
using ZhonTai.Admin.Services;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.Admin.Domain.DictType;
using ZhonTai.Admin.Domain.Dict;

namespace ZhonTai.Admin.Services.DictionaryTree
{
    /// <summary>
    /// 字典树服务
    /// </summary>
    [DynamicApi(Area = DevConsts.AreaName)]
    public partial class DictionaryTreeService : BaseService, IDictionaryTreeService, IDynamicApi
    {
        /// <summary>
        /// 获取字典树
        /// </summary>
        /// <param name="codes">类型编号列表</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Dto.DictionaryTreeOutput>> GetAsync(string? codes)
        {
            var _mapper = LazyGetRequiredService<IMapper>();

            IEnumerable<String>? typesToGet = !String.IsNullOrWhiteSpace(codes) ? codes.Split(',') : null;

            var typeRepos = LazyGetRequiredService<IDictTypeRepository>();
            var dictRepos = LazyGetRequiredService<IDictRepository>();
            var types = await typeRepos.Select
                .WhereIf(typesToGet != null, w => typesToGet.Contains(w.Code))
                .ToListAsync< Dto.DictionaryTreeOutput>();
            var typesId = types.Select(s => s.Id);
            var dicts = await dictRepos.Select.Where(w => typesId.Contains(w.DictTypeId)).ToListAsync();

            return types.Select(s =>
            {
                s.Childrens = dicts.Where(w => w.DictTypeId == s.Id).Select(s => _mapper.Map<Dto.DictionaryTreeOutput>(s));
                return s;
            });
        }
    }
}
