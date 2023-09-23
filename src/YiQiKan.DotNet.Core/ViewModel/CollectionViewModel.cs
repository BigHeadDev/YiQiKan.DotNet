using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.Service;

namespace YiQiKan.DotNet.Core.ViewModel {
    public partial class CollectionViewModel : ObservableObject {
        private readonly YiQiKanService yiQiKanService;
        private readonly ILocalization l10N;
        private readonly IApp app;
        private readonly ISetting setting;
        private readonly INavigationService navigationService;
        [ObservableProperty]
        private ILoadingCollection<CollectionItem> loadingItems;
        [ObservableProperty]
        private bool isLoading = true;
        [ObservableProperty]
        private string header;

        public CollectionViewModel(YiQiKanService yiQiKanService,ILocalization l10n, ILoadingCollection<CollectionItem> collectionItems,IApp app, ISetting setting, INavigationService navigationService) {
            this.yiQiKanService = yiQiKanService;
            l10N = l10n;
            LoadingItems = collectionItems;
            this.app = app;
            LoadingItems.GetLoadingItemsFunc = LoadMoreListData;
            this.setting = setting;
            this.navigationService = navigationService;
            Header = l10n.Get("MyCollection");
        }

        

        private async Task<LoadingResultData<CollectionItem>> LoadMoreListData(int pageIndex) {
            IsLoading = true;
            LoadingResultData<CollectionItem> result = null;
            try {
                result = await yiQiKanService.Video.GetCollectList("Movie", pageIndex, 21, setting.Token);
            } catch {
                app.ShowToast(l10N.Get("MoreDataFailed"), 2000);
            }
            IsLoading = false;
            return result;
        }

        [RelayCommand]
        private void ItemClick(VideoItem videoItem) {
            var viewModel = new VideoViewModel(videoItem.movieId, yiQiKanService, setting,app);
            navigationService.NavigateTo(Enums.PageType.VideoPage, true, viewModel);
        }
    }
}
