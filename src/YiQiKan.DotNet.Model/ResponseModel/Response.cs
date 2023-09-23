using System.Text.Json.Serialization;

namespace YiQiKan.DotNet.Model.ResponseModel {
    /// <summary>
    /// 说明：此类为响应包装类
    /// 所有的响应结果，都有最外层相同的数据结构，Data为不同的结果，所以用Response类来描述了响应的主体数据结构，而Data属性是用泛型的方式传递不同的类型组合成新的数据结构
    /// </summary>
    /// <typeparam name="T">Data类</typeparam>
    public class Response<T> {
        public string ResultMsg { get; set; }
        public int ResultCode { get; set; }
        [JsonPropertyName("currentTime")]//json中为currentTime
        public long TimeStamp { get; set; }
        [JsonPropertyName("data")]//json中为currentTime
        public T Data { get; set; }
    }
}
