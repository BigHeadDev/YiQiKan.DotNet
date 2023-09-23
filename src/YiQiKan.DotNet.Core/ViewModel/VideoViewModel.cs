using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Core.Enums;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.Core.UIModels;
using YiQiKan.DotNet.DataBase;
using YiQiKan.DotNet.DataBase.DBContexts;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.Service;
using YiQiKan.DotNet.Utils;

namespace YiQiKan.DotNet.Core.ViewModel {
    /// <summary>
    /// 视频页面的ViewMdeol
    /// 和前面的HomeViewModel、MainViewModel不一样，这两个是全局唯一的
    /// 但是每次点击的视频都会创建一个新的ViewModel，是动态每次new新的
    /// </summary>
    public partial class VideoViewModel : ObservableObject {
        /// <summary>
        /// 构造函数中，需要传入movieId，因为每次点击的视频都不一样
        /// 从轮播图、首页、猜你喜欢、搜索结果等等，都会跳转到视频页面
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="yiQiKanService"></param>
        /// <param name="setting"></param>
        public VideoViewModel(string movieId, YiQiKanService yiQiKanService, ISetting setting, IApp app) {
            this.movieId = movieId;
            this.yiQiKanService = yiQiKanService;
            this.setting = setting;
            this.app = app;
            this.l10N = DependencyInjectionExtensions.ServiceProvider.GetService<ILocalization>();
            this.videoHistoryMananger = DependencyInjectionExtensions.ServiceProvider.GetService<VideoHistoryMananger>();
        }
        public VideoViewModel() {

        }
        //视频id
        private string movieId;
        //一起看服务
        private readonly YiQiKanService yiQiKanService;

        private readonly ILocalization l10N;
        //设置服务
        private readonly ISetting setting;
        private readonly IApp app;
        private readonly VideoHistoryMananger videoHistoryMananger;

        #region 播放器
        public IYiQiKanPlayer yiqikanPlayer;

        [RelayCommand]
        private void LoadPlayer(IYiQiKanPlayer mediaPlayer) {
            if (yiqikanPlayer == null) {//确认是页面跳转进来的才会播放，退出全屏不走
                yiqikanPlayer = mediaPlayer;
                yiqikanPlayer.MediaOpened += MediaPlayer_MediaOpened;
                yiqikanPlayer.IndexChanged += YiqikanPlayer_IndexChanged;
                PlayItemSelected(SelectedPlayList.datalist[PlayIndex]);
            }
        }

        private void YiqikanPlayer_IndexChanged(int index) {
            if (PlayIndex != index && SelectedPlayList == yiqikanPlayer.PlayList) {
                app.UIInvoke(() => {
                    PlayIndex = index;
                });
            }
        }

        [RelayCommand]
        private void UnLoadPlayer() {
            _ = HistoryGenerated(movieId, yiqikanPlayer.PlayList.name, yiqikanPlayer.PlayList.datalist.IndexOf(yiqikanPlayer.CurrentPlayInfo), yiqikanPlayer.CurrentDuration, yiqikanPlayer.TotalDuration);
            if (yiqikanPlayer != null) {
                yiqikanPlayer.MediaOpened -= MediaPlayer_MediaOpened;
                yiqikanPlayer.IndexChanged -= YiqikanPlayer_IndexChanged;
                yiqikanPlayer?.Dispose();
                yiqikanPlayer = null;
            }
        }

        private void MediaPlayer_MediaOpened() {
            yiqikanPlayer.SetPosition(yiqikanPlayer.CurrentPlayInfo.Duration);
        }
        #endregion

        #region 一起看业务
        //初始化状态
        [ObservableProperty]
        private InitState initState;

        //视频详情
        [ObservableProperty]
        private VideoDetailData videoDetail;

        //选中下标
        [ObservableProperty]
        private int playIndex;

        //选中的视频源
        [ObservableProperty]
        private PlayListResource selectedPlayList;

        //是否收藏
        [ObservableProperty]
        public bool isCollected;

        //猜你喜欢
        [ObservableProperty]
        private RecommendationData[] recommendationList;

        //播放器那边变化的时候，这边没有变化!!!!!!!!!

        /// <summary>
        /// 因为有很多视频源，所以在切换视频源的时候，要根据当前选中的某一集，来选中和不选中
        /// </summary>
        /// <param name="oldValue">上一个视频源</param>
        /// <param name="newValue">这一个视频源</param>
        partial void OnSelectedPlayListChanged(PlayListResource? oldValue, PlayListResource newValue) {
            try {
                PlayIndex = newValue.datalist.IndexOf(yiqikanPlayer?.CurrentPlayInfo);
            } catch { }
        }

