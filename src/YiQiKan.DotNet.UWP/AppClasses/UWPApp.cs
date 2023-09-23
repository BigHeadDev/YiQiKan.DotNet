using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using YiQiKan.DotNet.Core.Enums;
using YiQiKan.DotNet.Core.Interface;

namespace YiQiKan.DotNet.UWP.AppClasses {
    /// <summary>
    /// UWP平台App的一些实现，主要是UWP中特有的一些功能或者界面属性绑定
    /// 和一起看服务流程无关，主要是为了实现一些界面而产生
    /// </summary>
    public partial class UWPApp : ObservableObject, IApp {
        private readonly INavigationService navigationService;

        public UWPApp(INavigationService navigationService) {
            this.navigationService = navigationService;
            navigationService.RootNavigated += NavigationService_RootNavigated;
        }

        /// <summary>
        /// 订阅主导航的事件
        /// 根据当前导航的页面，显示UWP中的标题栏的一些按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigationService_RootNavigated(object sender, PageType e) {
            if (e.Equals(PageType.SplashPage)) {
                IsBackButtonVisible = false;
                IsSearchBoxVisible = false;
                IsMenuButtonVisible = false;
            } else if (e.Equals(PageType.MainPage)) {
                IsMenuButtonVisible = true;
                IsBackButtonVisible = false;
                IsSearchBoxVisible = true;
            } else {
                IsMenuButtonVisible = false;
                IsBackButtonVisible = true;
                IsSearchBoxVisible = true;
            }
        }
        //UWP的NavigationView的菜单展开隐藏属性
        [ObservableProperty]
        private bool isMenuOpen = false;
        //UWP的标题栏返回按钮显示隐藏属性
        [ObservableProperty]
        private bool isBackButtonVisible = false;
        //UWP的标题栏NavigationView绑定的菜单按钮显示隐藏属性
        [ObservableProperty]
        private bool isMenuButtonVisible = false;
        //UWP标题栏收缩框的显示隐藏属性
        [ObservableProperty]
        private bool isSearchBoxVisible = false;

        public Assembly Assembly => Assembly.GetExecutingAssembly();

        public string DataPath { get => Path.Combine(ApplicationData.Current.LocalFolder.Path, "yiqikan.db"); }

        //UWP的弹窗实现
        public DialogResult ShowDialog(string title, string message) {
            new ToastContentBuilder()
                .AddText(title)
                .AddText(message)
                .Show();
            return DialogResult.OK;
        }

        //UWP的土司通知实现
        public void ShowToast(string message, double seconds) {
            new ToastContentBuilder()
                .AddText("提示")
                .AddText(message)
                .Show();
        }

        public async Task UIInvoke(Action action) {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () => {
                action.Invoke();
            });
        }

        public void ChangeTheme(ThemeType themeType) {
            if (themeType == ThemeType.Light) {
                (Window.Current.Content as FrameworkElement).RequestedTheme = ElementTheme.Light;
            }
            if(themeType == ThemeType.Dark) {
                (Window.Current.Content as FrameworkElement).RequestedTheme = ElementTheme.Dark;
            }
            if(themeType == ThemeType.System) {
                (Window.Current.Content as FrameworkElement).RequestedTheme = ElementTheme.Default;
            }
        }
    }
}
