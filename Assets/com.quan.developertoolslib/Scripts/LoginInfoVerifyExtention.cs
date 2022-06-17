using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeveloperToolsLib {
    public static class LoginInfoVerify {

        /// <summary>
        /// 身份证号格式验证
        /// </summary>
        public static bool VerifyIDCard(string idcardInput) {
            //省份代码
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (idcardInput.Length == 18) {
                if (!long.TryParse(idcardInput.Remove(17), out long idcardNum) || idcardNum < Math.Pow(10, 16)) {
                    Debug.Log("身份证号格式填写错误，请检查！");
                    return false;
                };

                if (address.IndexOf(idcardInput.Substring(0, 2)) == -1) {
                    Debug.LogError("身份证号中第一二位的省份代码填写错误，请检查！");
                    return false;
                }

                string birth = idcardInput.Substring(6, 8).Insert(6, "-").Insert(4, "-");
                DateTime time = new DateTime();
                if (!DateTime.TryParse(birth, out time)) {
                    Debug.LogError("身份证号中的出身日期填写错误，请检查！");
                    return false;
                }

                //最后一位数的校验码
                string[] arrVerifyCode = { "1", "0", "x", "9", "8", "7", "6", "5", "4", "3", "2" };
                //身份证各位乘以的系数
                string[] Wi = { "7", "9", "10", "5", "8", "4", "2", "1", "6", "3", "7", "9", "10", "5", "8", "4", "2" };
                char[] Ai = idcardInput.Remove(17).ToCharArray();
                int sum = 0;
                for (int i = 0; i < 17; i++) {
                    sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
                }

                Math.DivRem(sum, 11, out int yu);
                if (arrVerifyCode[yu] != idcardInput.Substring(17, 1).ToLower()) {
                    Debug.LogError("身份证号最后一位格式填写错误，请检查！");
                    return false;
                }

                return true;
            } else if (idcardInput.Length == 15) {
                if (!long.TryParse(idcardInput, out long idcardNum) || idcardNum < Math.Pow(10, 14)) {
                    Debug.Log("身份证号格式填写错误，请检查！");
                    return false;
                };

                if (address.IndexOf(idcardInput.Substring(0, 2)) == -1) {
                    Debug.LogError("身份证号中第一二位的省份代码填写错误，请检查！");
                    return false;
                }

                string birth = idcardInput.Substring(6, 6).Insert(4, "-").Insert(2, "-");
                DateTime time = new DateTime();
                if (!DateTime.TryParse(birth, out time)) {
                    Debug.LogError("身份证号中的出身日期填写错误，请检查！");
                    return false;
                }
                return true;
            } else {
                Debug.LogError("身份证号码为18位或者15位，请检查！");
                return false;
            }

        }

        /// <summary>
        /// 手机号信息验证
        /// </summary>
        public static bool VerifyPhoneNum(string phoneNumInput) {
            if (phoneNumInput.Length != 11) {
                Debug.LogError("手机号为11位，请检查！");
                return false;
            }
            return true;
        }

    }
}