using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace YiQiKan.DotNet.Utils {
    /// <summary>
    /// 说明：此类专门做加密和解密
    /// 
    /// Q1：为什么有这个类，是用来做啥的？
    /// A1：安卓app中，很多数据都做了加密处理，就像是在和服务的进行密文通信
    /// 通过破解安卓app，查看源代码之后，发现了一些加密和解密的流程，用C#实现了
    /// 
    /// Q2：哪里用到了这些加密解密？
    /// A2：保存手机登录状态接口，进行了客户端唯一标识和请求签名生成，这里用到了一些加密
    /// 视频播放地址获取接口，服务端返回了一串 “密文”，根本不能直接播放，需要进行一些解码，最终拿到播放地址
    /// 等等等等。。
    /// 
    /// </summary>
    public static class EncryptHelper {
        /// <summary>
        /// 把一个字符串通过MD5加密
        /// </summary>
        /// <param name="src">原字符串</param>
        /// <returns>md5加密字符串</returns>
        public static string EncryptByMD5(string src) {
            using (var md5 = MD5.Create()) {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(src));
                return bytes.BytesToString();
            }
        }

        #region Others
        public static string BytesToString(this byte[] bytes) {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
        public static string GetTrimString(this string str) {
            if (string.IsNullOrEmpty(str)) {
                return "0";
            }
            return str.Trim();
        }
        #endregion

        #region Base64
        public static byte[] GetBase64ConvertBytes(this string base64Str) {
            return Convert.FromBase64String(base64Str);
        }
        public static string GetBase64ConvertString(this byte[] bytes) {
            return Convert.ToBase64String(bytes);
        }
        #endregion

        #region UTF-8
        public static string GetUTF8String(this byte[] bytes) {
            return Encoding.UTF8.GetString(bytes);
        }
        public static byte[] GetUTF8Bytes(this string str) {
            return Encoding.UTF8.GetBytes(str);
        }
        #endregion

        #region Sha256
        public static byte[] GetSha256Data(this string content) {
            return GetSha256Data(content.GetUTF8Bytes());
        }
        public static byte[] GetSha256Data(this byte[] bytes) {
            return SHA256.Create().ComputeHash(bytes);
        }
        #endregion

        #region AES
        public static byte[] AESDecrypt(string content, string key, string iv, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7) {
            return AESDecrypt(content, key.GetUTF8Bytes(), iv.GetUTF8Bytes(), cipherMode);
        }

        public static byte[] AesEecrypt(string content, string key, string iv, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7) {
            return AesEecrypt(content, key.GetUTF8Bytes(), iv.GetUTF8Bytes(), cipherMode, paddingMode);
        }

        public static byte[] AESDecrypt(string content, byte[] key, byte[] iv, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7) {
            byte[] toEncryptArray = Convert.FromBase64String(content);

            SymmetricAlgorithm des = Aes.Create();
            des.Key = key;
            des.Mode = cipherMode;
            des.Padding = paddingMode;
            des.IV = iv;

            var desc = des.CreateDecryptor();
            byte[] resultArray = desc.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return resultArray;
        }

        public static byte[] AesEecrypt(string content, byte[] key, byte[] iv, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7) {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(content);

            SymmetricAlgorithm des = Aes.Create();
            des.Key = key;
            des.Mode = cipherMode;
            des.Padding = paddingMode;
            des.IV = iv;

            var desc = des.CreateEncryptor();
            byte[] resultArray = desc.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return resultArray;
        }
        #endregion
    }
}
