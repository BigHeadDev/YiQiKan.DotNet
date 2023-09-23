using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace YiQiKan.DotNet.Model.ResponseModel {
    public class HistoryItem : VideoItem {
        public long createTime { get; set; }
        public long durationTime { get; set; }
        public string historyType { get; set; }
        public string id { get; set; }
        public string image { set { pic = value; } }
        public int isSchedule { get; set; }
        public int userId { get; set; }
        public long playEndTime { get; set; }
        public long playStartTime { get; set; }
        public string targetId { set { movieId = value; } }
        public string movieCategory { get; set; }
    }
}
