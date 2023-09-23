using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace YiQiKan.DotNet.UWP.Converters {
    public class ScoreToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return System.Convert.ToDouble(value) > 0 ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
