using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Model.RequestModel;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.Service.Config;
using YiQiKan.DotNet.Service.Exceptions;
using YiQiKan.DotNet.Utils;

namespace YiQiKan.DotNet.Service.Module {
    /// <summary>
    /// 说明：此类用来做账户的所有服务器请求
    /// 方法包括：登录、退出登录、获取用户会员信息
    /// 
    /// Q1：这个类中，为什么已经有一个账号密码登录了，还有一个会员获取？
    /// A1：一起看App中，将登录和会员分成了两个接口，这样登录只管账号密码的验证已经一些关键信息的获取
    /// 而会员获取，就是根据登录的信息去拿会员相关信息，比如每日观看，会员等级等等
    /// 
    /// Q2：为什么会员获取不要账号密码，而是只要一个token？
    /// A2：token携带了用户信息，之后的所有关于用户相关的请求操作都会用到token
    /// 
    /// Q3：token是什么？
    /// A3：当用户第一次使用软件的时候，需要用账号和密码进行登录，
    /// 服务器查询账号和密码是否匹配，如果匹配的话，服务器会给一个令牌给你，并且告诉客户端
    /// “接下来15天内，你不用再输入账号密码了，太麻烦了。直接用这个令牌，以后我就知道是你了”
    /// 这里的令牌就是token，就像古代看见令牌就是看见皇上了，见牌如见人
    /// 拿着令牌在服务器接口上横着走O(∩_∩)O
    /// 
    /// 
    /// </summary>
    public class AccountService {
        internal AccountService(DomainConfig domainConfig) {
            var domain = domainConfig.Domain;
            //主域名从外部传入，在这里进行相关接口的拼接
            memberUserLogin = string.Concat(domain, "api/v1.2/member/userLogin");
            memberLogout = string.Concat(domain, "api/v1.0/member/logout");
            userGet = string.Concat(domain, "api/v1.0/user/get");
            savePhoneLoginLog = string.Concat(domain, "api/v1.0/userInfoLog/savePhoneLoginLog");
            changePwd = string.Concat(domain, "api/v1.0/security/changePwd");
            changeNickName = string.Concat(domain, "api/v1.0/userSetting/updateName?nickName={0}");
            getCodeImg = string.Concat(domain, "api/v1.0/phone/getCodeImg?areaCode={0}&phone={1}&type={2}");
            send = string.Concat(domain, "api/v1.0/phone/send");
            vailSms = string.Concat(domain, "api/v1.0/phone/vailSms");
            this.domainConfig = domainConfig;
        }


        //登录
        private string? memberUserLogin;
        //登出
        private string? memberLogout;
        //获取会员信息
        private string? userGet;
        //保存登录信息
        private string? savePhoneLoginLog;
        //修改密码
        private string? changePwd;
        //修改昵称
        private string? changeNickName;
        //获取图片验证码
        private string? getCodeImg;
        //获取短信验证码
        private string? send;
        //验证短信登陆结果
        private string? vailSms;
        private readonly DomainConfig domainConfig;

        /// <summary>
        /// 获取图像验证码，用来在短信验证码发送前的一道验证
        /// </summary>
        /// <param name="areaCode">手机区号</param>
        /// <param name="phone">手机号</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public async Task<Stream> GetCodeImageStream(string areaCode, string phone, string type = "AutoLogin") {
            return await HttpHelper.DownloadAsync(string.Format(getCodeImg, areaCode, phone, type));
        }

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="deviceId">设备唯一表示码</param>
        /// <param name="areaCode">区域码</param>
        /// <param name="areaName">区域名</param>
        /// <param name="code">图像验证码</param>
        /// <param name="isFromSetting">是否从设置登录</param>
        /// <param name="phone">手机号</param>
        /// <param name="type">登录类型</param>
        /// <returns>验证码是否发送成功</returns>
        /// <exception cref="YiQiKanRequestException"></exception>
        public async Task<bool> SendPhoneCode(string deviceId, string areaCode, string areaName, string code, bool isFromSetting, string phone, string type = "AutoLogin") {
            var sendPhoneCodeRequest = new SendPhoneCodeRequest {
                areaCode = areaCode,
                areaCodeName = areaName,
                code = code,
                isFromSetting = isFromSetting,
                phone = phone,
                type = type
            };
            var result = await HttpHelper.PostAsJsonAsync<Response<object>, SendPhoneCodeRequest>(send, sendPhoneCodeRequest, headers: new Dictionary<string, string> {
                {"onlyIdentificationm",deviceId }
            });
            if (result?.ResultCode == 200) {
                return true;
            }
            throw new YiQiKanRequestException(result?.ResultMsg);
        }

