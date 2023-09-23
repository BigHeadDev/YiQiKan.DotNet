using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using YiQiKan.DotNet.Core;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.Core.UIModels;
using YiQiKan.DotNet.Core.ViewModel;
using YiQiKan.DotNet.DataBase;
using YiQiKan.DotNet.DataBase.DBContexts;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.Service;

namespace YiQiKan.DotNet {
    public static class DependencyInjectionExtensions {
        public static IServiceCollection AddCoreApp<App, Navigation, Setting, Localization, LoginDialog, SearchCollection, MoreListCollection, CollectionCollection, HistoryCollection>(this IServiceCollection services)
            where App : class, IApp
            where Navigation : class, INavigationService
            where Setting : class, ISetting
            where Localization : class, ILocalization
            where LoginDialog : class, ILoginDialog
            where SearchCollection : class, ILoadingCollection<SearchItem>
            where MoreListCollection : class, ILoadingCollection<VideoItem>
            where CollectionCollection : class, ILoadingCollection<CollectionItem>
            where HistoryCollection : class, ILoadingCollection<HistoryItem> {
            services.AddSingleton<YiQiKanService>();
            services.AddSingleton<IApp, App>();
            services.AddSingleton<INavigationService, Navigation>();
            services.AddSingleton<ISetting, Setting>();
            services.AddSingleton<ILocalization, Localization>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<SearchViewModel>();
            services.AddSingleton<AccountViewModel>();
            services.AddSingleton<SettingViewModel>();
            services.AddTransient<HistoryViewModel>();
            services.AddTransient<CollectionViewModel>();
            services.AddTransient<ILoginDialog, LoginDialog>();
            services.AddTransient<ILoadingCollection<SearchItem>, SearchCollection>();
            services.AddTransient<ILoadingCollection<VideoItem>, MoreListCollection>();
            services.AddTransient<ILoadingCollection<CollectionItem>, CollectionCollection>();
            services.AddTransient<ILoadingCollection<HistoryItem>, HistoryCollection>();
            services.AddSingleton((s) => {
                return new VideoDBContext(s.GetService<IApp>().DataPath);
            });
            services.AddSingleton<VideoHistoryMananger>();
            return services;
        }
        public static IServiceProvider? ServiceProvider { get; set; }
    }
}
