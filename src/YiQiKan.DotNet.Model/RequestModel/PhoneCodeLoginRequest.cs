using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.RequestModel {
    public class PhoneCodeLoginRequest {
        public string? areaCode { get; set; }
        public string? areaCodeName { get; set; }
        public string? code { get; set; }
        public string? inviteCode { get; set; }
        public bool isFromSetting { get; set; }
        public string? model { get; set; }
        public string? osVersion { get; set; }
        public string? phone { get; set; }
        public string? type { get; set; }
    }
}
