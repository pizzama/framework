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
    }
}