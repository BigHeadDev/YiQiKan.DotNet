using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace YiQiKan.DotNet.UWP.Converters {
    internal class HomePivotSelectionChangedParamsConverter:IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var args = (SelectionChangedEventArgs)value;
            return args.AddedItems.FirstOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
