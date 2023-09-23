using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.Utils;

namespace YiQiKan.DotNet.Service.Config {
    /// <summary>
    /// 说明：此类用来表述域名相关的配置
    /// Domain为主域名，后续登录、注册、视频列表、详情、历史等都会根据这个Domain来拼接
    /// Domain不是固定的，一直被封、一直更换，需要用另外的地址去获取这个动态的Domain
    /// 
    /// Fiddler抓包：启动程序-抓到入口域名-分别对候选域名进行请求-确定最终速度最快的
    /// </summary>
    internal class DomainConfig {
        /// <summary>
        /// 入口域名，通过以下固定的三个域名可以动态获取到最新的可用主域名
        /// </summary>
        internal static string[] OriginalDomains { get; } = new string[]{
            "https://api.yqkapp.com/api/v1.0/link/getCheckDomainList",//可用于Web跨域
            "https://tv-config100.oss-accelerate.aliyuncs.com/api/v1.0/config/get",
            "https://tv-config100.oss-cn-hongkong.aliyuncs.com/api/v1.0/config/get",
            };

        //域名备选列表信息
        private List<DomainInfo> domainInfos = new List<DomainInfo>();
        //主域名
        public string? Domain { get; set; }

        public string? AppVersion { get; set; }
        public string? AppName { get; set; }

        /// <summary>
        /// 初始化主域名
        /// </summary>
        /// <returns>初始化成功与否</returns>
        /// 【由于一起看的版权政策原因，接口域名更新频繁，此方法是从原始域名中获取到最新的动态域名，然后选择速度最快的作为当前APP的主域名】
        public async Task<bool> Init() {
            bool reult = false;
            foreach (string item in OriginalDomains) {
                var apiUrlRespons = await HttpHelper.GetFromJsonAsync<Response<ApiUrlData>>(item);
                if (apiUrlRespons?.Data?.ApiUrlList?.Count > 0) {
                    await FilterCandidateDomain(apiUrlRespons?.Data?.ApiUrlList);
                    if (!string.IsNullOrEmpty(Domain)) {
                        reult = true;
                    }
                    break;//有一个原始域名获取到就可以了
                }
            }

            return reult;
        }
        /// <summary>
        /// 筛选候选域名
        /// </summary>
        /// <param name="apiUrls">域名列表</param>
        /// <returns>主域名是否成功更新</returns>
        private async Task FilterCandidateDomain(List<ApiUrlData.ApiUrl>? apiUrls) {
            domainInfos.Clear();
            List<Task> pingTasks = new List<Task>();
            foreach (var apiUrl in apiUrls) {
                pingTasks.Add(PingDomainTask(apiUrl.Url));
            }
            await WhenAnySuccess(pingTasks);
        }

        public async Task<Task> WhenAnySuccess(IEnumerable<Task> tasks) {
            var taskList = tasks.ToList();
            while (taskList.Count > 0) {
                var completedTask = await Task.WhenAny(taskList);
                if (completedTask.Status == TaskStatus.RanToCompletion) {
                    return completedTask;
                }
                taskList.Remove(completedTask);
            }
            return null;
        }
        /// <summary>
        /// 测试域名连通速度和是否可用
        /// </summary>
        /// <param name="url">传入的地址</param>
        /// <returns>任务</returns>
        private async Task PingDomainTask(string url) {
            DomainInfo domainInfo = new DomainInfo {
                Domain = url,
                Time = TimeSpan.MaxValue,
                IsEnable = false
            };
            Stopwatch stopwatch = Stopwatch.StartNew();
            var response = await HttpHelper.GetFromJsonAsync<Response<string>>(url + "api/v1.0/ping/get");
            stopwatch.Stop();
            domainInfo.Time = stopwatch.Elapsed;
            domainInfo.IsEnable = !string.IsNullOrEmpty(response?.Data);
            Debug.WriteLine(domainInfo?.ToString());
            if (!domainInfo.IsEnable.Value) {
                throw new Exception("domain is not enabled!");
            }
            domainInfos.Add(domainInfo);
            var fastestDomain = domainInfos.Where(d => d.IsEnable.Value).OrderBy(d => d.Time).FirstOrDefault();
            Domain = fastestDomain?.Domain;
            Debug.WriteLine("当前主域名：" + Domain);
        }
    }

    /// <summary>
    /// 候选域名信息
    /// </summary>
    internal class DomainInfo {
        //域名
        public string? Domain { get; set; }
        //延迟时间
        public TimeSpan? Time { get; set; }
        //能否使用
        public bool? IsEnable { get; set; }

        public override string ToString() {
            return $"域名：{Domain}，延迟：{Time}，是否可用：{IsEnable}";
        }
    }
}
