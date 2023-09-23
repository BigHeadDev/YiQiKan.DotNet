using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using YiQiKan.DotNet.Model.ResponseModel;

namespace YiQiKan.DotNet.UWP.Converters {
    internal class ItemClickEventArgsConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var args = value as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            return args.ClickedItem;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
