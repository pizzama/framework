using UnityEngine;

namespace PFramework
{
    public class StringTools
    {
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
                tempNum = MathTools.Round(tempNum, digit);
                str = $"{tempNum}{unitList[unitIndex]}";
            }
            return str;
        }

        public static void PrefixClassName(string classPath, out string nameSpace, out string className)
        {
            int index = classPath.IndexOf(".");
            if(index < 0)
            {
                nameSpace = "";
                className = classPath;
            }
            else
            {
                nameSpace = classPath.Substring(0, index);
                className = classPath.Substring(index, classPath.Length - 1);
            }
        }
    }
}