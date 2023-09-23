using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using YiQiKan.DotNet.Model.JsonConverters;

namespace YiQiKan.DotNet.Model.ResponseModel {
    public class VideoDetailData : VideoItem {
        public string classType { get; set; }
        public string area { get; set; }
        public string year { get; set; }
        public string introduction { get; set; }
        public string director { get; set; }
        public string actress { get; set; }
        public bool isUpdateDouBan { get; set; }
        public int watchCount { get; set; }
        public int distinctWatchCount { get; set; }
        [JsonConverter(typeof(StringToActorsConverter))]
        public Actor[] actors { get; set; }
        public string playTipsType { get; set; }
        public string playTips { get; set; }
        public int everydayPlaySurplus { get; set; }
        public int everydayPlayLimit { get; set; }
        public object[] everydayPlayList { get; set; }
        public string shareContent { get; set; }
        public PlayListResource[] resources { get; set; }
        public int expirationTime { get; set; }
        public bool isHavePlayAd { get; set; }
        public string collectionId { get; set; }
    }

    public class PlayListResource {
        public string VideoName { get; set; }
        public string name { get; set; }
        public string typeName { get; set; }
        public List<PlayInfo> datalist { get; set; }
        public int vipPlay { get; set; }
        public int SelectedIndex { get; set; }
    }

    public class PlayInfo {
        public string name { get; set; }
        public string playType { get; set; }
        public string address { get; set; }
        public string action { get; set; }
        public bool isIframePlay { get; set; }
        public Uri? DecodeAddress { get; set; }
        public long Duration { get; set; } = 0;
    }

    public class Actor {
        public string name { get; set; }
        public string role { get; set; }
        public string headImg { get; set; }
    }
}
