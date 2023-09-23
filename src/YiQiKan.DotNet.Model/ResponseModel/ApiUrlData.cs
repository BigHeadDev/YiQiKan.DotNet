using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.ResponseModel {
    public class ApiUrlData {
        public List<ApiUrl> ApiUrlList { get; set; }

        public class ApiUrl {
            public string Url { get; set; }
            public string CheckType { get; set; }
            public int IsMustPass { get; set; }
        }
    }
}
