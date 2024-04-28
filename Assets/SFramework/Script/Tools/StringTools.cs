using SFramework.Tools.Math;
using UnityEngine;


namespace SFramework.Tools
{
    public class StringTools
    {
        // static string[] unitList = new string[] { "", "K", "M", "B", "T", "AA", "AB", "AC", "AD" };
        static string[] unitList = new string[]
        {
            "",
            "千",
            "百万",
            "十亿",
            "万亿",
            "百兆",
            "十京",
            "垓",
            "千垓"
        };

        public static string FormatCurrency(long num, int digit = 1)
        {
            float tempNum = num;
            long v = 1000; //几位一个单位
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

        public static void PrefixClassName(
            string classPath,
            out string nameSpace,
            out string className
        )
        {
            int index = classPath.LastIndexOf(".");
            if (index < 0)
            {
                nameSpace = "";
                className = classPath;
            }
            else
            {
                nameSpace = classPath.Substring(0, index);
                className = classPath.Substring(index + 1, classPath.Length - index - 1);
            }
        }

        public static string GenerateRandomNumber(int Length)
        {
            char[] constant =
            {
                '0',
                '1',
                '2',
                '3',
                '4',
                '5',
                '6',
                '7',
                '8',
                '9',
                'a',
                'b',
                'c',
                'd',
                'e',
                'f',
                'g',
                'h',
                'i',
                'j',
                'k',
                'l',
                'm',
                'n',
                'o',
                'p',
                'q',
                'r',
                's',
                't',
                'u',
                'v',
                'w',
                'x',
                'y',
                'z',
                'A',
                'B',
                'C',
                'D',
                'E',
                'F',
                'G',
                'H',
                'I',
                'J',
                'K',
                'L',
                'M',
                'N',
                'O',
                'P',
                'Q',
                'R',
                'S',
                'T',
                'U',
                'V',
                'W',
                'X',
                'Y',
                'Z'
            };
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            System.Random rd = new System.Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }
    }
}
