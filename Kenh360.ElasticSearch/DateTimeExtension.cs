using System;

namespace VinEcom.Oms.ElasticSearch
{
    public static class DateTimeExtension
    {
        public static long ToUnixTimestamp(this DateTime target)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
            var unixTimestamp = System.Convert.ToInt64((target - date).TotalSeconds);

            return unixTimestamp;
        }

        public static DateTime ToDateTime(this DateTime target, long timestamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);

            return dateTime.AddSeconds(timestamp);
        }

        public static DateTime ToDailyTime(this DateTime target)
        {
            var dateTime = new DateTime(target.Year, target.Month, target.Day, 0, 0, 0, target.Kind);

            return dateTime;
        }

        public static DateTime ToHourlyTime(this DateTime target)
        {
            var dateTime = new DateTime(target.Year, target.Month, target.Day, target.Hour, 0, 0, target.Kind);

            return dateTime;
        }
        
    }
}