        /// <summary>
        /// 播放某一集点击，选中一下
        /// </summary>
        /// <param name="playInfo"></param>
        [RelayCommand]
        private void PlayItemSelected(PlayInfo playInfo) {
            if (yiqikanPlayer == null) {
                return;
            }
            if (SelectedPlayList != yiqikanPlayer.PlayList) {
                yiqikanPlayer.SetSource(SelectedPlayList);
            }
            try {
                PlayIndex = yiqikanPlayer.PlayList.datalist.IndexOf(playInfo);
            } catch { }

            yiqikanPlayer.SetIndex(PlayIndex);
        }

        /// <summary>
        /// 初始化命令，初始化视频详情数据
        /// 这里有三个状态，初始化中、初始化成功、初始化失败
        /// 视频详情数据是必须的，如果初始化失败，那么就不能播放视频
        /// 猜你喜欢数据是可选的，如果获取失败，就算了
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task Init() {
            InitState = InitState.Processing;
            try {
                //视频信息
                VideoDetail = await yiQiKanService?.Video?.GetVideoDetail(movieId, setting.DeviceId, setting.Token);
                IsCollected = !string.IsNullOrEmpty(VideoDetail.collectionId);


                foreach (var resource in VideoDetail.resources) {
                    resource.VideoName = VideoDetail.name;
                    foreach (var playInfo in resource.datalist) {
                        playInfo.DecodeAddress = yiQiKanService?.Video?.GetPlayDecodeAddress(playInfo.address, VideoDetail);
                    }
                }
                var selectedResource = VideoDetail.resources[0];

                var history = videoHistoryMananger.GetHistoryByMovieId(movieId);
                if (history != null) {
                    var resource = videoDetail.resources.FirstOrDefault(r => r.name.Equals(history.TypeName));
                    if (resource != null) {
                        selectedResource = resource;
                    }
                }
                SelectedPlayList = selectedResource;

                int index = 0;
                if (history != null) {
                    index = history.ItemIndex;
                    SelectedPlayList.datalist[index].Duration = history.Duration;
                }
                try {
                    PlayIndex = SelectedPlayList.SelectedIndex = index;
                } catch { }
                //PlayItemSelected(SelectedPlayList.datalist[index]);
                InitState = InitState.Success;
            } catch (Exception ex) {
                InitState = InitState.Failed;
            }

            //猜你喜欢
            try {
                RecommendationList = await yiQiKanService?.Video.GetRecommendationListAsync(movieId, "", "", setting.Token);
            } catch (Exception ex) {
                app.ShowToast(l10N.Get("GetGuessYouLikeFailed"), 2000);

            }
        }

        /// <summary>
        /// 点击猜你喜欢的某一个item，触发
        /// </summary>
        /// <param name="recommendationData">点击的item</param>
        [RelayCommand]
        private async Task RecommandItemClick(RecommendationData recommendationData) {
            movieId = recommendationData.movieId;
            await Init();
        }

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task ChangeCollect() {
            if (!setting.IsLogin) {
                await DependencyInjectionExtensions.ServiceProvider.GetService<AccountViewModel>().ShowLoginDialogCommand?.ExecuteAsync(null); ;
            }
            if (setting.IsLogin) {
                try {
                    if (IsCollected) {
                        IsCollected = !(await yiQiKanService.Video.RemoveCollection(new string[] { movieId }, "Movie", setting.Token));
                    } else {
                        VideoDetail.collectionId = await yiQiKanService.Video.AddCollection(movieId, "Movie", setting.Token);
                        IsCollected = !string.IsNullOrEmpty(VideoDetail.collectionId);
                    }
                } catch (Exception ex) {
                    app.ShowToast(l10N.Get("OptionFailed"), 2000);
                }

            } else {
                IsCollected = false;
            }
        }

        /// <summary>
        /// 添加历史
        /// </summary>
        /// <param name="current">当前进度</param>
        /// <param name="total">总进度</param>
        /// <returns></returns>
        private async Task HistoryGenerated(string movieId, string sourceName, int index, long current, long total) {
            var result = await videoHistoryMananger.InsertOrUpdateHistory(new Model.DBModel.VideoHistory {
                Duration = current,
                MovieId = movieId,
                TypeName = sourceName,
                ItemIndex = index
            });
            if (setting.IsLogin) {
                var endTimeStamp = DateTimeHelper.GetTimeStamp();
                var startTimeStamp = endTimeStamp - current;
                try {
                    await yiQiKanService.Video.AddHistory("Movie", yiqikanPlayer.CurrentPlayInfo.name, "General", setting.DeviceId, movieId, startTimeStamp, endTimeStamp, 0, current, current, total, setting.Token);
                } catch { }
            }
        }
        #endregion
    }
}
