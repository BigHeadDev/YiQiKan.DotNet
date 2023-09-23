using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.Service.Config;
using YiQiKan.DotNet.Service.Exceptions;
using YiQiKan.DotNet.Utils;

namespace YiQiKan.DotNet.Service.Module {
    public class SearchService {
        internal SearchService(DomainConfig? domainConfig) {
            var domain = domainConfig?.Domain;
            hotSearchGet = string.Concat(domain, "api/v1.0/hotSearch/get");
            getMovieByOptions = string.Concat(domain, "api/v1.0/search/getMovieByOptions?q={0}&category={1}&type={2}&area={3}&year={4}&sort={5}&moviePlayType={6}&pageIndex={7}&pageSize={8}");
        }
        //获取热搜
        private string? hotSearchGet;
        //筛选视频
        private string? getMovieByOptions;

        public async Task<HotSearchItem[]> GetHotSearchItems(string token) {
            var result = await HttpHelper.GetFromJsonAsync<Response<HotSearchItem[]>>(hotSearchGet, token);
            if (result?.Data == null) {
                throw new YiQiKanRequestException(result?.ResultMsg);
            }
            return result?.Data;
        }

        public async Task<LoadingResultData<SearchItem>> GetMovieByOptions(string query, string category, string type, string area, string year, string sort, string moviePlayType, int pageIndex, int pageSize, string token) {
            var url = string.Format(getMovieByOptions,query,category,type,area,year,sort,moviePlayType,pageIndex,pageSize);
            var result = await HttpHelper.GetFromJsonAsync<Response<LoadingResultData<SearchItem>>>(url, token);
            if (result?.Data == null) {
                throw new YiQiKanRequestException(result?.ResultMsg);
            }
            return result?.Data;
        }
    }
}
