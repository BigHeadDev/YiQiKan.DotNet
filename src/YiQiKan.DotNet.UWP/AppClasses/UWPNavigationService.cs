using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using YiQiKan.DotNet.Core.Enums;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.UWP.Pages;

namespace YiQiKan.DotNet.UWP.AppClasses {
    /// <summary>
    /// UWP的INavigationService实现
    /// </summary>
    internal class UWPNavigationService : INavigationService {
        public event EventHandler<PageType> RootNavigated;
        public event EventHandler<PageType> ChildNavigated;
        //主Frame，来自AppShellPage.xaml中的Frame控件，它显示整个软件的界面
        private Frame rootFrame;
        private Frame RootNavigationService {
            get {
                if (rootFrame == null) {
                    rootFrame = ((Window.Current.Content as Frame).Content as AppShellPage).RootFrame;
                    rootFrame.Navigated += RootFrame_Navigated;
                }
                return rootFrame;
            }
        }

        //主Frame导航完毕之后，会触发事件（把Frame自带的事件包装了一下）
        private void RootFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e) {
            var pageName = e.SourcePageType.Name;
            var pageType = (PageType)Enum.Parse(typeof(PageType), pageName);
            RootNavigated?.Invoke(this, pageType);
        }

        //子Frame，来自MainPage.xaml的Frame控件
        private Frame childFrame;
        private Frame ChildNavigationService {
            get {
                if (childFrame == null && RootNavigationService.Content is MainPage mainPage) {
                    childFrame = mainPage.ChildFrame;
                    childFrame.Navigated += ChildFrame_Navigated;
                }
                return childFrame;
            }
        }


        public PageType CurrentRootPage => (PageType)Enum.Parse(typeof(PageType), RootNavigationService.Content.GetType().Name);


        public PageType CurrentChildPage => (PageType)Enum.Parse(typeof(PageType), ChildNavigationService.Content.GetType().Name);

        //子Frame导航完毕，会触发子导航事件（把Frame自带的事件包装了一下）
        private void ChildFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e) {
            var pageName = e.SourcePageType.Name;
            var pageType = (PageType)Enum.Parse(typeof(PageType), pageName);
            ChildNavigated?.Invoke(this, pageType);
        }

        /// <summary>
        /// UWP平台实现GoBack，根据isRoot来区分是哪个Frame执行GoBack
        /// </summary>
        /// <param name="isRoot"></param>
        public void GoBack(bool isRoot = false) {
            if (isRoot) {
                RootNavigationService?.GoBack();
            } else {
                ChildNavigationService?.GoBack();
            }
        }


        /// <summary>
        /// UWP平台的GoForward实现，根据isRoot来区分不同的Frame执行
        /// </summary>
        /// <param name="isRoot"></param>
        public void GoForward(bool isRoot = false) {
            if (isRoot) {
                RootNavigationService?.GoForward();
            } else {
                ChildNavigationService?.GoForward();
            }
        }

        /// <summary>
        /// UWP平台的导航方法实现
        /// </summary>
        /// <param name="pageType">页面类型</param>
        /// <param name="isRoot">主导航还是子导航</param>
        /// <param name="param">导航参数</param>
        public void NavigateTo(PageType pageType, bool isRoot = false, object param = null) {
            var page = Type.GetType("YiQiKan.DotNet.UWP.Pages." + pageType);
            if (isRoot) {
                if (RootNavigationService.Content == null || RootNavigationService.Content.GetType() != page) {
                    RootNavigationService?.Navigate(page, param);
                }
            } else {
                if (ChildNavigationService.Content == null || ChildNavigationService.Content.GetType() != page) {
                    ChildNavigationService?.Navigate(page, param);
                }
            }
        }
    }
}
