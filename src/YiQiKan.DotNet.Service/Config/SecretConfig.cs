using System;
using System.Collections.Generic;
using System.Text;

namespace YiQiKan.DotNet.Service.Config {
    public static class SecretConfig {
        private static readonly string commonIV = "A-16-Byte-String";
        private static readonly string commonKey = "9JFPeqizqMhOGX1t";
        private static readonly string common2Key = "OsZlj2Vs95MXyA4J";

        private static readonly string playAddressSecretKey = "I6KIv8yu6nA68+UQAGd8XQ==";
        private static readonly string playSignKey = "f028f684404417b4";
        private static readonly string playSignIV = "ABCDEFH123456789";
        public static string PlayAddressSecretKey => playAddressSecretKey;
        public static string CommonIV => commonIV;
        public static string Common2Key => common2Key;
        public static string CommonKey => commonKey;
        public static string PlaySignKey => playSignKey;
        public static string PlaySignIV => playSignIV;
    }
}
