using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.Service;
using YiQiKan.DotNet.Service.Exceptions;

namespace YiQiKan.DotNet.Core.ViewModel {
    public partial class AccountViewModel : ObservableObject {
        public AccountViewModel() {

        }
        private readonly ISetting setting;
        private readonly IApp app;
        private readonly ILocalization l10N;
        private readonly YiQiKanService yiQiKanService;
        private readonly INavigationService navigationService;

        public AccountViewModel(ISetting setting, IApp app, ILocalization l10n, YiQiKanService yiQiKanService, INavigationService navigationService) {
            this.setting = setting;
            this.app = app;
            l10N = l10n;
            this.yiQiKanService = yiQiKanService;
            this.navigationService = navigationService;
            UserInfo = new UserInfoData {
                nickname = l10n.Get("Unknow"),
                userHead = "/Assets/Images/Account.png"
            };
        }
        [ObservableProperty]
        private bool isLoggedIn;

        [ObservableProperty]
        private UserInfoData userInfo;
        [ObservableProperty]
        private LoadingResultData<HistoryItem> previewHostoryItems;
        [ObservableProperty]
        private LoadingResultData<CollectionItem> previewCollectionItems;

        [RelayCommand]
        private async Task InitAccount() {
            if (string.IsNullOrEmpty(setting.Token)) {
                IsLoggedIn = false;
                return;
            }
            IsLoggedIn = true;
            await RefreshUserInfo();
        }



        [RelayCommand]
        private async Task Init() {
            await InitAccount();
            if (IsLoggedIn) {
                await RefreshHistory();
                await RefreshStars();
            }
        }
        [RelayCommand]
        private async Task ShowLoginDialog() {
            if (!IsLoggedIn) {
                var loginDialog = DependencyInjectionExtensions.ServiceProvider.GetService<ILoginDialog>();
                var result = await loginDialog.ShowDialog(new LoginViewModel(app, yiQiKanService, setting, l10N, loginDialog));
                if (result == Enums.DialogResult.OK) {
                    await Init();
                }
            }
        }
        [RelayCommand]
        private async Task Logout() {
            IsLoggedIn = false;
            try {
                await yiQiKanService.Account.Logout(setting.Token);
            } catch (Exception ex) { }
            setting.Token = string.Empty;
        }
        [RelayCommand]
        private void ItemClick(VideoItem videoItem) {
            var viewModel = new VideoViewModel(videoItem.movieId, yiQiKanService, setting, app);
            navigationService.NavigateTo(Enums.PageType.VideoPage, true, viewModel);
        }
        [RelayCommand]
        private void GetMoreHistory() {
            navigationService.NavigateTo(Enums.PageType.HistoryPage, true);
        }
        [RelayCommand]
        private void GetMoreCollection() {
            navigationService.NavigateTo(Enums.PageType.CollectionPage, true);
        }
        private async Task RefreshUserInfo() {
            try {
                if (UserInfo != null) {
                    UserInfo = await yiQiKanService?.Account?.GetUserInfo(setting?.Token);
                }
            } catch (YiQiKanRequestException ex) {
                if (ex.Message.Equals("Error")) {
                    setting.Token = string.Empty;
                    IsLoggedIn = false;
                    app.ShowToast(l10N.Get("TokenExpired"), 2000);
                }
            }
        }

        private async Task RefreshHistory() {
            try {
                PreviewHostoryItems = await yiQiKanService?.Video?.GetHistoryList("Movie", 1, 21, setting?.Token);
            } catch (Exception ex) {
                app.ShowToast(l10N.Get("HistoryFailed"), 2000);
            }
        }
        private async Task RefreshStars() {
            try {
                PreviewCollectionItems = await yiQiKanService?.Video?.GetCollectList("Movie", 1, 21, setting.Token);
            } catch (Exception ex) {
                app.ShowToast(l10N.Get("CollectionFailed"), 2000);
            }
        }

    }
}
