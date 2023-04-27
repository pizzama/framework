using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace PFramework
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
                // //下面是第二种写法
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

        public static List<T> GetRandomSequence<T>(T[] array, int count)
        {
            List<T> output = new List<T>();
            int num;
            for (var i = 0; i < count; i++)
            {
                num = UnityEngine.Random.Range(0, array.Length);
                if (output.Contains(array[num]))
                {
                    i--;
                    continue;
                }
                output.Add(array[num]);
            }
            return output;
        }

        public static T[] GetRandomArray<T>(T[] array, int count)
        {
            T[] output = new T[count];
            for (var i = array.Length - 1; i >= 0 && count > 0; i--)
            {
                if (UnityEngine.Random.Range(0, i + 1) < count)
                {
                    output[count - 1] = array[i];
                    count--;
                }
            }
            return output;
        }

        public static T[] GetRandomArray2<T>(T[] array, int count)
        {
            T[] output = new T[count];
            int end = array.Length;
            int num;
            for (var i = 0; i < count; i++)
            {
                num = UnityEngine.Random.Range(0, end);
                output[i] = array[num];
                array[num] = array[end - 1];
                end--;
            }

            return output;
        }

        public static IEnumerator Shake(Transform baseTransform, float duration, float magnitude)
        {
            Vector3 originalPos = baseTransform.localPosition;

            float elapsed = 0.0f;

            while (elapsed < duration) // TODO: Consider if game is paused
            {
                float x = originalPos.x + UnityEngine.Random.Range(-1f, 1f) * magnitude;
                float y = originalPos.y + UnityEngine.Random.Range(-1f, 1f) * magnitude;
                baseTransform.localPosition = new Vector3(x, y, originalPos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }
            baseTransform.localPosition = originalPos;
        }

        public static T[] CombineArray<T>(T[] a, T[] b)
        {
            T[] c = new T[a.Length + b.Length];
            Array.Copy(a, 0, c, 0, a.Length);
            Array.Copy(b, 0, c, a.Length, b.Length);
            return c;
        }

    }

    //component 扩展类 组件copy
    public static class CopyComponent
    {
        public static T GetCopyOf<T>(this Component component, T componentToCopy) where T : Component
        {
            Type type = component.GetType();

            if (type != componentToCopy.GetType()) return null;

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;

            PropertyInfo[] propertyInformation = type.GetProperties(flags);

            foreach (var information in propertyInformation)
            {
                if (information.CanWrite)
                {
                    information.SetValue(component, information.GetValue(componentToCopy, null), null);
                }
            }

            FieldInfo[] fieldInformation = type.GetFields(flags);

            foreach (var information in fieldInformation)
            {
                information.SetValue(component, information.GetValue(componentToCopy));
            }

            return component as T;
        }
    }

}
