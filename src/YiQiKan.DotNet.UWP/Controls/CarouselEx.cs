using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using YiQiKan.DotNet.UWP.Args;

namespace YiQiKan.DotNet.UWP.Controls {
    public class CarouselEx : Carousel {
        DispatcherTimer timer = new DispatcherTimer();
        public CarouselEx() {
            Loaded += CarouselEx_Loaded;
            GettingFocus += CarouselEx_GettingFocus;
        }

        public event CarouselItemClickEventHandler ItemClick;

        protected override void OnTapped(TappedRoutedEventArgs e) {
            var carouselItem = this.ContainerFromIndex(SelectedIndex) as CarouselItem;
            if (carouselItem != null) {
                // 获取点击位置相对于 CarouselItem 的坐标
                var renderSize = carouselItem.RenderSize;
                var relativePoint = e.GetPosition(carouselItem);
                bool isInside = relativePoint.X >= 0 && relativePoint.X <= renderSize.Width && relativePoint.Y >= 0 && relativePoint.Y <= renderSize.Height;
                if (isInside) {
                    var args = new CarouselItemClickEventArgs() {
                        ClickedItem = carouselItem.Content
                    };
                    ItemClick?.Invoke(carouselItem, args);
                }
            }
        }
        private void CarouselEx_GettingFocus(UIElement sender, GettingFocusEventArgs args) {
            args.TryCancel();//取消焦点
        }

        private void CarouselEx_Loaded(object sender, RoutedEventArgs e) {
            timer.Tick += Timer_Tick;
        }
        private int offset = 1;
        private void Timer_Tick(object sender, object e) {
            if (SelectedIndex == Items.Count - 1) {
                offset = -1;
            } else if (SelectedIndex == 0) {
                offset = 1;
            }
            SelectedIndex += offset;
        }

        protected override void OnItemsChanged(object e) {
            base.OnItemsChanged(e);
            if (Items.Count > 0) {
                if (AutoSlide) {
                    timer.Interval = TimeSpan.FromSeconds(SlideIntervalSecond);
                    timer.Start();
                }
            }

        }


        public double SlideIntervalSecond {
            get { return (double)GetValue(SlideIntervalSecondProperty); }
            set { SetValue(SlideIntervalSecondProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SlidesIntervalSecond.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SlideIntervalSecondProperty =
            DependencyProperty.Register("SlideIntervalSecond", typeof(double), typeof(CarouselEx), new PropertyMetadata(3, OnSliderIntervalSecondChanged));

        private static void OnSliderIntervalSecondChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is CarouselEx carousel) {
                carousel.ChangeSliderIntervalSecond();
            }
        }

        private void ChangeSliderIntervalSecond() {
            timer.Interval = TimeSpan.FromSeconds(SlideIntervalSecond);
        }

        public bool AutoSlide {
            get { return (bool)GetValue(AutoSlideProperty); }
            set { SetValue(AutoSlideProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AutoSlide.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoSlideProperty =
            DependencyProperty.Register("AutoSlide", typeof(bool), typeof(CarouselEx), new PropertyMetadata(false, OnAutoSlideChanged));

        private static void OnAutoSlideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is CarouselEx carousel) {
                carousel.ChangeAutoSlide();
            }
        }

        private void ChangeAutoSlide() {
            if (Items.Count > 0) {
                if (AutoSlide) {
                    if (!timer.IsEnabled) {
                        timer.Start();
                    }
                } else {
                    if (timer.IsEnabled) {
                        timer.Stop();
                    }
                }
            }
        }
    }
}
