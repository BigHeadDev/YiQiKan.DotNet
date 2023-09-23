using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.RequestModel {
    public class ChangePasswordRequest {
        public string? NewPassword { get; set; }
        public string? OldPassword { get; set; }
    }
}
