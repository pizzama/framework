using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SFramework.Tools
{
    public class TimeTools
    {
        /// <summary>
        /// 获取时间戳-单位秒
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStampSecond()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            try
            {
                return Convert.ToInt64(ts.TotalSeconds);
            }
            catch (Exception ex)
            {
                Debug.Log($"GetTimeStampSecond Error = {ex}");
                return 0;
            }
        }

        /// <summary>
        /// 获取时间戳-单位毫秒
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStampMilliSecond()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            try
            {
                return Convert.ToInt64(ts.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                Debug.Log($"GetTimeStampMilliSecond Error = {ex}");
                return 0;
            }
        }

        /// <summary>
        /// 显示当前时间，显示的格式为20220506-11:09:30
        /// </summary>
        public static string GetTime()
        {
            string year = DateTime.Now.Year.ToString();
            string month =
                DateTime.Now.Month < 10
                    ? "0" + DateTime.Now.Month.ToString()
                    : DateTime.Now.Month.ToString();
            string day =
                DateTime.Now.Day < 10
                    ? "0" + DateTime.Now.Day.ToString()
                    : DateTime.Now.Day.ToString();
            string hour =
                DateTime.Now.Hour < 10
                    ? "0" + DateTime.Now.Hour.ToString()
                    : DateTime.Now.Hour.ToString();
            string minute =
                DateTime.Now.Minute < 10
                    ? "0" + DateTime.Now.Minute.ToString()
                    : DateTime.Now.Minute.ToString();
            string second =
                DateTime.Now.Second < 10
                    ? "0" + DateTime.Now.Second.ToString()
                    : DateTime.Now.Second.ToString();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(year);
            stringBuilder.Append(month);
            stringBuilder.Append(day);
            stringBuilder.Append("-");
            stringBuilder.Append(hour);
            stringBuilder.Append(":");
            stringBuilder.Append(minute);
            stringBuilder.Append(":");
            stringBuilder.Append(second);
            Debug.Log($"当前时间 = {stringBuilder.ToString()}");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Unix时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式,例如:1482115779, 或long类型</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// Unix时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式,例如:1482115779, 或long类型</param>
        /// <param name="zoneHours">时区</param>
        /// <returns>C#格式时间</returns>
        public static DateTime CoverToZoneDateTime(string timeStamp, int zoneHours)
        {
            var rt = TimeTools.GetDateTime(timeStamp.ToString());
            var rr = rt.ToUniversalTime();
            rr = rr.AddHours(zoneHours);

            return rr;
        }
    }
}
