using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Extensions.DependencyInjection;
using YiQiKan.DotNet.Core.ViewModel;
using System.Reflection;
using Windows.UI.Core;
using Windows.Storage;
using YiQiKan.DotNet.Core.Enums;
using YiQiKan.DotNet.Core.Interface;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace YiQiKan.DotNet.UWP {
    /// <summary>
    /// 此页面是整个App的页面，它包含一个标题栏，和一个Frame
    /// </summary>
    public sealed partial class AppShellPage : Page {
        private ApplicationViewTitleBar titleBar;
        public AppShellPage() {
            this.InitializeComponent();
            DataContext = DependencyInjectionExtensions.ServiceProvider.GetService<MainViewModel>();
            titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            // Hide default title bar.
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            UpdateTitleBarLayout(coreTitleBar);

            // Set XAML element as a draggable region.
            Window.Current.SetTitleBar(AppTitleBar);

            // Register a handler for when the size of the overlaid caption control changes.
            // For example, when the app moves to a screen with a different DPI.
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            // Register a handler for when the title bar visibility changes.
            // For example, when the title bar is invoked in full screen mode.
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;

            //Register a handler for when the window changes focus
            Window.Current.Activated += Current_Activated;
            ActualThemeChanged += AppShellPage_ActualThemeChanged;
            Loaded += AppShellPage_Loaded;
        }

        private void AppShellPage_Loaded(object sender, RoutedEventArgs e) {
            var theme = DependencyInjectionExtensions.ServiceProvider.GetService<ISetting>().Theme;
            DependencyInjectionExtensions.ServiceProvider.GetService<IApp>().ChangeTheme(theme);
        }

        private void AppShellPage_ActualThemeChanged(FrameworkElement sender, object args) {
            _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                Color bgColor = Colors.Transparent;
                Color fgColor = ((SolidColorBrush)Resources["ButtonForegroundColor"]).Color;
                Color inactivefgColor = ((SolidColorBrush)Resources["ButtonInactiveForegroundBrush"]).Color;
                Color hoverbgColor = ((SolidColorBrush)Resources["ButtonHoverBackgroundBrush"]).Color;
                Color hoverfgColor = ((SolidColorBrush)Resources["ButtonHoverForegroundBrush"]).Color;
                Color pressedbgColor = ((SolidColorBrush)Resources["ButtonPressedBackgroundBrush"]).Color;
                Color pressedfgColor = ((SolidColorBrush)Resources["ButtonPressedForegroundBrush"]).Color;
                titleBar.ButtonBackgroundColor = bgColor;
                titleBar.ButtonForegroundColor = fgColor;
                titleBar.ButtonInactiveBackgroundColor = bgColor;
                titleBar.ButtonInactiveForegroundColor = inactivefgColor;
                titleBar.ButtonHoverBackgroundColor = hoverbgColor;
                titleBar.ButtonHoverForegroundColor = hoverfgColor;
                titleBar.ButtonPressedBackgroundColor = pressedbgColor;
                titleBar.ButtonPressedForegroundColor = pressedfgColor;
            });
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args) {
            UpdateTitleBarLayout(sender);
        }
        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar) {
            // Update title bar control size as needed to account for system size changes.
            //AppTitleBar.Height = coreTitleBar.Height;

            // Ensure the custom title bar does not overlap window caption controls
            Thickness currMargin = AppTitleBar.Margin;
            AppTitleBar.Margin = new Thickness(currMargin.Left, currMargin.Top, coreTitleBar.SystemOverlayRightInset, currMargin.Bottom);
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args) {
            if (sender.IsVisible) {
                AppTitleBar.Visibility = Visibility.Visible;
            } else {
                AppTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        // Update the TitleBar based on the inactive/active state of the app
        private void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e) {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated) {
                AppTitle.Opacity = 0.7;
            } else {
                AppTitle.Opacity = 1.0;
            }
        }

        public Frame RootFrame {
            get {
                return rootFrame;
            }
        }


        private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
            searchRankTeachingTip.IsOpen = false;
        }

    }
}
