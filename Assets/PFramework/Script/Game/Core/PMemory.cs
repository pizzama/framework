using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFramework
{
    public class PMemory<T1, T2, T3>
    {
        public Dictionary<T1, Dictionary<T2, T3>> values;
        public PMemory()
        {
            values = new Dictionary<T1, Dictionary<T2, T3>>();
        }

        public T3 GetValue(T1 key1, T2 key2)
        {
            Dictionary<T2, T3> vv;
            values.TryGetValue(key1, out vv);
            T3 result;
            if(vv != null)
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
            if(vv == null)
            {
                vv = new Dictionary<T2, T3>();
            }
            vv[key2] = value;
        }

        public T3 DeleteValue(T1 key1, T2 key2)
        {
            Dictionary<T2, T3> vv;
            values.TryGetValue(key1, out vv);
            T3 result = default;
            if(vv != null)
            {
                vv.TryGetValue(key2, out result);
                vv.Remove(key2);
            }

            return result;
        }
    }

}