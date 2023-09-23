using LibVLCSharp.Platforms.Windows;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using Windows.UI.Xaml;

namespace YiQiKan.DotNet.UWP.Controls {
    public class AdvanceVlcPlayer : VideoView {
        public AdvanceVlcPlayer()
        {
            Initialized += AdvanceVlcPlayer_Initialized;
            
        }
        private LibVLC LibVLC { get; set; }


        private void AdvanceVlcPlayer_Initialized(object sender, InitializedEventArgs e) {
            LibVLC = new LibVLC(enableDebugLogs: true, e.SwapChainOptions);
            MediaPlayer = new MediaPlayer(LibVLC);
        }



        public Uri Source {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(Uri), typeof(AdvanceVlcPlayer), new PropertyMetadata(default(Uri),OnUriChanged));

        private static void OnUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if(d is AdvanceVlcPlayer vlcPlayer) {
                vlcPlayer.Play();
            }
        }

        private void Play() {
            var media = new Media(LibVLC, Source.AbsoluteUri, FromType.FromLocation);
            SystemMediaTransportControls systemControls = SystemMediaTransportControls.GetForCurrentView();
            systemControls.IsEnabled = true;
            systemControls.IsPlayEnabled = true;
            systemControls.IsPauseEnabled = true;
            systemControls.IsStopEnabled = true;
            systemControls.ButtonPressed += SystemControls_ButtonPressed;
            systemControls.DisplayUpdater.Type = MediaPlaybackType.Video;

            systemControls.DisplayUpdater.VideoProperties.Title = "测试！";
            systemControls.DisplayUpdater.VideoProperties.Subtitle = "子标题";
            systemControls.DisplayUpdater.Update();
            MediaPlayer.Play();
            MediaPlayer.SeekTo(TimeSpan.FromMinutes(10));
        }

        private void SystemControls_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args) {
            switch (args.Button) {
                case SystemMediaTransportControlsButton.Play:
                    MediaPlayer.Play();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    MediaPlayer.Pause();
                    break;
                case SystemMediaTransportControlsButton.Stop:
                    MediaPlayer.Stop();
                    break;
            }
        }
    }
}
