using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Core.Enums;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.Core.UIModels;
using YiQiKan.DotNet.Model.Enums;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.Service;

namespace YiQiKan.DotNet.Core.ViewModel {
    public partial class SearchViewModel : ObservableObject {
        private readonly YiQiKanService yiQiKanService;
        private readonly IApp app;
        private readonly ILocalization l10N;
        private readonly INavigationService navigationService;
        private readonly ISetting setting;
        [ObservableProperty]
        private string header;
        public SearchViewModel(YiQiKanService yiQiKanService, IApp app,ILocalization l10n, ILoadingCollection<SearchItem> searchItems, INavigationService navigationService, ISetting setting) {
            LoadingItems = searchItems;
            LoadingItems.GetLoadingItemsFunc = InnerSearch;
            var years = new Dictionary<string, string> {
                { "全部年份", "-1" }
            };
            var year = DateTime.Now.Year;
            for (int i = year; i >= 2017; i--) {
                var yearItem = i.ToString();
                years.Add(yearItem, yearItem);
            }
            years.Add("10-16年", "2010-2016");
            years.Add("00年代", "2000-2009");
            years.Add("90年代", "1990-1999");
            years.Add("80年代", "1980-1989");
            years.Add("更早", "<=1979");
            Years = years;
            this.yiQiKanService = yiQiKanService;
            this.app = app;
            l10N = l10n;
            this.navigationService = navigationService;
            this.setting = setting;

            SelectedCategory = Categories.FirstOrDefault();
            SelectedArea = Areas.FirstOrDefault();
            SelectedYear = Years.FirstOrDefault();
            SelectedSort = Sorts.FirstOrDefault();
            SelectedPlayType = PlayTypes.FirstOrDefault();
        }
        public Dictionary<string, string> Categories { get; set; } = new Dictionary<string, string>() {
            {"全部类型","-1" },
            {"电影", Category.Film.ToString()},
            {"电视剧", Category.TVSeries.ToString()},
            {"美剧", Category.America.ToString()},
            {"韩剧", Category.Korea.ToString()},
            {"综艺", Category.VarietyShow.ToString()},
            {"动漫", Category.Anime.ToString()},
            {"纪录片", Category.Documentary.ToString()},
            {"其他", Category.Other.ToString()}
        };
        public Dictionary<string, string> Areas { get; set; } = new Dictionary<string, string>() {
            {"全部地区","-1" },
            {"大陆", Area.China.ToString()},
            {"香港", Area.Hk.ToString()},
            {"美国", Area.USA.ToString()},
            {"韩国", Area.Korea.ToString()},
            {"日本", Area.Japan.ToString()},
            {"台湾", Area.Taiwan.ToString()},
            {"泰国", Area.Thailand.ToString()},
            {"新加坡", Area.Singapore.ToString()},
            {"马来西亚", Area.Malaysia.ToString()},
            {"印度", Area.India.ToString()},
            {"英国", Area.UK.ToString()},
            {"法国", Area.France.ToString()},
            {"加拿大", Area.Canada.ToString()},
            {"西班牙", Area.Spain.ToString()},
            {"俄罗斯", Area.Russia.ToString()},
            {"其他", Area.Other.ToString()}
        };
        [ObservableProperty]
        private Dictionary<string, string> years;

        public Dictionary<string, string> Sorts { get; set; } = new Dictionary<string, string> {
            {"最新热映","-1" },
            {"最新",Sort.Update.ToString() },
            {"最热",Sort.Like.ToString() },
        };
        public Dictionary<string, string> PlayTypes { get; set; } = new Dictionary<string, string> {
            {"全部","-1" },
            {"VIP",PlayType.VIP_Source.ToString() },
            {"免费",PlayType.Free.ToString() },
        };

        [ObservableProperty]
        private KeyValuePair<string, string> selectedCategory;
        [ObservableProperty]
        private KeyValuePair<string, string> selectedArea;
        [ObservableProperty]
        private KeyValuePair<string, string> selectedYear;
        [ObservableProperty]
        private KeyValuePair<string, string> selectedSort;
        [ObservableProperty]
        private KeyValuePair<string, string> selectedPlayType;


        public SearchViewModel() {

        }
        [ObservableProperty]
        private string keyWords;

        partial void OnKeyWordsChanged(string value) {
            Header = string.Format(l10N.Get("SearchResultTitle"), value);
        }

        [ObservableProperty]
        private ILoadingCollection<SearchItem> loadingItems;
        [ObservableProperty]
        private bool isLoading = true;
        [RelayCommand]
        private void Search() {
            LoadingItems.Reset();
        }

        private async Task<LoadingResultData<SearchItem>?> InnerSearch(int pageIndex) {
            IsLoading = true;
            LoadingResultData<SearchItem> result = null;
            try {
                result = await yiQiKanService?.Search?.GetMovieByOptions(KeyWords, SelectedCategory.Value, "-1", SelectedArea.Value, SelectedYear.Value, SelectedSort.Value, SelectedPlayType.Value, pageIndex, 21, setting.Token);
            } catch {
                app.ShowToast(l10N.Get("GetDataFailed"), 2000);
            }
            IsLoading = false;
            return result;
        }


        [RelayCommand]
        private void ItemClick(VideoItem videoItem) {
            var viewModel = new VideoViewModel(videoItem.movieId, yiQiKanService, setting, app);
            navigationService.NavigateTo(Enums.PageType.VideoPage, true, viewModel);
        }
    }
}
