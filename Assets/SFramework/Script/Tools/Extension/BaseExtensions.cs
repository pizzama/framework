using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace SFramework.Extension
{
    /// <summary>
    /// Base extensions
    /// </summary>
    public static class BaseExtensions
    {
        public static Dictionary<TKey, TValue> RandomValues<TKey, TValue>(this Dictionary<TKey, TValue> dict, int count)
        {
            Random rand = new Random();
            Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
            int size = dict.Count;
            count = count > size ? size : count;
            List<TKey> values = Enumerable.ToList(dict.Keys);
            while (dic.Count < count)
            {
                TKey tk = values[rand.Next(size)];
                if (!dic.Keys.Contains(tk))
                {
                    dic[tk] = dict[tk];
                }
            }
            return dic;
        }

        public static (TKey, TValue) RandomValue<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            Random rand = new Random();
            int size = dict.Count;
            List<TKey> values = Enumerable.ToList(dict.Keys);
            TKey tk = values[rand.Next(size)];
            return (tk, dict[tk]);
        }
    }
}