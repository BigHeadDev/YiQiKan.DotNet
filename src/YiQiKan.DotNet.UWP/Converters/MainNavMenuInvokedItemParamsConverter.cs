using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace YiQiKan.DotNet.UWP.Converters {
    public class MainNavMenuInvokedItemParamsConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var args = (NavigationViewItemInvokedEventArgs)value;
            var item = (NavigationViewItem)args.InvokedItemContainer;
            return item.Tag.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
