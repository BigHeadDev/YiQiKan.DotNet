using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YiQiKan.DotNet.Core.Enums;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.UWP.AppClasses;
using YiQiKan.DotNet.UWP.Helpers;
using YiQiKan.DotNet.UWP.Pages;
using YiQiKan.DotNet.UWP.UIModels;

namespace YiQiKan.DotNet.UWP {
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App() {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            UnhandledException += App_UnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e) {
            //exit ...
        }

        private void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e) {
            e.Handled = true;
            ShowErrorDialog(e.Exception);
        }

        private void RegisterExceptionHandlingSynchronizationContext() {
            ExceptionHandlingSynchronizationContext
                .Register()
                .UnhandledException += SynchronizationContext_UnhandledException;
        }
        private ILocalization l10n;
        private ILocalization l10N {
            get {
                if (l10n == null) {
                    l10n = DependencyInjectionExtensions.ServiceProvider.GetService<ILocalization>();
                }
                return l10n;
            }
        }
        private void SynchronizationContext_UnhandledException(object sender, Helpers.UnhandledExceptionEventArgs e) {
            e.Handled = true;
            ShowErrorDialog(e.Exception);
        }

        private async void ShowErrorDialog(Exception e) {
            var title = "Error (⊙﹏⊙)";
            var error = "Error Info:";
            if (l10N != null) {
                title = l10N.Get("ExceptionHappend");
                error = l10N.Get("ExceptionInfo");
            }
            await new ContentDialog() {
                Title = title,
                Content = $"{error}\r\nMessage------>\r\n{e.Message}\r\n\r\nStackTrace------>\r\n{e.StackTrace}",
                DefaultButton = ContentDialogButton.Close,
                CloseButtonText = l10N.Get("Close")
            }.ShowAsync();
        }

        protected override void OnActivated(IActivatedEventArgs args) {
            RegisterExceptionHandlingSynchronizationContext();
            base.OnActivated(args);
        }
        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e) {
            RegisterExceptionHandlingSynchronizationContext();
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null) {
                // 创建要充当导航上下文的框架，并导航到第一页
                var services = new ServiceCollection();
                services.AddCoreApp<UWPApp, UWPNavigationService, UWPSetting, Localization, LoginDialog, IncrementalLoadingVideoCollection<SearchItem>, IncrementalLoadingVideoCollection<VideoItem>, IncrementalLoadingVideoCollection<CollectionItem>, IncrementalLoadingVideoCollection<HistoryItem>>();
                DependencyInjectionExtensions.ServiceProvider = services.BuildServiceProvider();


                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false) {
                if (rootFrame.Content == null) {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数

                    //【软件主页面】
                    rootFrame.Navigate(typeof(AppShellPage), e.Arguments);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e) {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
    }
}
