using System;
using System.IO;
using System.Text;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using UnityEngine;
using LitJson;

namespace DeveloperToolsLib{
    public static class EncryptLogin{
        
        #region 使用电脑MAC地址  加密
        public struct EncryJson
        {
            public string key;
            public string value;
        }
        public static string DataKey = "$a1weascfasdfasfdgs;fiogjspofig3";
        private static string path;

        /// <summary>加密登录</summary>
		public static void TryLoginWithEncrypt()
        {
            path = Application.dataPath + "/ShaderCacheMono.db";
            string tempStr = Application.dataPath + "/ShaderCacheMono.txt";

            //QUIT
            try
            {
                //替换文件后缀
                File.Move(path, tempStr);
            }
            catch (Exception e)
            {
                //Debug.Log(e);
                Application.Quit();
            }

            //读取文件内容
            string sssstr = File.ReadAllText(tempStr);
            bool v = string.IsNullOrEmpty(sssstr);

            if (v)
            {
                //写入
                EncryJson json = new EncryJson();
                json.key = "key";
                json.value = GetMacAddress();
                string str = JsonMapper.ToJson(json);
                //把加密的数据写入
                string encryptStr = RijndaelEncrypt(str, DataKey);

                FileStream file = new FileStream(tempStr, FileMode.OpenOrCreate);
                byte[] charArray = Encoding.UTF8.GetBytes(encryptStr);
                file.Write(charArray, 0, charArray.Length);
                //释放资源
                file.Flush();
                file.Close();
                file.Dispose();

            }
            else
            {
                //解密
                EncryJson encryJson = JsonMapper.ToObject<EncryJson>(RijndaelDecrypt(sssstr, DataKey));
                if (!encryJson.value.Equals(GetMacAddress()))
                    Application.Quit();
            }
            //替换后缀
            File.Move(tempStr, path);
        }
        /// <summary>获取电脑MAC地址</summary>
		private static string GetMacAddress()
        {
            string physicalAddress = "";
            NetworkInterface[] nice = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adaper in nice)
            {
                // Debug.Log(adaper.Description);
                if (adaper.Description == "en0")
                {
                    physicalAddress = adaper.GetPhysicalAddress().ToString();
                    break;
                }
                else
                {
                    physicalAddress = adaper.GetPhysicalAddress().ToString();
                    if (physicalAddress != "")
                    {
                        break;
                    };
                }
            }
            return physicalAddress;
        }
        /// <summary>加密</summary>
        public static string RijndaelEncrypt(string pString, string pKey)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(pString);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>解密</summary>
        public static String RijndaelDecrypt(string pString, string pKey)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey);
            byte[] toEncryptArray = Convert.FromBase64String(pString);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        #endregion
    }
}