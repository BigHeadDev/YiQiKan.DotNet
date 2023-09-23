using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.RequestModel {
    public class AccountPasswordLoginRequest {
        public AccountPasswordLoginRequest(string account, string password) {
            username = phone = account;
            this.passWord = password;
        }

        public string? areaCode { get; set; } = "86";
        public string? code { get; set; } = string.Empty;
        public int latitude { get; set; } = 0;
        public int longitude { get; set; } = 0;
        public string? model { get; set; } = Environment.MachineName;
        public string? osVersion { get; set; }
        public string? passWord { get; set; }
        public string? phone { get; set; }
        public string? pushPackageName { get; set; }
        public string? username { get; set; }
    }

}
