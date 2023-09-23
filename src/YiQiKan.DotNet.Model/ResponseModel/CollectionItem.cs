using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.ResponseModel {
    public class CollectionItem : VideoItem {
        public long createTime { get; set; }
        public string id { get; set; }
        public string image { set { pic = value; } }
        public int isSchedule { get; set; }
        public string movieCategory { get; set; }
        public string targetId { set { movieId = value; } }
        public string movieArea { get; set; }

    }
}
