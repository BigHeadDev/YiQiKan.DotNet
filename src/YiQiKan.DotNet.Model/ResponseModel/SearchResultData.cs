using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Model.ResponseModel {

    public class LoadingResultData<T> where T : VideoItem {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int totalPage { get; set; }
        public int totalCount { get; set; }
        public T[] items { get; set; }
    }

    public class SearchItem : VideoItem {
        public object starLevel { get; set; }
        public string sourceAddress { get; set; }
        public string director { get; set; }
        public string actress { get; set; }
        public string year { get; set; }
        public string area { get; set; }
        public int heat { get; set; }
        public long releaseTime { get; set; }
        public bool isFavorite { get; set; }
        public string movieCategory { get; set; }
        public bool isIframePlay { get; set; }
    }

}
