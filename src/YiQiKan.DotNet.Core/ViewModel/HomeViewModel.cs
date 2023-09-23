using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.Core.UIModels;
using YiQiKan.DotNet.Model.Enums;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.Service;

namespace YiQiKan.DotNet.Core.ViewModel {

    /// <summary>
    /// 作为HomePage的ViewModel
    /// </summary>
    public partial class HomeViewModel : ObservableRecipient {
        public HomeViewModel(YiQiKanService yiQiKanService, IApp app, ISetting setting,ILocalization l10n, INavigationService navigationService) {
            this.yiQiKanService = yiQiKanService;
            this.app = app;
            this.setting = setting;
            l10N = l10n;
            this.navigationService = navigationService;
        }
        public HomeViewModel() {

        }
        //首页的tab标签
        [ObservableProperty]
        private ObservableCollection<HomeTabData> tabs = new ObservableCollection<HomeTabData> {
            new HomeTabData("推荐",Category.Recommend ),
            new HomeTabData("电影",Category.Film ),
            new HomeTabData("电视剧",Category.TVSeries ),
            new HomeTabData("美剧",Category.America ),
            new HomeTabData("韩剧",Category.Korea ),
            new HomeTabData("综艺",Category.VarietyShow),
            new HomeTabData("动漫",Category.Anime ),
            new HomeTabData("纪录片",Category.Documentary)
        };
        //当前选中的标签
        [ObservableProperty]
        private HomeTabData selectedTab;
        //一起看服务
        private readonly YiQiKanService yiQiKanService;
        private readonly IApp app;

        //设置服务
        private readonly ISetting setting;
        private readonly ILocalization l10N;

        //导航服务
        private readonly INavigationService navigationService;


        /// <summary>
        /// 切换上方的Pivot会触发这个命令
        /// 推荐、电影、电视剧、美剧、韩剧、综艺、纪录片
        /// 比如切换到电影，会触发这个命令，然后会去请求电影的数据
        /// 如果已经请求过了，就不会再去请求了
        /// </summary>
        /// <param name="selectedHomeTabData">切换的Tab数据</param>
        /// <returns></returns>
        [RelayCommand]
        private async Task TabSelectionChanged(HomeTabData selectedHomeTabData) {
            if (selectedHomeTabData.Data == null) {
                try {
                    selectedHomeTabData.Data = await yiQiKanService.Video.GetHomeListAsync(selectedHomeTabData.Category, setting.Token);
                }catch(Exception ex) {
                    app.ShowToast(l10N.Get("GetDataFailed"), 2000);
                }
            }
        }

        /// <summary>
        /// 点击了某个视频，会触发这个命令
        /// 动态构造一个ViewModel，然后跳转到VideoPage
        /// 【注意】这里的跳转，是主Frame跳转
        /// </summary>
        /// <param name="videoItem">点击的视频</param>
        [RelayCommand]
        private void ItemClick(VideoItem videoItem) {
            var viewModel = new VideoViewModel(videoItem.movieId, yiQiKanService, setting,app);
            navigationService.NavigateTo(Enums.PageType.VideoPage, true, viewModel);
        }

        /// <summary>
        /// 点击了某个Banner，会触发这个命令
        /// 动态构造一个ViewModel，然后跳转到VideoPage
        /// 【注意】这里的跳转，是主Frame跳转
        /// </summary>
        /// <param name="banner"></param>
        [RelayCommand]
        private void TopBannerClick(Topbanner banner) {
            var viewModel = new VideoViewModel(banner.movieId, yiQiKanService, setting, app);
            navigationService.NavigateTo(Enums.PageType.VideoPage, true, viewModel);
        }

        [RelayCommand]
        private void GetMoreList(string type) {
            var moreListViewModel = new MoreListViewModel(app,SelectedTab.Category, l10N,  type, DependencyInjectionExtensions.ServiceProvider.GetService<ILoadingCollection<VideoItem>>(), yiQiKanService, setting, navigationService);
            navigationService.NavigateTo(Enums.PageType.MoreListPage, true, moreListViewModel);
        }
    }
}
