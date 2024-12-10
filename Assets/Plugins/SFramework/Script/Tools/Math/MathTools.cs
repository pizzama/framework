using System;
using UnityEngine;
using System.Collections.Generic;

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

        public static int RandomInt(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        /// <summary>
        /// 将一个范围的值映射到另一个范围
        /// </summary>
        /// <param name="dvalue">源范围的值</param>
        /// <param name="dmin">源范围的最小值</param>
        /// <param name="dmax">源范围的最大值</param>
        /// <param name="tmin">目标范围的最小值</param>
        /// <param name="tmax">目标范围的最大值</param>
        /// <returns>映射后的值</returns>
        public static float MapToRange(float dvalue, float dmin, float dmax, float tmin, float tmax)
        {
            //tmin + (DVALUE - DMIN)(tmax - tmin) / (DMAX - DMIN)
            return tmin + (dvalue - dmin) * (tmax - tmin) / (dmax - dmin);
        }

        /// <summary>
        /// 将x和y坐标打包成一个整数
        /// </summary>
        /// <param name="x">x坐标 (-32768 到 32767)</param>
        /// <param name="y">y坐标 (-32768 到 32767)</param>
        /// <returns>打包后的整数</returns>
        public static int PackCoordinates(short x, short y)
        {
            return (x << 16) | (y & 0xFFFF);
        }

        /// <summary>
        /// 从打包的整数中解析出x和y坐标
        /// </summary>
        /// <param name="packed">打包的整数</param>
        /// <returns>包含x和y坐标的Vector2</returns>
        public static Vector2 UnpackCoordinates(int packed)
        {
            short x = (short)(packed >> 16);
            short y = (short)(packed & 0xFFFF);
            return new Vector2(x, y);
        }
    }
}