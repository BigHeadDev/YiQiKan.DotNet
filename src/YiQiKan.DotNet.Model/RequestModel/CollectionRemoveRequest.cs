using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.RequestModel {
    public class CollectionRemoveRequest {
        public string[] targetId { get; set; }
        public string type { get; set; }
    }
}
