using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.Model.Enums;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.Service;

namespace YiQiKan.DotNet.Core.ViewModel {
    public partial class MoreListViewModel : ObservableObject {
        private readonly IApp app;
        private readonly Category category;
        private readonly ILocalization l10N;
        private readonly YiQiKanService yiQiKanService;
        private readonly ISetting setting;
        private readonly INavigationService navigationService;
        [ObservableProperty]
        private ILoadingCollection<VideoItem> loadingItems;
        [ObservableProperty]
        private string header;

        public MoreListViewModel() { }
        public MoreListViewModel(IApp app,Category category,ILocalization l10n, string type, ILoadingCollection<VideoItem> videos, YiQiKanService yiQiKanService, ISetting setting,INavigationService navigationService) {
            this.app = app;
            this.category = category;
            l10N = l10n;
            Header = type;
            LoadingItems = videos;
            LoadingItems.GetLoadingItemsFunc = LoadMoreListData;
            this.yiQiKanService = yiQiKanService;
            this.setting = setting;
            this.navigationService = navigationService;
        }
        [ObservableProperty]
        private bool isLoading = true;



        private async Task<LoadingResultData<VideoItem>> LoadMoreListData(int pageIndex) {
            IsLoading = true;
            LoadingResultData<VideoItem> result = null;
            try {
                result = await yiQiKanService.Video.GetMoreListAsync(category, Header, pageIndex, 21, setting.Token);
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
