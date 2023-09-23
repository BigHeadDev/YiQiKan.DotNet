using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Utils {
    public static class DateTimeHelper {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp() {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
        
    }
}
