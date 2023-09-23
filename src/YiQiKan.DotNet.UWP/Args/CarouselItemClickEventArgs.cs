using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace YiQiKan.DotNet.UWP.Args {
    public class CarouselItemClickEventArgs : RoutedEventArgs {
        public object ClickedItem { get; set; }
    }
    public delegate void CarouselItemClickEventHandler(object sender, CarouselItemClickEventArgs e);

}
