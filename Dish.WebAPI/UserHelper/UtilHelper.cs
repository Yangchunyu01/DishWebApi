using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Dish.WebAPI.UserHelper
{
    public class UtilHelper
    {
        public string AesKey;
        public string AesIV;
        /// <summary>
        /// 获取链接返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetUrltoHtml(string url, string type)
        {
            try
            {
                System.Net.WebRequest Wresq = System.Net.WebRequest.Create(url);
                System.Net.WebResponse Wresp = Wresq.GetResponse();
                System.IO.Stream respStream = Wresp.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding(type)))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }

        public string AESDecrypt(string inputData)
        {
            try
            {
                AesIV = AesIV.Replace(" ", "+");
                AesKey = AesKey.Replace(" ", "+");
                inputData = inputData.Replace(" ", "+");
                byte[] encryptedData = Convert.FromBase64String(inputData);
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Key = Convert.FromBase64String(AesKey);
                rijndaelCipher.IV = Convert.FromBase64String(AesIV);
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                string result = Encoding.UTF8.GetString(plainText);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}