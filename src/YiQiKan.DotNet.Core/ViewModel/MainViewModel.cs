using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Core.Enums;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.DataBase;
using YiQiKan.DotNet.DataBase.DBContexts;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.Service;

namespace YiQiKan.DotNet.Core.ViewModel {
    /// <summary>
    /// 主ViewModel，作为第一个页面SplashPage、第二个页面MainPage的ViewModel
    /// 1，负责一起看服务初始化，主页跳转
    /// 2，负责主页的菜单跳转
    /// 3，负责标题显示
    /// </summary>
    public partial class MainViewModel : ObservableObject {
        //导航服务
        private readonly INavigationService navigationService;
        private readonly AccountViewModel account;
        private readonly SearchViewModel searchViewModel;

        //一起看服务
        private readonly YiQiKanService yiQiKanService;
        private readonly ISetting setting;
        private readonly VideoHistoryMananger videoHistoryMananger;

        //app服务（平台相关的功能）
        [ObservableProperty]
        private IApp app;

        //标题
        [ObservableProperty]
        private string title = "一起看";

        //初始化状态属性
        [ObservableProperty]
        private InitState initState = InitState.Processing;

        //热搜列表
        [ObservableProperty]
        private HotSearchItem[] hotSearchItems;
        [ObservableProperty]
        private bool isHotSearchItemsVisible = true;
        [ObservableProperty]
        private string keyWordsInput = "";

        public MainViewModel(IApp app, INavigationService navigationService,AccountViewModel account, SearchViewModel searchViewModel, YiQiKanService yiQiKanService, ISetting setting, VideoHistoryMananger videoHistoryMananger) {
            this.App = app;
            this.navigationService = navigationService;
            this.account = account;
            this.searchViewModel = searchViewModel;
            this.yiQiKanService = yiQiKanService;
            this.setting = setting;
            this.videoHistoryMananger = videoHistoryMananger;
        }
        public MainViewModel() {

        }
        [RelayCommand]
        private void QuerySubmitted(string queryText) {
            if (string.IsNullOrEmpty(queryText)) {
                return;
            }
            searchViewModel.KeyWords = queryText;
            if (navigationService.CurrentRootPage != PageType.SearchPage) {
                navigationService.NavigateTo(PageType.SearchPage, true, searchViewModel);
            }
            searchViewModel.SearchCommand.Execute(null);
        }

        [RelayCommand]
        private void HotwordsClick(HotKeywords hotKeywords) {
            KeyWordsInput = hotKeywords.name;
            QuerySubmitted(hotKeywords.name);
        }

        /// <summary>
        /// 初始化一起看的域名服务
        /// 成功就进入主页，失败就显示失败
        /// 【注意】SplashPage的显示界面根据InitState来变化
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task Init() {
            InitState = InitState.Processing;
            var initResult = await yiQiKanService.Init(app.Assembly);
            if (!initResult) {
                InitState = InitState.Failed;
            } else {
                _ = Task.Run(async () => {
                    var dbInitState = videoHistoryMananger.InitDataBase();
                    if (HotSearchItems == null) {
                        try {
                            HotSearchItems = await yiQiKanService?.Search?.GetHotSearchItems(setting.Token);
                            IsHotSearchItemsVisible = HotSearchItems != null;
                        } catch {
                            IsHotSearchItemsVisible = false;
                        }
                    }
                    if (setting.IsLogin) {
                        await account.InitAccountCommand.ExecuteAsync(null);
                        await yiQiKanService.Account.SavePhoneLoginLog(string.Empty, setting.DeviceId, setting.Token);
                    }
                });

                navigationService.NavigateTo(PageType.MainPage, true);//主Frame导航到MainPage
                navigationService.NavigateTo(PageType.HomePage);//MainPage内部的Frame导航到HomePage
            }
        }

        /// <summary>
        /// MainPage的菜单点击事件传递到这里
        /// </summary>
        /// <param name="page"></param>
        [RelayCommand]
        private void ItemInvoked(string page) {
            var pageType = (PageType)Enum.Parse(typeof(PageType), page);
            navigationService.NavigateTo(pageType);//MainPage内部的Frame导航到菜单点击的页面
        }
    }
}
