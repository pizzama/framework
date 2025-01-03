using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SFramework.Tools
{
    public class ObjectTools
    {
        //you must be add Serializable tag on class which one you want to deepcopy
        public static T SerializableDeepCopy<T>(T obj)
        {
            object retval;
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            retval = bf.Deserialize(ms);
            ms.Close();
            return (T)retval;
        }

        public static Type GetType(string TypeName)
        {
            var type = Type.GetType(TypeName);
            if (type != null)
                return type;

            if (TypeName.Contains("."))
            {
                var assemblyName = TypeName.Substring(0, TypeName.IndexOf('.'));
                var assembly = Assembly.Load(assemblyName);
                if (assembly == null)
                    return null;
                type = assembly.GetType(TypeName);
                if (type != null)
                    return type;
            }

            var currentAssembly = Assembly.GetExecutingAssembly();
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
            foreach (var assemblyName in referencedAssemblies)
            {
                var assembly = Assembly.Load(assemblyName);
                if (assembly != null)
                {
                    type = assembly.GetType(TypeName);
                    if (type != null)
                        return type;
                }
            }
            return null;
        }

        public static T CreateInstance<T>(string nameSpace, string className, string assemblyName = "Assembly-CSharp")
        {
            try
            {
                string fullName = nameSpace + "." + className;//命名空间.类型名
                //此为第一种写法
                object ect = Assembly.Load(assemblyName).CreateInstance(fullName);//加载程序集，创建程序集里面的 命名空间.类型名 实例
                return (T)ect;//类型转换并返回
                //下面是第二种写法
                // string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
                // Type o = Type.GetType(path);//加载类型
                // object obj = Activator.CreateInstance(o, true);//根据类型创建实例
                // return (T)obj;//类型转换并返回
            }
            catch
            {
                //发生异常，返回类型的默认值
                return default(T);
            }
        }

        public static T CreateInstance<T>(T classtype)
        {
            try
            {
                Type o = classtype.GetType();
                object obj = Activator.CreateInstance(o, true);//根据类型创建实例
                return (T)obj;//类型转换并返回
            }
            catch
            {
                //发生异常，返回类型的默认值
                return default(T);
            }
        }

        //Fischer-Yates shuffle
        public static List<T> ShuffleList<T>(List<T> array)
        {
            for (var i = array.Count; i > 0; --i)
            {
                var j = UnityEngine.Random.Range(0, i);
                var index = array[j];
                array[j] = array[i - 1];
                array[i - 1] = index;
            }

            return array;
        }

        //Fischer-Yates shuffle 洗牌算法
        public static T[] ShuffleList<T>(T[] array)
        {
            for (var i = array.Length; i > 0; --i)
            {
                var j = UnityEngine.Random.Range(0, i);
                var index = array[j];
                array[j] = array[i - 1];
                array[i - 1] = index;
            }

            return array;
        }

        public static T[] CombineArray<T>(T[] a, T[] b)
        {
            T[] c = new T[a.Length + b.Length];
            Array.Copy(a, 0, c, 0, a.Length);
            Array.Copy(b, 0, c, a.Length, b.Length);
            return c;
        }
    }

}
