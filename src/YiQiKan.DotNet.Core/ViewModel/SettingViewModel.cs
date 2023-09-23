using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Core.Enums;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.DataBase;
using YiQiKan.DotNet.DataBase.DBContexts;

namespace YiQiKan.DotNet.Core.ViewModel {
    public partial class SettingViewModel : ObservableObject {
        public SettingViewModel(IApp app, ISetting setting, ILocalization l10n, VideoHistoryMananger videoHistoryMananger) {
            this.app = app;
            this.setting = setting;
            l10N = l10n;
            this.videoHistoryMananger = videoHistoryMananger;
            themes = new Dictionary<ThemeType, string> {
                    {ThemeType.System,l10N.Get("SystemTheme") },
                    {ThemeType.Light ,l10N.Get("LightTheme")},
                    {ThemeType.Dark ,l10N.Get("DarkTheme")},
                };
            selectedTheme = Themes.FirstOrDefault(t => t.Key == setting.Theme);
        }

        [ObservableProperty]
        private Dictionary<ThemeType, string> themes;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedThemeIndex))]
        private KeyValuePair<ThemeType, string> selectedTheme;
        private readonly IApp app;
        private readonly ISetting setting;
        private readonly ILocalization l10N;
        private readonly VideoHistoryMananger videoHistoryMananger;

        public int SelectedThemeIndex => (int)SelectedTheme.Key;
        partial void OnSelectedThemeChanged(KeyValuePair<ThemeType, string> value) {
            setting.Theme = value.Key;
            app.ChangeTheme(setting.Theme);
        }



        [RelayCommand]
        private async Task ClearCache() {
            bool result = false;
            try {
               result = await videoHistoryMananger.Reset();
            } catch { }
            if (result) {
                app.ShowToast(l10N.Get("OptionSuccess"), 2000);
            } else {
                app.ShowToast(l10N.Get("OptionFailed"), 2000);
            }

        }
    }
}
