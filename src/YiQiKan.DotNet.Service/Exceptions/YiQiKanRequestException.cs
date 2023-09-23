using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Service.Exceptions {
    /// <summary>
    /// 只是用来区分一起看服务请求失败的异常
    /// </summary>
    public class YiQiKanRequestException : Exception {
        public YiQiKanRequestException(string resultMsg) : base(resultMsg) {

        }
    }
}
