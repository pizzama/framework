using System;
using System.Collections.Generic;
using System.Text;
using SFramework.Tools.Math;
using UnityEngine;


namespace SFramework.Tools
{
    public class StringTools
    {
        // 中文数字字符串数组
        private readonly static string[] chineseNumbers = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
        /// <summary>
        /// 将阿拉伯数字转换为中文数字
        /// </summary>
        /// <param name="number">要转换的阿拉伯数字</param>
        /// <returns>转换后的中文数字</returns>
        public static string ConvertNumberToChinese(int number)
        {
            if (number < 0)
            {
                return "不支持负数";
            }

            // 处理 0 到 10 的数字
            if (number <= 10)
            {
                return chineseNumbers[number];
            }

            // 处理 11 到 99 的数字
            if (number < 100)
            {
                int tens = number / 10;
                int units = number % 10;

                if (units == 0)
                {
                    return chineseNumbers[tens] + chineseNumbers[10]; // 例如：20 -> 二十
                }
                else
                {
                    return chineseNumbers[tens] + chineseNumbers[10] + chineseNumbers[units]; // 例如：23 -> 二十三
                }
            }

            if (number < 1000)
            {
                int hundreds = number / 100;
                int remainder = number % 100;
                string result = chineseNumbers[hundreds] + "百";

                if (remainder > 0)
                {
                    result += ConvertNumberToChinese(remainder);
                }

                return result;
            }

            // 处理 100 及以上的数字
            return "不支持超过 1000 的数字";
        }
        // static string[] unitList = new string[] { "", "K", "M", "B", "T", "AA", "AB", "AC", "AD" };
        static string[] unitList = new string[]
        {
            "", "千", "百万", "十亿", "万亿", "百兆", "十京", "垓", "千垓"
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

        public static string TimeFormat(double value, string language = "")
        {
            var ts = TimeSpan.FromSeconds(value);
            // if (string.IsNullOrEmpty(language))
            // {
            if (ts.Days > 0)
            {
                return $"{ts.Days}D {ts.Hours}H";
            }

            if (ts.Hours > 0)
            {
                return $"{GetShowTime(ts.Hours)}H {GetShowTime(ts.Minutes)} M {GetShowTime(ts.Seconds)}S";
            }

            return $"{GetShowTime(ts.Minutes)}M {GetShowTime(ts.Seconds)}S";
            // }
            // else
            // {
            //     return language
            // }
        }

        private static string GetShowTime(int value)
        {
            if (value >= 10)
            {
                return value.ToString();
            }

            return $"0{value}";
        }
        
        /// <summary>
        /// 将 Dictionary<string, string> 转换为 URL 查询参数
        /// </summary>
        /// <param name="parameters">包含参数的字典</param>
        /// <returns>URL 查询参数字符串</returns>
        public static string BuildQueryString(Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return string.Empty;
            }

            // 使用 StringBuilder 提高性能
            StringBuilder queryString = new StringBuilder();

            // 遍历字典，拼接参数
            foreach (var pair in parameters)
            {
                if (queryString.Length > 0)
                {
                    queryString.Append("&"); // 如果不是第一个参数，添加 &
                }

                // 对 key 和 value 进行 URL 编码
                queryString.Append(System.Web.HttpUtility.UrlEncode(pair.Key));
                queryString.Append("=");
                queryString.Append(System.Web.HttpUtility.UrlEncode(pair.Value));
            }

            return queryString.ToString();
        }
    }
}