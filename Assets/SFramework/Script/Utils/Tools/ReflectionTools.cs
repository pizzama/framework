using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SFramework
{
    /*
    * ReflectionTools.Call("UnityEditor.EditorGUIUtility, UnityEditor", "DrawHorizontalSplitter", new Rect(0f,0f,100f,100f));
    * GUIStyle style = ReflectionTools.GetProperty<GUIStyle>("UnityEditor.EditorStyles, UnityEditor", "toolbarSearchField");
    */
    public class ReflectionTools
    {
        public static void Call(string typeName, string methodName, params object[] args)
        {
            Call<object>(typeName, methodName, args);
        }

        public static T Call<T>(string typeName, string methodName, params object[] args)
        {
            Type type = Type.GetType(typeName);
            T defaultValue = default(T);
            if (null == type) return defaultValue;

            Type[] argTypes = new Type[args.Length];
            for (int i = 0, count = args.Length; i < count; ++i)
            {
                argTypes[i] = null != args[i] ? args[i].GetType() : null;
            }
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, argTypes, null);
            if (null == method)
            {
                Debug.LogError(string.Format("method {0} does not exist!", methodName));
                return defaultValue;
            }
            object result = method.Invoke(null, args);
            if (null == result)
                return defaultValue;
            if (!(result is T))
            {
                Debug.LogError(string.Format("method {0} cast failed!", methodName));
                return defaultValue;
            }
            return (T)result;
        }

        public static T GetProperty<T>(string typeName, string propertyName)
        {
            return GetProperty<T>(null, typeName, propertyName);
        }

        public static T GetProperty<T>(object instance, string typeName, string propertyName)
        {
            bool isStatic = null == instance;
            Type type = Type.GetType(typeName);
            T defaultValue = default(T);
            if (null == type) return defaultValue;

            BindingFlags flag = (isStatic ? BindingFlags.Static : BindingFlags.Instance);
            PropertyInfo property = type.GetProperty(propertyName, flag | BindingFlags.Public | BindingFlags.NonPublic);
            if (null == property)
            {
                Debug.LogError(string.Format("property {0} does not exist!", propertyName));
                return defaultValue;
            }
            object result = property.GetValue(instance, null);
            if (null == result)
                return defaultValue;
            if (!(result is T))
            {
                Debug.LogError(string.Format("property {0} cast failed!", propertyName));
                return defaultValue;
            }
            return (T)result;
        }

        public static T GetField<T>(string typeName, string fieldName)
        {
            return GetField<T>(null, typeName, fieldName);
        }

        public static T GetField<T>(object instance, string typeName, string fieldName)
        {
            bool isStatic = null == instance;
            Type type = Type.GetType(typeName);
            T defaultValue = default(T);
            if (null == type) return defaultValue;

            BindingFlags flag = (isStatic ? BindingFlags.Static : BindingFlags.Instance);
            FieldInfo field = type.GetField(fieldName, flag | BindingFlags.Public | BindingFlags.NonPublic);
            if (null == field)
            {
                Debug.LogError(string.Format("field {0} does not exist!", fieldName));
                return defaultValue;
            }
            object result = field.GetValue(instance);
            if (null == result)
                return defaultValue;
            if (!(result is T))
            {
                Debug.LogError(string.Format("field {0} cast failed!", fieldName));
                return defaultValue;
            }
            return (T)result;
        }


        /// <summary>
        /// 从所有的程序集中取到子类  特别费性能
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="immediate class">是否是直系的类型</param>
        /// <param name="exception"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<Type> GetTypesFormTypeWithAllAssembly(Type baseType, bool immediateClass = false,
            bool exception = true)
        {
            if (baseType == null)
            {
                throw new Exception("Type is invalid.");
            }

            if (string.IsNullOrEmpty(baseType.FullName))
            {
                throw new Exception($"Type '{baseType}' full name is invalid");
            }


            List<Type> types = new List<Type>();
            Assembly[] sAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in sAssemblies)
            {
                var allTypes = assembly.GetTypes();
                foreach (var type in allTypes)
                {
                    if (!baseType.IsAssignableFrom(type))
                    {
                        continue;
                    }

                    if (!type.IsClass)
                    {
                        continue;
                    }

                    if (type.IsInterface)
                    {
                        continue;
                    }

                    if (type.IsAbstract)
                    {
                        continue;
                    }

                    if (type == baseType)
                    {
                        continue;
                    }

                    if (immediateClass && type.BaseType != baseType)
                    {
                        continue;
                    }

                    types.Add(type);
                }
            }

            if (types.Count <= 0)
            {
                if (exception)
                {
                    throw new Exception("you have must to have implementation class . Type :" + baseType.FullName);
                }
            }

            return types;
        }
        
    }
}