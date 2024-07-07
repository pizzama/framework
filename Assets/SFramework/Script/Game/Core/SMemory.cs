using System.Collections;
using System.Collections.Generic;

namespace SFramework
{
    public class SMemory<T1, T2, T3> : IEnumerable
    {
        public Dictionary<T1, Dictionary<T2, T3>> values;

        public object Current => throw new System.NotImplementedException();

        public SMemory()
        {
            values = new Dictionary<T1, Dictionary<T2, T3>>();
        }

        public T3 GetValue(T1 key1, T2 key2)
        {
            Dictionary<T2, T3> vv;
            values.TryGetValue(key1, out vv);
            T3 result;
            if (vv != null)
            {
                vv.TryGetValue(key2, out result);
                return result;
            }
            else
            {
                return default;
            }

        }

        public void SetValue(T1 key1, T2 key2, T3 value)
        {
            Dictionary<T2, T3> vv;
            values.TryGetValue(key1, out vv);
            if (vv == null)
            {
                vv = new Dictionary<T2, T3>();
                values[key1] = vv;
            }
            vv[key2] = value;
        }

        public T3 DeleteValue(T1 key1, T2 key2)
        {
            Dictionary<T2, T3> vv;
            values.TryGetValue(key1, out vv);
            T3 result = default;
            if (vv != null)
            {
                vv.TryGetValue(key2, out result);
                vv.Remove(key2);
            }

            return result;
        }

        public T GetValue<T>()
        {
            foreach (var map in values)
            {
                foreach (var item in map.Value)
                {
                    if (item.Value.GetType() == typeof(T))
                    {
                        return (T)(object)item.Value;
                    }
                }
            }
            return default;
        }

        public T3 FindT2First(T2 key2)
        {
            foreach(var map in values)
            {
                if(map.Value.ContainsKey(key2))
                {
                    return map.Value[key2];
                }
            }
            return default;
        }

        public List<T> GetAllValue<T>()
        {
            List<T> temp = new List<T>();
            foreach (var map in values)
            {
                foreach (var item in map.Value)
                {
                    if (item.Value.GetType() == typeof(T))
                    {
                        temp.Add((T)(object)item.Value);
                    }
                }
            }

            return temp;
        }

        public IEnumerator GetEnumerator()
        {
           return values.GetEnumerator();
        }

        public void Clear()
        {
            foreach (var item in values)
            {
                item.Value.Clear();
            }

            values.Clear();
        }

    }

}