using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.Service.Config;
using YiQiKan.DotNet.Service.Module;
using YiQiKan.DotNet.Utils;

namespace YiQiKan.DotNet.Service {
    /// <summary>
    /// 说明：此类为一起看的主服务类，用来掌管域名、账户、电视、播放等所有的一起看接口的C#实现
    /// 外部调用的时候，通过点出不同的服务进行对应的操作
    /// 
    /// >>一起看服务
    ///       >>账户服务
    ///       >>分类服务
    ///       >>搜索服务
    ///       >>账户服务
    ///       >>....more....
    /// 
    /// Q：这些服务的逻辑从哪来？为什么走这样的逻辑？
    /// A：通过抓包安卓app，通过手机上的操作，从抓包工具看到动作对应的请求，然后进行分析和猜测的来
    /// 
    /// </summary>
    public class YiQiKanService {
        /// <summary>
        /// 账户服务
        /// </summary>
        public AccountService? Account { get; private set; }

        /// <summary>
        /// 视频服务
        /// </summary>
        public VideoService? Video { get; private set; }

        /// <summary>
        /// 搜索服务
        /// </summary>
        public SearchService? Search { get; private set; }


        //域名配置对象
        private DomainConfig domainConfig = new DomainConfig();

        /// <summary>
        /// 初始化方法，主要是初始化域名
        /// 如果主域名成功就初始化其他的服务（将域名给到其他子服务）
        /// </summary>
        /// <returns>初始化成功与否</returns>
        public async Task<bool> Init(Assembly assembly = null) {
            var result = await domainConfig.Init();
            Debug.Write("主域名已经获取");
            if (result == true) {
                var app = assembly.GetName();
                domainConfig.AppVersion = app.Version.ToString();
                domainConfig.AppName = app.Name;
                Account = new AccountService(domainConfig);
                Video = new VideoService(domainConfig);
                Search = new SearchService(domainConfig);
            }
            return result;
        }
        /// <summary>
        /// 用于测试，不建议用于实际开发
        /// </summary>
        /// <param name="url">主域名</param>
        public void Init(string url) {
            domainConfig.Domain = url;
            Account = new AccountService(domainConfig);
            Video = new VideoService(domainConfig);
        }
    }
}
