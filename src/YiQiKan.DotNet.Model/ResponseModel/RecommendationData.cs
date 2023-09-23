using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.ResponseModel {
    public class RecommendationData {
        public string movieId { get; set; }
        public string pic { get; set; }
        public string name { get; set; }
        public int tvNumber { get; set; }
        public string sourceAddress { get; set; }
        public long modifyTime { get; set; }
        public string category { get; set; }
        public bool movie { get; set; }
    }
}
