using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.DBModel {
    public class VideoHistory {
        public string MovieId { get; set; }
        public string TypeName { get; set; }
        public int ItemIndex { get; set; }
        public long Duration { get; set; } = 0;
    }
}
