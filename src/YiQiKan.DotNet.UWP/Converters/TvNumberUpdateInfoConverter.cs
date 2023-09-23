using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using YiQiKan.DotNet.Model.ResponseModel;

namespace YiQiKan.DotNet.UWP.Converters {
    internal class TvNumberUpdateInfoConverter:IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var video = (VideoItem)value;
            var tvNumber = video.tvNumber;
            var serializeStatus = video.serializeStatus;
            if (serializeStatus == 0) {
                return "更新至" + tvNumber + "集";
            }
            return "全" + tvNumber + "集";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
