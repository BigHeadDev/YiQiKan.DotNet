using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.RequestModel {
    public class SavePhoneLoginLogRequest {
        public string? AgentCode { get; set; }
        public string? OnlyIdentificationm { get; set; }
        public long Ts { get; set; }
        public string? Sign { get; set; }
    }
}
