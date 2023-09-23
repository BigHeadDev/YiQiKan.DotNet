using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YiQiKan.DotNet.Service.Exceptions;

namespace YiQiKan.DotNet.Test {
    [TestClass]
    public class UserTest : ServiceTestBase {
        private string token = string.Empty;
        [DataTestMethod]
        [DataRow("13277573621")]
        public async Task GetPhoneImageCode(string phone) {
            var stream = await YiQiKanService?.Account?.GetCodeImageStream("86", phone);
            Assert.IsNotNull(stream);
        }
        [DataTestMethod]
        [DataRow("b61026e8aeb84c10b0096d120922054a", "13277573621", "cf2d")]
        public async Task SendPhoneCode(string deviceId, string phone, string imgCode) {
            var result = await YiQiKanService?.Account?.SendPhoneCode(deviceId, "86", "中国大陆", imgCode, false, phone);
            Assert.IsTrue(result);
        }
        [DataTestMethod]
        [DataRow("b61026e8aeb84c10b0096d120922054a", "13277573621", "675020")]

        public async Task LoginTest(string deviceId, string phone, string msgCode) {
            var result = await YiQiKanService?.Account?.LoginByPhoneAndCode(deviceId, "86", "中国大陆", msgCode, "", false, "Surface-Book 3", "1.5.4", phone);
            Assert.IsNotNull(result);
            token = result.oAuth.token;
            TestContext.WriteLine(JsonSerializer.Serialize(result));
        }

        /// <summary>
        /// 测试登录是否成功
        /// </summary>
        /// <param name="account">用户</param>
        /// <param name="pwd">密码</param>
        /// <returns>是否有内容</returns>
        [DataTestMethod]
        [DataRow("13277573621", "yqkcy4486Cy", false)]
        [DataRow("13277573621", "yiqikan666", true)]
        public async Task LoginTest(string account, string pwd, bool expectSuccess) {
            if (expectSuccess) {
                var result = await YiQiKanService?.Account?.LoginByAccountAndPassword(account, pwd);
                Assert.IsNotNull(result);
                token = result.oAuth.token;
                TestContext.WriteLine(JsonSerializer.Serialize(result));
            } else {
                var exception = await Assert.ThrowsExceptionAsync<YiQiKanRequestException>(async () => {
                    var result = await YiQiKanService?.Account?.LoginByAccountAndPassword(account, pwd);
                    Assert.IsNull(result);
                });
                TestContext.WriteLine(exception?.Message);
            }
        }

        /// <summary>
        /// 测试退出登录是否成功
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>是否成功</returns>
        [DataTestMethod]
        [DataRow("a9172b402e754808bc457d7aaef9ba8c")]
        public async Task LogoutTest(string token) {
            var result = await YiQiKanService?.Account?.Logout(token);
            Assert.IsTrue(result);
        }
        /// <summary>
        /// 测试用户信息获取
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>用户信息</returns>
        [DataTestMethod]
        [DataRow("410fe137b8da4bd0a4a5a4b8043be2ef")]
        public async Task GetUserInfoTest(string token) {
            var result = await YiQiKanService?.Account?.GetUserInfo(token);
            Assert.IsNotNull(result);
            TestContext.WriteLine(JsonSerializer.Serialize(result));
        }
        /// <summary>
        /// 测试用户登录信息的保存
        /// </summary>
        /// <param name="agentCode">邀请码</param>
        /// <param name="deviceId">设备id</param>
        /// <param name="accessToken">token</param>
        /// <returns></returns>
        [DataTestMethod]
        [DataRow("15FTGg", "b61026e8aeb84c10b0096d120922054a", "ae94c9ed07d0423281f8a2a9ca650111")]
        public async Task SaveLoginLogTest(string agentCode, string deviceId, string accessToken) {
            var result = await YiQiKanService?.Account?.SavePhoneLoginLog(agentCode, deviceId, accessToken);
            Assert.IsTrue(result);
        }
        /// <summary>
        /// 测试修改密码
        /// </summary>
        /// <param name="newPassword">新密码</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        [TestMethod]
        [DataRow("yqkcy4486cy1", "yiqikan666", "c466e0881fa24eae9e182d15c1501b25")]
        public async Task ChangePasswordTest(string oldPassword, string newPassword, string token) {
            var result = await YiQiKanService?.Account?.ChangePassword(oldPassword, newPassword, token);
            Assert.IsTrue(result);
        }
        /// <summary>
        /// 修改昵称测试
        /// </summary>
        /// <param name="newNickName">新昵称</param>
        /// <returns></returns>
        [TestMethod]
        [DataRow("大头哈哈", "e50302b1e8dd4fd38f23c602ccf597c0")]
        public async Task ChangeNickNameTest(string newNickName, string token) {
            await YiQiKanService?.Account?.ChangeNickName(newNickName, token);
        }
    }
}
