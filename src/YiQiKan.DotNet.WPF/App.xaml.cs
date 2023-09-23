using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.WPF.AppClasses;
using YiQiKan.DotNet.WPF.Pages;

namespace YiQiKan.DotNet.WPF {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCoreApp<WPFApp,WPFNavigationService,WPFSetting>()
                .AddSingleton<MainWindow>()
                .AddTransient<HomePage>()
                    .AddTransient<SportPage>()
                    .AddTransient<LivePage>()
                    .AddTransient<AccountPage>();
            DenpendencyInjectionExtensions.ServiceProvider = serviceCollection.BuildServiceProvider();
            DenpendencyInjectionExtensions.ServiceProvider.GetService<MainWindow>()?.ShowDialog();
        }
    }
}
