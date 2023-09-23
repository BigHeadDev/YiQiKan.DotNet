using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Service;

namespace YiQiKan.DotNet.Test {
    [TestClass]
    public abstract class ServiceTestBase {
        private TestContext testContext;

        public TestContext TestContext {
            get { return testContext; }
            set { testContext = value; }
        }
        protected YiQiKanService? YiQiKanService { get; set; }
        public ServiceTestBase()
        {
            YiQiKanService = new YiQiKanService();
            YiQiKanService.Init("https://api-aws.ctdy.cc/");
        }
    }
}
