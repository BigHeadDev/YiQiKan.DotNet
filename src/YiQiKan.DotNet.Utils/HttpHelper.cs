using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YiQiKan.DotNet.Utils {
    /// <summary>
    /// 说明：此类为网络请求类，包括常用的Get、Post、附带headers、附带body等方法
    /// 
    /// 注意：请求中用了System.Net.Http.Json库，可以直接从请求结果反序列化为C#对象
    /// </summary>
    public static class HttpHelper {
        //全局唯一httpClient
        private static HttpClient httpClient;
        static HttpHelper() {
            httpClient = new HttpClient();
            //这里加入默认的请求头
            //！！！模拟安卓app的请求！！！！免得被发现了封禁
            //①Client-Type Android  这里是在请求头加入客户端类型，服务器认为你是安卓
            //②User-Agent  okhttp/3.14.9  这里是在请求头加入请求的java http包，服务器认为你是通过java请求的
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh");
            httpClient.DefaultRequestHeaders.Add("Client-Type", "Windows");//为了不与其他端互相挤下线，还是换成Windows，可能会被封禁
            httpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            httpClient.DefaultRequestHeaders.Add("User-Agent", Environment.OSVersion.ToString());
        }
        /// <summary>
        /// 使用Get直接请求获取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="token">用户token</param>
        /// <returns>响应结果经过反序列化得到的对象</returns>
        public static async Task<T> GetFromJsonAsync<T>(string url, string token = "", Dictionary<string, string> headers = null) {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (!string.IsNullOrEmpty(token)) {
                request.Headers.Add("accessToken", token);
            }
            if (headers != null) {
                foreach (var header in headers) {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            try {
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode) {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
            } catch (Exception) {

            }
            return default;
        }

        /// <summary>
        /// 使用Post直接得到对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="content">传入的对象</param>
        /// <param name="token">用户token</param>
        /// <returns>http响应结果</returns>
        public static async Task<TOutPut> PostAsJsonAsync<TOutPut, TInput>(string url, TInput content, string token = "", Dictionary<string, string> headers = null) {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(JsonSerializer.Serialize(content), System.Text.Encoding.UTF8, "application/json");
            if (!string.IsNullOrEmpty(token)) {
                request.Headers.Add("accessToken", token);
            }
            if (headers != null) {
                foreach (var header in headers) {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadFromJsonAsync<TOutPut>();
            }
            return default;
        }

        /// <summary>
        /// 试用Get来下载文件流
        /// </summary>
        /// <param name="url">文件下载地址</param>
        /// <returns>流数据</returns>
        public static async Task<Stream> DownloadAsync(string url) {
            var stream = await httpClient.GetStreamAsync(url);
            MemoryStream memoryStream = new MemoryStream();
            using (StreamReader reader = new StreamReader(stream)) {
                reader.BaseStream.CopyTo(memoryStream);
            }
            memoryStream.Position = 0;
            return memoryStream;
        }

    }
}