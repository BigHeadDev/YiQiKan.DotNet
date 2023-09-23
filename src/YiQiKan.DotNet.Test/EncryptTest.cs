using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Utils;

namespace YiQiKan.DotNet.Test {
    [TestClass]
    public class EncryptTest {
        /// <summary>
        /// 手机号登录日志加解密测试
        /// </summary>
        /// <param name="src">源字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">偏移</param>
        /// <param name="result">结果对比</param>
        [TestMethod]
        [DataRow("agentCode=15FTGg&onlyIdentificationm=b61026e8aeb84c10b0096d120922054a&ts=1683793266015", "9JFPeqizqMhOGX1t", "A-16-Byte-String", "4964913fa0de190836c997cd7366b322a318adc333c27dda0d7c112695aa2e27")]
        public void AESEncypt(string src, string key, string iv, string result) {
            var hash = EncryptHelper.AesEecrypt(src, Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv)).GetSha256Data().BytesToString();
            Assert.Equals(result, hash);
        }
    }
}
