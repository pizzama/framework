using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace PUtils
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

        // static string[] unitList = new string[] { "", "K", "M", "B", "T", "AA", "AB", "AC", "AD" };
        static string[] unitList = new string[] { "", "千", "百万", "十亿", "万亿", "百兆", "十京", "垓", "千垓" };
        public static string FormatCurrency(long num, int digit = 1)
        {
            float tempNum = num;
            long v = 1000;//几位一个单位
            int unitIndex = 0;
            while (tempNum >= v)
            {
                unitIndex++;
                tempNum /= v;
            }

            string str = "";
            if (unitIndex >= unitList.Length)
            {
                Debug.LogError("超出单位表中的最大单位");
                str = num.ToString();
            }
            else
            {
                tempNum = Round(tempNum, digit);
                str = $"{tempNum}{unitList[unitIndex]}";
            }
            return str;
        }

        public static float Round(float value, int digits = 1)
        {
            float multiple = Mathf.Pow(10, digits);
            float tempValue = value * multiple + 0.5f;
            tempValue = Mathf.FloorToInt(tempValue);
            return tempValue / multiple;
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

        public static Vector2 ScreenToUILocalPos(RectTransform targetParentRect, Vector2 mousePos, Camera canvasCam = null)
        {
            //targetRect 目标物体，也就是ui物体
            // Canvas 的RenderMode在Camera和World模式下，传入的camera为UI摄像机，在OverLay，camara参数应该传入null
            Vector2 uiLocalPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(targetParentRect, mousePos, canvasCam, out uiLocalPos);
            return uiLocalPos; //是当前物体的本地坐标
        }

        public static Vector3 ScreenToUIWorldPos(RectTransform targetParentRect, Vector2 mousePos, Camera canvasCam = null)
        {
            //targetRect 目标物体，也就是ui物体
            // Canvas 的RenderMode在Camera和World模式下，传入的camera为UI摄像机，在OverLay，camara参数应该传入null
            Vector3 worldPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(targetParentRect, mousePos, canvasCam, out worldPos);
            return worldPos; //转换到世界坐标
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

        //计算圆心半径求点
        public static Vector2 CircularVecotr2(Vector2 center, float radius, float angle)
        {
            float x = center.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad); //Mathf.PI / 180f
            float y = center.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            return new Vector2(x, y);
        }

        //计算椭圆半径求点, 分别是xradius, yradius 长短轴
        public static Vector2 EllipseVector2(Vector2 center, float xradius, float yradius, float angle)
        {
            float x = center.x + xradius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = center.y + yradius * Mathf.Sin(angle * Mathf.Deg2Rad);
            return new Vector2(x, y);
        }

        //计算双曲线, xradius, yradisu 实轴和虚轴
        public static Vector2 HyperbolaVector2(float xradius, float yradisu, float angle)
        {
           float x =  xradius / Mathf.Cos(angle * Mathf.Deg2Rad);
           float y = yradisu / Mathf.Tan(angle * Mathf.Deg2Rad);
           return new Vector2(x, y);
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
