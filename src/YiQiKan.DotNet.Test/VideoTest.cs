using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YiQiKan.DotNet.Model.Enums;

namespace YiQiKan.DotNet.Test {
    [TestClass]
    public class VideoTest :ServiceTestBase{
        [DataTestMethod]
        [DataRow(Category.Recommend,"")]
        public async Task GetHomeListTest(Category category,string token) {
            var result = await YiQiKanService?.Video?.GetHomeListAsync(category,token);
            Assert.IsNotNull(result);
            TestContext.WriteLine(JsonSerializer.Serialize(result));
        }
        [DataTestMethod]
        [DataRow(Category.Recommend,"最近更新",1,21,"")]
        public async Task GetMoreListTest(Category category,string type,int pageIndex,int pageCount,string token) {
            var result = await YiQiKanService?.Video?.GetHomeListAsync(category, token);
            Assert.IsNotNull(result);
            TestContext.WriteLine(JsonSerializer.Serialize(result));
        }

        [DataTestMethod]
        [DataRow("", "", "", "")]
        public async Task GuessYouLike(string movieId, string tvid, string scheduleId, string token) {
            var result = await YiQiKanService?.Video?.GetRecommendationListAsync(movieId, tvid, scheduleId, token);
            Assert.IsNotNull(result);
            TestContext.WriteLine(JsonSerializer.Serialize(result));
        }


        [DataTestMethod]
        [DataRow("")]
        public async Task HotSearch(string token) {
            var result = await YiQiKanService?.Search.GetHotSearchItems(token);
            Assert.IsNotNull(result);
            TestContext.WriteLine(JsonSerializer.Serialize(result));
        }
        [DataTestMethod]
        [DataRow("你好",1,21,"")]
        public async Task Search(string keyWords,int index,int count) {
            var result = await YiQiKanService?.Search.GetMovieByOptions(keyWords,"-1", "-1", "-1", "-1", "-1","-1", index, count, "");
            Assert.IsNotNull(result);
            TestContext.WriteLine(JsonSerializer.Serialize(result));
        }
    }
}
