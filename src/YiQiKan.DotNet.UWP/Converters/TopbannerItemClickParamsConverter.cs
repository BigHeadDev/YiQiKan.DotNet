using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using YiQiKan.DotNet.Model.ResponseModel;
using YiQiKan.DotNet.UWP.Args;

namespace YiQiKan.DotNet.UWP.Converters {
    internal class TopbannerItemClickParamsConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var args = value as CarouselItemClickEventArgs;
            var item = args.ClickedItem as Topbanner;
            return item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
