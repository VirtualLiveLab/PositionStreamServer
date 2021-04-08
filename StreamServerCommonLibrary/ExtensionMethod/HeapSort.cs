using System;
using System.Collections.Generic;

namespace CommonLibrary.ExtensionMethod
{
    /// <Summary>
    /// ヒープソートをListに追加するための拡張メソッド
    /// </Summary>
    public static class ListExtension
    {
        /// <summary>
        /// ヒープソート
        /// </summary>
        /// <param name="array"></param>
        /// <param name="comparison"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void HeapSort<T>(this List<T> array, Comparison<T> comparison, int? count = null)
        {
            // 必要なヒープ用配列を確保します
            var heap = new T[array.Count];
            int num = 0;

            // ヒープに要素を追加します
            for (var target = 0; target < array.Count; target++)
                Insert(ref num, ref heap, array[target], comparison);

            // ヒープから取り出しながら配列に格納します。
            for (var target = 0; num > 0 && (count != null && target < count); target++)
                array[target] = DeleteGetValue(ref num, ref heap, comparison);
        }

        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="num"></param>
        /// <param name="heap"></param>
        /// <param name="value"></param>
        /// <param name="comparison"></param>
        /// <typeparam name="T"></typeparam>
        public static void Insert<T>(ref int num, ref T[] heap, T value, Comparison<T> comparison)
        {
            heap[num++] = value;
            int i = num, j = i / 2;
            while (i > 1 && comparison(heap[i - 1], heap[j - 1]) < 0)
            {
                var t = heap[i - 1];
                heap[i - 1] = heap[j - 1];
                heap[j - 1] = t;
                i = j;
                j = i / 2;
            }
        }

        /// <summary>
        /// 先頭の要素を取り除き、返す 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="heap"></param>
        /// <param name="comparison"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static T DeleteGetValue<T>(ref int num, ref T[] heap, Comparison<T> comparison)
        {
            var r = heap[0];
            heap[0] = heap[--num];
            int i = 1, j = i * 2;
            while (j <= num)
            {
                if (j + 1 <= num && comparison(heap[j], heap[j - 1]) < 0) j++;
                if (comparison(heap[j - 1], heap[i - 1]) < 0)
                {
                    var t = heap[i - 1];
                    heap[i - 1] = heap[j - 1];
                    heap[j - 1] = t;
                }

                i = j;
                j = i * 2;
            }

            return r;
        }
    }
}