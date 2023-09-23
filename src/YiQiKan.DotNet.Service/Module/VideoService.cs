using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Model.Enums;
using YiQiKan.DotNet.Model.RequestModel;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.Service.Config;
using YiQiKan.DotNet.Service.Exceptions;
using YiQiKan.DotNet.Utils;

namespace YiQiKan.DotNet.Service.Module {
    public class VideoService {
        internal VideoService(DomainConfig? domainConfig) {
            var domain = domainConfig?.Domain;
            getHomeList = string.Concat(domain, "api/v1.1/movie/getHomeList?category={0}");
            getMoreList = string.Concat(domain, "api/v1.0/movie/getMoreList?category={0}&type={1}&pageIndex={2}&pageSize={3}");
            getVideoDetail = string.Concat(domain, "api/v1.1/movie/getDetail?movieId={0}&objectId={1}");
            getGuessWhatYouLike = string.Concat(domain, "api/v1.0/movie/getGuessWhatYouLike?movieId={0}&tvId={1}&scheduleId={2}");
            getHistoryList = string.Concat(domain, "api/v1.0/playHistory/getPageList?historyType={0}&name=&pageIndex={1}&pageSize={2}");
            getCollectList = string.Concat(domain, "api/v1.0/collection/getPageList?type={0}&pageIndex={1}&pageSize={2}");
            addHistory = string.Concat(domain, "api/v1.0/playHistory/add");
            addCollect = string.Concat(domain, "api/v1.0/collection/add");
            removeCollect = string.Concat(domain, "api/v1.0/collection/remove");
        }

        //获取首页列表数据
        private string? getHomeList;
        //加载更多列表数据
        private string? getMoreList;
        //获取视频详情
        private string? getVideoDetail;
        //相关推荐
        private string? getGuessWhatYouLike;
        //获取历史记录
        private string? getHistoryList;
        //获取收藏列表
        private string? getCollectList;
        //添加历史记录
        private string? addHistory;
        //添加收藏
        private string? addCollect;
        //删除历史
        private string? removeHistory;
        //删除收藏
        private string? removeCollect;

        /// <summary>
        /// 获取首页列表数据
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="token">用户token</param>
        /// <returns>首页数据</returns>
        /// <exception cref="YiQiKanRequestException">如果请求错误，就会抛出异常</exception>
        public async Task<HomeListData> GetHomeListAsync(Category category, string token) {
            var url = string.Format(getHomeList, category);
            var result = await HttpHelper.GetFromJsonAsync<Response<HomeListData>>(url, token, new Dictionary<string, string> { { "Client-Type", "Android" } });
            if (result?.Data == null) {
                throw new YiQiKanRequestException(result?.ResultMsg);
            }
            return result?.Data;
        }

        /// <summary>
        /// 获取更多视频
        /// </summary>
        /// <param name="category">大分类</param>
        /// <param name="type">子类别</param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageCount">页面数量</param>
        /// <param name="token">用户token</param>
        /// <returns>更多视频结果</returns>
        /// <exception cref="YiQiKanRequestException">如果服务器错误，则抛出异常</exception>
        public async Task<LoadingResultData<VideoItem>> GetMoreListAsync(Category category, string type, int pageIndex, int pageCount, string token) {
            var url = string.Format(getMoreList, category, type, pageIndex, pageCount);
            var result = await HttpHelper.GetFromJsonAsync<Response<LoadingResultData<VideoItem>>>(url, token);
            if (result?.Data == null) {
                throw new YiQiKanRequestException(result?.ResultMsg);
            }
            return result?.Data;
        }

        /// <summary>
        /// 获取视频详情
        /// </summary>
        /// <param name="movieId">视频id</param>
        /// <param name="objectId">objid</param>
        /// <param name="token">用户token</param>
        /// <returns>视频详情</returns>
        /// <exception cref="YiQiKanRequestException">如果服务器错误，则抛出异常</exception>
        public async Task<VideoDetailData> GetVideoDetail(string movieId, string deviceId, string token) {
            var url = string.Format(getVideoDetail, movieId, string.IsNullOrEmpty(token) ? deviceId : token);
            var result = await HttpHelper.GetFromJsonAsync<Response<VideoDetailData>>(url, token);
            if (result?.Data == null) {
                throw new YiQiKanRequestException(result?.ResultMsg);
            }
            return result?.Data;
        }

