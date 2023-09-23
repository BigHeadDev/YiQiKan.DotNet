using System;
using System.Collections.Generic;
using System.Text;
using YiQiKan.DotNet.Core.Enums;

namespace YiQiKan.DotNet.Core.Interface {
    public interface ISetting {
        string? Token { get; set; }
        string? DeviceId { get; }
        bool IsLogin { get; }
        ThemeType Theme { get; set; }
    }
}
