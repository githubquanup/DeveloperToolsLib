using System.Collections;
using System.Collections.Generic;
using System;

namespace DeveloperToolsLib {
    public static class ArrayExtention {

        /// <summary>Array To List</summary>
        public static List<T> ToList<T>(this Array array) {
            List<T> list = new List<T>(array.Length);
            for (int i = 0; i < array.Length; i++) {
                list.Add((T)array.GetValue(i));
            }
            return list;
        }

        /// <summary>打乱数组元素顺序</summary>
        public static void ShuffleList<T>(List<T> list) {
            for (int i = list.Count; i > 0; i--) {
                int index = UnityEngine.Random.Range(0, i);
                T temp = list[i - 1];
                list[i - 1] = list[index];
                list[index] = temp;
            }
        }

        /// <summary>判断数组是否包含另一个同类型数组</summary>
        public static bool ContainsAll<T>(this List<T> tList, List<T> otherList) {
            if (tList.Count != otherList.Count && tList.Count < otherList.Count)
                return false;

            foreach (var item in otherList) {
                if (!tList.Contains(item)) {
                    return false;
                }
            }
            return true;
        }

        #region 快速排序
        public static void QuickSortStrict(IList<int> data) {
            QuickSortStrict(data, 0, data.Count - 1);
        }

        private static void QuickSortStrict(IList<int> data, int low, int high) {
            if (low >= high) return;
            int temp = data[low];
            int i = low + 1, j = high;
            while (true) {
                while (data[j] > temp) j--;
                while (data[i] < temp && i < j) i++;
                if (i >= j) break;
                Swap(data, i, j);
                i++; j--;
            }
            if (j != low)
                Swap(data, low, j);
            QuickSortStrict(data, j + 1, high);
            QuickSortStrict(data, low, j - 1);
        }
        private static void Swap(IList<int> data, int a, int b) {
            int aValue = data[a];
            int bValue = data[b];
            data[a] = bValue;
            data[b] = aValue;
        }
        #endregion
    }
}