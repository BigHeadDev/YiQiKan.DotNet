using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.RequestModel {

    public class SendPhoneCodeRequest {
        public string? areaCode { get; set; }
        public string? areaCodeName { get; set; }
        public string? code { get; set; }
        public bool isFromSetting { get; set; }
        public string? phone { get; set; }
        public string? type { get; set; }
    }
}
