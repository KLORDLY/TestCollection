using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace SymmetricCryptoHelperTest
{
    //对称加密帮助类
    public class SymmetricCryptoHelper
    {
        //对称加密算法提供器
        private ICryptoTransform encryptor; //加密器对象
        private ICryptoTransform decryptor; //解密器对象
        private const int BufferSize = 1024;

        public SymmetricCryptoHelper(string algorithmName, byte[] key)
        {
            SymmetricAlgorithm provider = SymmetricAlgorithm.Create(algorithmName);
            provider.Key = key;
            provider.IV = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            encryptor = provider.CreateEncryptor();
            decryptor = provider.CreateDecryptor();
        }

        public SymmetricCryptoHelper(byte[] key)
            : this("TripleDES", key)
        { }

        //加密算法
        public string Encrypt(string clearText)
        {
            //创建明文流
            byte[] clearBuffer = Encoding.UTF8.GetBytes(clearText);
            MemoryStream clearStream = new MemoryStream(clearBuffer);

            //创建密文流
            MemoryStream encryptedStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write);

            //将明文流写入buffer中
            //将buffer中的数据写入到cryptoStream中
            int bytesRead = 0;
            byte[] buffer = new byte[BufferSize];
            while (true)
            {
                bytesRead = clearStream.Read(buffer, 0, BufferSize);
                if (bytesRead <= 0)
                {
                    break;
                }
                cryptoStream.Write(buffer, 0, bytesRead);
            }
            cryptoStream.FlushFinalBlock();

            //获取加密后的文本
            buffer = encryptedStream.ToArray();
            string encryptedText = Convert.ToBase64String(buffer);
            return encryptedText;
        }

        //解密算法
        public string Decrypt(string encryptedText)
        {
            byte[] encryptedBuffer = Convert.FromBase64String(encryptedText);
            MemoryStream encryptedStream = new MemoryStream(encryptedBuffer);

            MemoryStream clearStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(encryptedStream, decryptor, CryptoStreamMode.Read);

            int bytesRead = 0;
            byte[] buffer = new byte[BufferSize];
            while (true)
            {
                bytesRead = cryptoStream.Read(buffer, 0, BufferSize);
                if (bytesRead <= 0)
                {
                    break;
                }
                clearStream.Write(buffer, 0, bytesRead);
            }
            buffer = clearStream.GetBuffer();
            string clearText = Encoding.UTF8.GetString(buffer, 0, (int)clearStream.Length);
            return clearText;
        }

        public static string Encrypt(string clearText, string key)
        {
            byte[] keyData = new byte[16];
            byte[] sourceData = Encoding.Default.GetBytes(key);
            int copyBytes = 16;
            if (sourceData.Length< 16)
                copyBytes = sourceData.Length;
            Array.Copy(sourceData, keyData, copyBytes);
            SymmetricCryptoHelper helper = new SymmetricCryptoHelper(keyData);
            return helper.Encrypt(clearText);
        }

        public static string Decrypt(string encryptedText,string key)
        {
            byte[] keyData = new byte[16];
            byte[] sourceData = Encoding.Default.GetBytes(key);
            int copyBytes = 16;
            if (sourceData.Length < 16)
                copyBytes = sourceData.Length;
            Array.Copy(sourceData, keyData, copyBytes);
            SymmetricCryptoHelper helper = new SymmetricCryptoHelper(keyData);
            return helper.Decrypt(encryptedText);
        }
    }
}
