using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.ResponseModel {
    public class HotSearchItem {
        public string? title { get; set; }
        public HotKeywords[] list { get; set; }
    }
    public class HotKeywords {
        public string? name { get; set; }
    }
}
