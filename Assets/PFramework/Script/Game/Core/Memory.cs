using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFramework
{
    public class Memory<T1, T2>
    {
        public Dictionary<T1, Dictionary<T2, object>> values;
        public Memory()
        {
            values = new Dictionary<T1, Dictionary<T2, object>>();
        }

        public object GetValue(T1 key1, T2 key2)
        {
            
        }

        public object SetValue(T1 key1, T2 key2, object value)
        {
            
        }
    }

}