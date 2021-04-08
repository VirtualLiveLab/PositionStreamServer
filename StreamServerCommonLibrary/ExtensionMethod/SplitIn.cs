using System;
using System.Collections.Generic;

namespace CommonLibrary.ExtensionMethod
{
    public static class SplitInExtension
    {
        public static IEnumerable<List<T>> SplitInto<T>(this List<T> list, int num)
        {
            var count = list.Count;
            var per = count / num;
            var amari = count - (count / num) * num;
            for (int i = 0; i < num; i++)
            {
                yield return list.GetRange(i * per + Math.Min(i, amari), per + (i < amari ? 1 : 0));
            }
        }
    }
}