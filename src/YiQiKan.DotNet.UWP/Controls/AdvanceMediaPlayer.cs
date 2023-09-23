using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using YiQiKan.DotNet.Core.Interface;
using YiQiKan.DotNet.Core.UIModels;
using YiQiKan.DotNet.Model.ResponseModel;

namespace YiQiKan.DotNet.UWP.Controls {
    public class AdvanceMediaPlayer : MediaPlayerElement, IYiQiKanPlayer {

        public AdvanceMediaPlayer() {
            Loaded += AdvanceMediaPlayer_Loaded;
        }
        public bool RealTimePlaybackEnable {
            get { return (bool)GetValue(RealTimePlaybackEnableProperty); }
            set { SetValue(RealTimePlaybackEnableProperty, value); }
        }
        // Using a DependencyProperty as the backing store for RealTimePlaybackEnable .  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RealTimePlaybackEnableProperty =
            DependencyProperty.Register("RealTimePlaybackEnable", typeof(bool), typeof(AdvanceMediaPlayer), new PropertyMetadata(true));



        private void AdvanceMediaPlayer_Loaded(object sender, RoutedEventArgs e) {
            MediaPlayer.RealTimePlayback = RealTimePlaybackEnable;
            MediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
        }

        private void MediaPlayer_MediaOpened(MediaPlayer sender, object args) {
            MediaOpened?.Invoke();
        }

        public void Play() {
            MediaPlayer?.Play();
        }

        public void Pause() {
            MediaPlayer?.Pause();
        }

        public void SetSource(PlayListResource playList) {
            this.playList = playList;
            if (Source is MediaPlaybackList lastMediaPlaybackList) {
                lastMediaPlaybackList.CurrentItemChanged -= LastMediaPlaybackList_CurrentItemChanged;
            }
            MediaPlaybackList mediaPlaybackList = new MediaPlaybackList();
            foreach (var item in playList.datalist) {
                var playItem = new MediaPlaybackItem(MediaSource.CreateFromUri(item.DecodeAddress));
                var props = playItem.GetDisplayProperties();
                props.Type = Windows.Media.MediaPlaybackType.Video;
                props.VideoProperties.Title = playList.VideoName;
                props.VideoProperties.Subtitle = string.Format("{0} - {1}", playList.name, item.name);
                playItem.ApplyDisplayProperties(props);
                mediaPlaybackList.Items.Add(playItem);
            }
            mediaPlaybackList.StartingItem = mediaPlaybackList.Items[playList.SelectedIndex];
            Source = mediaPlaybackList;
            mediaPlaybackList.CurrentItemChanged += LastMediaPlaybackList_CurrentItemChanged;
        }

        private void LastMediaPlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args) {
            var index = (int)sender.CurrentItemIndex;
            currentPlayInfo = playList?.datalist?[index];
            IndexChanged?.Invoke(index);
        }

        public void SetIndex(int index) {
            currentPlayInfo = playList?.datalist?[index];
            if (Source is MediaPlaybackList playbackList) {
                playbackList.MoveTo((uint)index);
            }
        }

        public void Dispose() {
            MediaPlayer?.Dispose();
        }

        public async void SetPosition(long position) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                MediaPlayer.PlaybackSession.Position = TimeSpan.FromMilliseconds(position);
            });
        }

        public long TotalDuration => (long)MediaPlayer.PlaybackSession.NaturalDuration.TotalMilliseconds;

        public long CurrentDuration => (long)MediaPlayer.PlaybackSession.Position.TotalMilliseconds;

        private PlayListResource playList;
        public PlayListResource PlayList => playList;
        private PlayInfo currentPlayInfo;
        public PlayInfo CurrentPlayInfo => currentPlayInfo;

        public event Action<int> OnIndexChanged;
        public event Action MediaOpened;
        public event Action OnPrevious;
        public event Action OnNext;
        public event Action<int> IndexChanged;
    }
}
