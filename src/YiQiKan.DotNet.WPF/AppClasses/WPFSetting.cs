using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.Core.Interface;

namespace YiQiKan.DotNet.WPF.AppClasses {
    public class WPFSetting : ISetting {
        public string? Token { get => YiQiKan_DotNet.Default.Token; set => YiQiKan_DotNet.Default.Token = value; }

        public string? DeviceId => "";
    }
}
