using System;
using UnityEngine;

namespace SFramework.Tools.Math
{
    [Serializable]
    public struct MinMax
    {
        public float min;
        public float max;

        public MinMax(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public bool Contains(float value)
        {
            return min <= value && value <= max;
        }

        private static System.Random random = new System.Random();
        public float Random()
        {
            return (float)(min + random.NextDouble() * (max - min));
        }

        public Vector2 RandomVector2()
        {
            return new Vector2(Random(), Random());
        }
    }
    public class MathTools
    {
        private static readonly System.Random random = new System.Random();
        //remap value from t1,t2 to s1 s2
        public static float Remap(float value, float t1, float t2, float s1, float s2)
        {
            return (value - t1) / (t2 - t1) * (s2 - s1) + s1;
        }

        public static float Round(float value, int digits = 1)
        {
            float multiple = Mathf.Pow(10, digits);
            float tempValue = value * multiple + 0.5f;
            tempValue = Mathf.FloorToInt(tempValue);
            return tempValue / multiple;
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
            float x = xradius / Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = yradisu / Mathf.Tan(angle * Mathf.Deg2Rad);
            return new Vector2(x, y);
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

        public static float GetRandomFloat(float minValue, float maxValue)
        {
            return (float)(random.NextDouble() * (maxValue - minValue + 1) + minValue);
        }

        public static float MapToRange(float dvalue, float dmin, float dmax, float tmin, float tmax)
        {
            //tmin + (DVALUE - DMIN)(tmax - tmin) / (DMAX - DMIN)
            return tmin + (dvalue - dmin) * (tmax - tmin) / (dmax - dmin);
        }
    }
}