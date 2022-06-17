using System;
using System.Collections;
using System.Collections.Generic;

namespace DeveloperToolsLib{

    public class EnumTool<T> where T : Enum{
        public static int GetValue(string name){
            try
            {
                if(Enum.Parse(typeof(T), name) != null)
                    return (int)Enum.Parse(typeof(T), name);
                return 0;
            }
            catch (System.Exception)
            {
                return 0;
            }
        }
        public static string GetKey(object value){
            try
            {
                if(value != null && !string.IsNullOrWhiteSpace(value.ToString()) && value.ToString() != "0"){
                    return Enum.Parse(typeof(T),value.ToString()).ToString();
                }
                return "";
            }
            catch (System.Exception)
            {
                return "";
            }
        }
        public static T GetT(object value){
            T result = default(T);

            if(value != null && !string.IsNullOrWhiteSpace(value.ToString()) && value.ToString() != "0"){
                result =(T) (object)Enum.Parse(typeof(T),value.ToString());
            }

            return result;
        }
    }
}