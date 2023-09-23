using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.RequestModel {
    public class CollectionRequest {
        public int isSchedule { get; set; }
        public string targetId { get; set; }
        public string type { get; set; }
    }
}