        /// <summary>
        /// 手机号+短信验证码登录
        /// </summary>
        /// <param name="deviceId">设备唯一表示码</param>
        /// <param name="areaCode">区域码</param>
        /// <param name="areaName">区域名</param>
        /// <param name="code">图像验证码</param>
        /// <param name="inviteCode">邀请码</param>
        /// <param name="isFromSetting">是否从设置登录</param>
        /// <param name="model">手机型号</param>
        /// <param name="osVersion">软件版本</param>
        /// <param name="phone">手机号</param>
        /// <param name="type">登录类型</param>
        /// <returns></returns>
        /// <exception cref="YiQiKanRequestException">短信验证码发送失败的异常</exception>
        public async Task<LoginData> LoginByPhoneAndCode(string deviceId,string areaCode, string areaName, string code, string inviteCode, bool isFromSetting, string model, string osVersion, string phone, string type = "AutoLogin") {
            var request = new PhoneCodeLoginRequest {
                areaCode = areaCode,
                areaCodeName = areaName,
                code = code,
                inviteCode = inviteCode,
                isFromSetting = isFromSetting,
                model = model,
                osVersion = osVersion,
                phone = phone,
                type = type
            };
            var result = await HttpHelper.PostAsJsonAsync<Response<LoginData>, PhoneCodeLoginRequest>(vailSms, request, headers: new Dictionary<string, string> {
                {"onlyIdentificationm",deviceId }
            });
            if (result?.Data == null) {
                throw new YiQiKanRequestException(result.ResultMsg);
            }
            return result.Data;
        }

        /// <summary>
        /// 账号密码登录方法
        /// </summary>
        /// <param name="account">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录结果</returns>
        /// <exception cref="YiQiKanRequestException">服务器如果验证失败，会在这里抛出异常</exception>
        public async Task<LoginData> LoginByAccountAndPassword(string account, string password) {
            //这里需要对密码进行MD5值加密，保证安全性
            var loginRequest = new AccountPasswordLoginRequest(account, EncryptHelper.EncryptByMD5(password)) {
                pushPackageName = domainConfig.AppName,
                osVersion = domainConfig.AppVersion
            };
            var result = await HttpHelper.PostAsJsonAsync<Response<LoginData>, AccountPasswordLoginRequest>(memberUserLogin, loginRequest);
            if (result?.Data == null) {
                throw new YiQiKanRequestException(result.ResultMsg);
            }
            return result.Data;
        }

        /// <summary>
        /// 退出登录方法
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns>退出是否成功</returns>
        public async Task<bool> Logout(string token) {
            var result = await HttpHelper.PostAsJsonAsync<Response<object>, LogoutRequest>(memberLogout, new LogoutRequest(), token);
            if (result?.ResultCode == 200) {
                return true;
            }
            throw new YiQiKanRequestException(result?.ResultMsg);
        }

        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns>会员信息数据</returns>
        /// <exception cref="YiQiKanRequestException">服务器如果验证失败，会在这里抛出异常</exception>
        public async Task<UserInfoData> GetUserInfo(string token) {
            var result = await HttpHelper.GetFromJsonAsync<Response<UserInfoData>>(userGet, token);
            if (result?.Data == null) {
                throw new YiQiKanRequestException(result?.ResultMsg);
            }
            return result?.Data;
        }

        /// <summary>
        /// 保存手机登录日志记录
        /// </summary>
        /// <param name="agentCode">推荐码</param>
        /// <param name="onlyIdentification">设备唯一id</param>
        /// <returns>保存成功与否</returns>
        public async Task<bool> SavePhoneLoginLog(string agentCode, string onlyIdentification, string accessToken) {
            var timeStamp = DateTimeHelper.GetTimeStamp();
            var srcContact = $"agentCode={agentCode}&onlyIdentificationm={onlyIdentification}&ts={timeStamp}";
            var sign = EncryptHelper.AesEecrypt(srcContact, Encoding.UTF8.GetBytes(SecretConfig.CommonKey), Encoding.UTF8.GetBytes(SecretConfig.CommonIV)).GetSha256Data().BytesToString();
            var request = new SavePhoneLoginLogRequest {
                AgentCode = agentCode,
                OnlyIdentificationm = onlyIdentification,
                Ts = timeStamp,
                Sign = sign
            };
            var result = await HttpHelper.PostAsJsonAsync<Response<object>, SavePhoneLoginLogRequest>(savePhoneLoginLog, request, accessToken);
            if (result?.ResultCode == 200) {
                return true;
            }
            throw new YiQiKanRequestException(result?.ResultMsg);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldPassword">原密码</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="token">token</param>
        /// <returns>成功与否</returns>
        /// <exception cref="YiQiKanRequestException">修改失败的异常信息</exception>
        public async Task<bool> ChangePassword(string oldPassword, string newPassword, string token) {
            var request = new ChangePasswordRequest {
                OldPassword = EncryptHelper.EncryptByMD5(oldPassword),
                NewPassword = EncryptHelper.EncryptByMD5(newPassword)
            };
            var result = await HttpHelper.PostAsJsonAsync<Response<object>, ChangePasswordRequest>(changePwd, request, token);
            if (result?.ResultCode == 200) {
                return true;
            }
            throw new YiQiKanRequestException(result?.ResultMsg);
        }

        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="newNickName">新的昵称</param>
        /// <param name="token">token</param>
        /// <returns>修改成功与否</returns>
        /// <exception cref="YiQiKanRequestException">修改失败的异常信息</exception>
        public async Task<bool> ChangeNickName(string newNickName, string token) {
            string url = string.Format(changeNickName, newNickName);
            var result = await HttpHelper.GetFromJsonAsync<Response<object>>(url, token);
            if (result?.ResultCode == 200) {
                return true;
            }
            throw new YiQiKanRequestException(result?.ResultMsg);
        }
    }
}