        /// <summary>
        /// 获取视频播放地址
        /// </summary>
        /// <param name="playAddress">源加密地址</param>
        /// <param name="videoDetail">播放详情（需要通过这里面的一些数据进行解密操作）</param>
        /// <returns>解密的播放地址</returns>
        public Uri GetPlayDecodeAddress(string playAddress, VideoDetailData videoDetail) {
            var secretKeyBytes = SecretConfig.PlayAddressSecretKey.GetBase64ConvertBytes();
            var sha256Bytes = EncryptHelper.GetSha256Data((videoDetail.movieId.GetTrimString() + videoDetail.director.GetTrimString() + videoDetail.shareContent.GetTrimString() + videoDetail.name.GetTrimString()).Substring(3));
            byte[] keyBytes = new byte[16];
            Array.Copy(sha256Bytes, 8, keyBytes, 0, 4);
            Array.Copy(secretKeyBytes, 0, keyBytes, 4, 4);
            Array.Copy(sha256Bytes, 19, keyBytes, 8, 4);
            Array.Copy(secretKeyBytes, 7, keyBytes, 12, 4);
            string m3u8Url = EncryptHelper.AESDecrypt(playAddress, keyBytes, SecretConfig.CommonIV.GetUTF8Bytes()).GetUTF8String() +
            "?yqk=" +
                EncryptHelper.AesEecrypt("yqk;" + DateTimeOffset.Now.ToUnixTimeSeconds(), SecretConfig.PlaySignKey.GetUTF8Bytes(), SecretConfig.PlaySignIV.GetUTF8Bytes()).BytesToString();
            return new Uri(m3u8Url);
        }

        /// <summary>
        /// 获取推荐数据
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="tvId"></param>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        /// <exception cref="YiQiKanRequestException"></exception>
        public async Task<RecommendationData[]> GetRecommendationListAsync(string movieId, string tvId, string scheduleId, string token) {
            var url = string.Format(getGuessWhatYouLike, movieId, tvId, scheduleId);
            var result = await HttpHelper.GetFromJsonAsync<Response<RecommendationData[]>>(url, token);
            if (result?.Data == null) {
                throw new YiQiKanRequestException(result?.ResultMsg);
            }
            return result?.Data;
        }

        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="type">类型 一般是电影</param>
        /// <param name="index">当前页面下表</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="token">用户信息</param>
        /// <returns></returns>
        /// <exception cref="YiQiKanRequestException"></exception>
        public async Task<LoadingResultData<HistoryItem>> GetHistoryList(string type, int index, int pageSize, string token) {
            var url = string.Format(getHistoryList, type, index, pageSize);
            var result = await HttpHelper.GetFromJsonAsync<Response<LoadingResultData<HistoryItem>>>(url, token);
            if (result?.Data == null) {
                throw new YiQiKanRequestException(result?.ResultMsg);
            }
            return result?.Data;
        }

        /// <summary>
        /// 获取收藏列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <param name="pageSize"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="YiQiKanRequestException"></exception>
        public async Task<LoadingResultData<CollectionItem>> GetCollectList(string type, int index, int pageSize, string token) {
            var url = string.Format(getCollectList, type, index, pageSize);
            var result = await HttpHelper.GetFromJsonAsync<Response<LoadingResultData<CollectionItem>>>(url, token);
            if (result?.Data == null) {
                throw new YiQiKanRequestException(result?.ResultMsg);
            }
            return result?.Data;
        }


