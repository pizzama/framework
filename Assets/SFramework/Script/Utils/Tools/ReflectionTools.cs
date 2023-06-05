using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFramework
{
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
            object result = field.GetValue(instance, null);
            if (null == result)
                return defaultValue;
            if (!(result is T))
            {
                Debug.LogError(string.Format("field {0} cast failed!", fieldName));
                return defaultValue;
            }
            return (T)result;
        }
    }
}