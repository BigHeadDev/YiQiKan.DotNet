using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.ResponseModel {
    public class UserInfoData {
        public int userId { get; set; }
        public string? nickname { get; set; }
        public string? userHead { get; set; }
        public string? areaCode { get; set; }
        public string? phone { get; set; }
        public int points { get; set; }
        public string? userLevelType { get; set; }
        public int level { get; set; }
        public string? levelName { get; set; }
        public int everydayPlayLimit { get; set; }
        public int everydayPlaySurplus { get; set; }
        public object[] everydayPlayList { get; set; }
        public int sex { get; set; }
        public string? inviteCode { get; set; }
        public int inviteCount { get; set; }
        public int inviteLevelCount { get; set; }
        public string? shareLink { get; set; }
        public string? shareContent { get; set; }
        public string? agentCode { get; set; }
    }
}