        /// <summary>
        /// 添加历史记录
        /// </summary>
        /// <param name="historyType">Movie</param>
        /// <param name="playItem">第01集</param>
        /// <param name="moviePlayType">General</param>
        /// <param name="deviceId">设备id</param>
        /// <param name="movieId">视频id</param>
        /// <param name="startTime">开始的北京时间</param>
        /// <param name="endTime">结束的北京时间</param>
        /// <param name="playStartTime">开始播放时刻（毫秒）</param>
        /// <param name="playEndTime">结束播放的时刻（毫秒）</param>
        /// <param name="durationTime">播放的时刻（毫秒）</param>
        /// <param name="totalDuration">总时长（毫秒）</param>
        /// <returns></returns>
        /// <exception cref="YiQiKanRequestException"></exception>
        public async Task<bool> AddHistory(string historyType, string playItem, string moviePlayType, string deviceId, string movieId, long startTime, long endTime, long playStartTime, long playEndTime, long durationTime, long totalDuration, string token) {
            var timeStamp = DateTimeHelper.GetTimeStamp();
            var history = new HistoryRequest {
                historyType = historyType,
                moviePlayType = moviePlayType,
                onlyIdentificationm = deviceId,
                targetId = movieId,
                startTime = startTime,
                endTime = endTime,
                playStartTime = playStartTime,
                playEndTime = playEndTime,
                durationTime = durationTime,
                duration = totalDuration,
                isSchedule = 0,
                playItem = playItem,
                ts = timeStamp
            };
            var srcContact = $"historyType={historyType}&targetId={movieId}&moviePlayType={moviePlayType}&playItem={playItem}&startTime={startTime}&endTime={endTime}&playStartTime={playStartTime}&playEndTime={playEndTime}&duration={totalDuration}&durationTime={durationTime}&ts={timeStamp}";
            var sign = EncryptHelper.AesEecrypt(srcContact, Encoding.UTF8.GetBytes(SecretConfig.Common2Key), Encoding.UTF8.GetBytes(SecretConfig.CommonIV)).GetSha256Data().BytesToString();
            history.sign = sign;
            var result = await HttpHelper.PostAsJsonAsync<Response<object>, HistoryRequest>(addHistory, history, token, new Dictionary<string, string> { { "Client-Type", "Android" } });
            if (result?.ResultCode == 200) {
                return true;
            }
            throw new YiQiKanRequestException(result?.ResultMsg);
        }

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="movieId">视频id</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        /// <exception cref="YiQiKanRequestException"></exception>
        public async Task<string> AddCollection(string movieId, string type, string token) {
            var collection = new CollectionRequest {
                isSchedule = 0,
                targetId = movieId,
                type = type
            };
            var result = await HttpHelper.PostAsJsonAsync<Response<string>, CollectionRequest>(addCollect, collection, token);
            if (result?.Data == null) {
                throw new YiQiKanRequestException(result?.ResultMsg);
            }
            return result?.Data;
        }

        /// <summary>
        /// 删除历史记录
        /// </summary>
        /// <param name="movieIds">视频id数组</param>
        /// <returns></returns>
        /// <exception cref="YiQiKanRequestException"></exception>
        public async Task<bool> RemoveHistory(string[] movieIds) {
            var result = await HttpHelper.PostAsJsonAsync<Response<object>, string[]>(removeHistory, movieIds);
            if (result?.ResultCode == 200) {
                return true;
            }
            throw new YiQiKanRequestException(result?.ResultMsg);
        }


        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="movieIds">视频id数组</param>
        /// <param name="type">类别</param>
        /// <returns></returns>
        /// <exception cref="YiQiKanRequestException"></exception>
        public async Task<bool> RemoveCollection(string[] movieIds, string type, string token) {
            var collection = new CollectionRemoveRequest {
                targetId = movieIds,
                type = type
            };
            var result = await HttpHelper.PostAsJsonAsync<Response<bool>, CollectionRemoveRequest>(removeCollect, collection, token);
            if (result?.Data == null) {
                throw new YiQiKanRequestException(result?.ResultMsg);
            }
            return result.Data;
        }
    }
}
