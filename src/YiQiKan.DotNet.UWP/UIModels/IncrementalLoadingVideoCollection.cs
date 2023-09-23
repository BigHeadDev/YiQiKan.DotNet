using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using YiQiKan.DotNet.Core.UIModels;
using YiQiKan.DotNet.Model.ResponseModel;

namespace YiQiKan.DotNet.UWP.UIModels {
    public class IncrementalLoadingVideoCollection<T> : LoadingVideoCollectionBase<T>, ISupportIncrementalLoading where T : VideoItem {
        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count) {
            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        private async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken c, uint count) {
            uint resultCount = 0;
            var newItems = await GetItemsAsync();
            resultCount = (uint)newItems?.Length;
            return new LoadMoreItemsResult { Count = resultCount };
        }
    }
}
