using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;

namespace YiQiKan.DotNet.UWP.Converters {
    public class StreamToBitmapSourceConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var stream = value as Stream;
            if (stream != null) {
                var bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                bitmapImage.SetSource(stream.AsRandomAccessStream());
                return bitmapImage;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
