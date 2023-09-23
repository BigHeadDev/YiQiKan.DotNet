using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace YiQiKan.DotNet.UWP.Converters {
    internal class AutoSuggestBoxQuerySubmittedEventArgsConverter:IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var args = value as AutoSuggestBoxQuerySubmittedEventArgs;
            return args.QueryText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
