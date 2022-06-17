using System.Collections;
using System.Collections.Generic;

namespace DeveloperToolsLib {
    public static class DictionaryExtension {
        /// <summary>
        /// 尝试根据key得到value，得到了的话直接返回value，没有得到直接返回null
        /// this Dictionary<Tkey,Tvalue> dict 这个字典表示我们要获取值的字典
        /// </summary>
        public static Tvalue TryGet<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key) {
            Tvalue value;
            dict.TryGetValue(key, out value);
            return value;
        }
        /// <summary>
        /// 尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
        /// </summary>
        public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value) {
            if (dict.ContainsKey(key) == false) dict.Add(key, value);
            return dict;
        }
        /// <summary>
        /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        public static Dictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value) {
            dict[key] = value;
            return dict;
        }

    }

}