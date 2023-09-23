using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Model.ResponseModel;

namespace YiQiKan.DotNet.Core.Interface {
    public interface ILoadingCollection<T> where T : VideoItem {
        Func<int, Task<LoadingResultData<T>>>? GetLoadingItemsFunc { get; set; }
        void Reset();
        Task<T[]> GetItemsAsync();
    }
}
