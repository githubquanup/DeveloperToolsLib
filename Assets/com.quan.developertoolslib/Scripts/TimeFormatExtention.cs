using System;
using System.Text.RegularExpressions;

namespace DeveloperToolsLib{
    public static class TimeFormatExtention{
        #region 返回指定格式时间
        /// <summary>时分秒</summary>
        public static string FormatTime(int totalSeconds)
        {
            int hours = totalSeconds / 3600;
            string hh = hours < 10 ? "0" + hours : hours.ToString();
            int minutes = (totalSeconds - hours * 3600) / 60;
            string mm = minutes < 10f ? "0" + minutes : minutes.ToString();
            int seconds = totalSeconds - hours * 3600 - minutes * 60;
            string ss = seconds < 10 ? "0" + seconds : seconds.ToString();
            return string.Format("{0}:{1}:{2}", hh, mm, ss);
        }
        /// <summary>分秒</summary>
        public static string FormatTwoTime(int totalSeconds)
        {
            int minutes = totalSeconds / 60;
            string mm = minutes < 10f ? "0" + minutes : minutes.ToString();
            int seconds = (totalSeconds - (minutes * 60));
            string ss = seconds < 10 ? "0" + seconds : seconds.ToString();
            return string.Format("{0}:{1}", mm, ss);
        }
        /// <summary>天时分秒</summary>
        public static string FormatDayTime(int totalSeconds)
        {
            int days = (totalSeconds / 3600) / 24;
            string dd = days < 10 ? "0" + days : days.ToString();
            int hours = (totalSeconds / 3600) - (days * 24);
            string hh = hours < 10 ? "0" + hours : hours.ToString();
            int minutes = (totalSeconds - (hours * 3600) - (days * 86400)) / 60;
            string mm = minutes < 10f ? "0" + minutes : minutes.ToString();
            int seconds = totalSeconds - (hours * 3600) - (minutes * 60) - (days * 86400);
            string ss = seconds < 10 ? "0" + seconds : seconds.ToString();
            return string.Format("{0}:{1}:{2}:{3}", dd, hh, mm, ss);
        }
        #endregion

        #region 返回返回给定的时间格式是否正确
        /// <summary>返回给定的时间格式是否正确
        /// ，支持识别（-/.）分隔的日期格式，或是未加分隔符的日期格式：年月日
        /// </summary>
        public static bool GetIsFormattedData(string timer)
        {
            Regex regex = new Regex(@"^(?:(?!0000)[0-9]{4}([-/.]?)(?:(?:0?[1-9]|1[0-2])\1(?:0?[1-9]|1[0-9]|2[0-8])|(?:0?[13-9]|1[0-2])\1(?:29|30)|(?:0?[13578]|1[02])\1(?:31))|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)([-/.]?)0?2\2(?:29))$");
            return !regex.IsMatch(timer);
        }
        public static bool GetIsFormattedTimer(string timer)
        {
            Regex regex = new Regex(@"^(?:(?!0000)[0-9]{4}([-/.]?)(?:(?:0?[1-9]|1[0-2])\1(?:0?[1-9]|1[0-9]|2[0-8])|(?:0?[13-9]|1[0-2])\1(?:29|30)|(?:0?[13578]|1[02])\1(?:31))|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)([-/.]?)0?2\2(?:29))([0-1]?[0-9]|2[0-3])([:]?)([0-1]?[0-9]|(2|3|4|5)[0-9])$");
            return !regex.IsMatch(timer);
        }
        #endregion
    }
}