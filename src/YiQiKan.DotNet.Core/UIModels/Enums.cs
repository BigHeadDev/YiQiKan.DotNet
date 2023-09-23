using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Core.Enums {
    public enum DialogResult {
        OK,
        Cancel,
        Close
    }
    public enum ThemeType {
        System,
        Light,
        Dark
    }
    public enum PageType {
        SplashPage,
        MainPage,
        HomePage,
        SportPage,
        LivePage,
        AccountPage,
        SettingPage,
        VideoPage,
        SearchPage,
        MoreListPage,
        HistoryPage,
        CollectionPage
    }
    public enum InitState {
        Processing,
        Failed,
        Success
    }
}
