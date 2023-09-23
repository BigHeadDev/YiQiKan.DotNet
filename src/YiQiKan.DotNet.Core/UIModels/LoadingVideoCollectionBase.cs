using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.Model.ResponseModel;

namespace YiQiKan.DotNet.Core.UIModels {
    public class LoadingVideoCollectionBase<T> : ObservableCollection<T>, ILoadingCollection<T> where T :VideoItem {
        protected int currentPage = 0;
        protected int totalPage = 1;
        public virtual bool HasMoreItems => currentPage < totalPage;
        public Func<int, Task<LoadingResultData<T>>>? GetLoadingItemsFunc { get; set; }
        public async Task<T[]> GetItemsAsync() {
            if (HasMoreItems) {
                try {
                    var result = await GetLoadingItemsFunc?.Invoke(currentPage + 1);
                    if (result != null) {
                        foreach (var item in result.items) {
                            this.Add(item);
                        }
                        currentPage = result.pageIndex;
                        totalPage = result.totalPage;
                        return result.items;
                    }
                    return null;
                } catch {
                    return null;
                }
            }
            return null;
        }
        public void Reset() {
            currentPage = 0;
            totalPage = 1;
            Clear();
        }
    }
}
