using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.ResponseModel {
    public class VideoItem {
        public string movieId { get;set; }
        public string pic { get; set; }
        public string picJuHe { get; set; }
        public string name { get; set; }
        public int tvNumber { get; set; }
        public int serializeStatus { get; set; }
        public float score { get; set; }
        public string category { get; set; }
        public long modifyTime { get; set; }
        public bool isMovie { get; set; }
        public bool updateTime { get; set; }
    }
}
