using Microsoft.VisualStudio.TestTools.UnitTesting;
using YiQiKan.DotNet.Service;

namespace YiQiKan.DotNet.Test {
    /// <summary>
    /// 说明：此类为一起看服务单元测试类
    /// </summary>
    [TestClass]
    public class DomainTest : ServiceTestBase {
        /// <summary>
        /// 测试初始化域名是否成功
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task InitTest() {
            var result = await YiQiKanService?.Init();
            Assert.IsTrue(result);
        }
    }
}
