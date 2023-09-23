using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;
using YiQiKan.DotNet.Core.Enums;
using YiQiKan.DotNet.Core.Interface;

namespace YiQiKan.DotNet.UWP.AppClasses {
    /// <summary>
    /// UWP平台实现设置
    /// </summary>
    public class UWPSetting : ISetting {
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        //UWP的Token从localSettings对象中读取和存储
        public string? Token { get => localSettings.Values["Token"]?.ToString(); set => localSettings.Values["Token"] = value; }


        //UWP的设备id实现，从EasClientDeviceInformation中读取
        private string? deviceId;
        public string? DeviceId {
            get {
                if (string.IsNullOrEmpty(deviceId)) {
                    var deviceInfo = new EasClientDeviceInformation();
                    deviceId = deviceInfo.Id.ToString().Replace("-", string.Empty);
                }
                return deviceId;
            }
        }

        public bool IsLogin => !string.IsNullOrEmpty(Token);

        public ThemeType Theme {
            get {
                try {
                    if (localSettings.Values["Theme"] != null) {
                        return (ThemeType)Enum.Parse(typeof(ThemeType), localSettings.Values["Theme"].ToString());
                    }
                    localSettings.Values["Theme"] = "System";
                    return ThemeType.System;
                } catch {
                    return ThemeType.System;

                }
            }
            set {
                string newValue = value.ToString();
                if (localSettings.Values["Theme"] != null) {
                    if (!localSettings.Values["Theme"].Equals(newValue)) {
                        localSettings.Values["Theme"] = value.ToString();
                    }
                } else {
                    localSettings.Values["Theme"] = value.ToString();
                }
            }
        }
    }
}
