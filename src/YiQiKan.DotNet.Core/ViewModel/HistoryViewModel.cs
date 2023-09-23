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
    public partial class HistoryViewModel : ObservableObject {
        private readonly YiQiKanService yiQiKanService;
        private readonly ILocalization l10N;
        private readonly ISetting setting;
        private readonly IApp app;
        private readonly INavigationService navigationService;
        [ObservableProperty]
        private ILoadingCollection<HistoryItem> loadingItems;
        [ObservableProperty]
        private bool isLoading = true;
        [ObservableProperty]
        private string header;
        public HistoryViewModel(YiQiKanService yiQiKanService,ILocalization l10n, ILoadingCollection<HistoryItem> historyItems, ISetting setting, IApp app, INavigationService navigationService) {
            this.yiQiKanService = yiQiKanService;
            l10N = l10n;
            LoadingItems = historyItems;
            LoadingItems.GetLoadingItemsFunc = LoadMoreListData;
            this.setting = setting;
            this.app = app;
            this.navigationService = navigationService;
            Header = l10N.Get("History");
        }

        private async Task<LoadingResultData<HistoryItem>> LoadMoreListData(int pageIndex) {
            IsLoading = true;
            LoadingResultData<HistoryItem> result = null;
            try {
                result = await yiQiKanService.Video.GetHistoryList("Movie", pageIndex, 21, setting.Token);
            } catch {
                app.ShowToast(l10N.Get("MoreDataFailed"), 2000);
            }
            IsLoading = false;
            return result;
        }

        [RelayCommand]
        private void ItemClick(VideoItem videoItem) {
            var viewModel = new VideoViewModel(videoItem.movieId, yiQiKanService, setting, app);
            navigationService.NavigateTo(Enums.PageType.VideoPage, true, viewModel);
        }
    }
}
