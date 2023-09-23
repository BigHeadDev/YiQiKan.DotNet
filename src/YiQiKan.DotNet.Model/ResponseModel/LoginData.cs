using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.ResponseModel {
    public class LoginData {
        public User user { get; set; }
        public Oauth oAuth { get; set; }
    }

    public class User {
        public int userId { get; set; }
        public string nickname { get; set; }
        public string userHead { get; set; }
        public string areaCode { get; set; }
        public string phone { get; set; }
        public int sex { get; set; }
        public string agentCode { get; set; }
        public string inviteCode { get; set; }
        public int isFirstLogin { get; set; }
        public int level { get; set; }
    }

    public class Oauth {
        public string token { get; set; }
        public int expiresIn { get; set; }
    }

}
