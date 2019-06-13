using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RSACryptoHelperTest
{
    public class RSACryptoHelper
    {
        /// <summary>
        /// 在非对称加密的加密模式
        /// 加密方法
        /// </summary>
        /// <param name="publicKeyXml"></param>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Encrypt(string publicKeyXml, string plainText)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(publicKeyXml);  //使用公钥初始化对象
            byte[] plainData = Encoding.Default.GetBytes(plainText);
            byte[] encryptedData = provider.Encrypt(plainData, true);
            return Convert.ToBase64String(encryptedData);
        }
        /// <summary>
        /// 在非对称加密的加密模式
        /// 解密方法
        /// </summary>
        /// <param name="privateKeyXml"></param>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        public static string Decrypt(string privateKeyXml, string encryptedText)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(privateKeyXml);  //使用私钥初始化对象
            byte[] encryptedData = Convert.FromBase64String(encryptedText);
            byte[] plainData = provider.Decrypt(encryptedData, true);
            return Encoding.Default.GetString(plainData);
        }
        /// <summary>
        /// 数字签名优化后的非对称加密的认证模式
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="privateKeyXml"></param>
        /// <returns>
        /// 摘要
        /// </returns>
        public static string SignData(string plainText, string privateKeyXml)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(privateKeyXml);

            byte[] plainData = Encoding.Default.GetBytes(plainText);
            //设置获取摘要的算法
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA1");
            //获取签名过的摘要
            byte[] signedDigest = provider.SignData(plainData, sha1);
            return Convert.ToBase64String(signedDigest);
        }
        /// <summary>
        /// 数字签名优化后的非对称加密的认证模式
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="signature"></param>
        /// <param name="publicKeyXml"></param>
        /// <returns>
        /// 原始摘要和本地摘要对比后的结果
        /// </returns>
        public static bool VerifyData(string plainText, string signature, string publicKeyXml)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(publicKeyXml);

            byte[] plainData = Encoding.Default.GetBytes(plainText);
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA1");
            byte[] signedDigest = Convert.FromBase64String(signature);
            return provider.VerifyData(plainData, sha1, signedDigest);
        }
    }
}
