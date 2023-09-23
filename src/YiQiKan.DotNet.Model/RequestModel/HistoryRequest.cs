using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.RequestModel {
    public class HistoryRequest {
        public long duration { get; set; }
        public long durationTime { get; set; }
        public long endTime { get; set; }
        public string historyType { get; set; }
        public int isSchedule { get; set; }
        public string moviePlayType { get; set; }
        public string onlyIdentificationm { get; set; }
        public long playEndTime { get; set; }
        public string playItem { get; set; }
        public long playStartTime { get; set; }
        public string sign { get; set; }
        public long startTime { get; set; }
        public string targetId { get; set; }
        public long ts { get; set; }
    }
    //{"duration":2764980,"durationTime":8971,"endTime":1692341049840,string"historyType":"Movie","isSchedule":0,"moviePlayType":"General","onlyIdentificationm":"b61026e8aeb84c10b0096d120922054a","playEndTime":42824,"playItem":"第01集","playStartTime":36656,"sign":"8712b76a6d1346867a35479b1f70ba5fb1864cab414124fa11bed2f94fd7da9a","startTime":1692341036809,"targetId":"63c2c5676a582a76ce01c90e","ts":1692341053917}
}
