using System;

public class TimeUtils
{
    public static int ConvertDateTimeInt(DateTime time)
    {
        DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        return (int)(time - dateTime).TotalSeconds;
    }

    public static int GetNow()
    {
        DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        return (int)(DateTime.Now - dateTime).TotalSeconds;
    }

    public static DateTime GetTime(string timeStamp)
    {
        DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long ticks = long.Parse(timeStamp + "0000000");
        TimeSpan value = new TimeSpan(ticks);
        return dateTime.Add(value);
    }

    public static void SplitTime(ulong timeData, out ulong d, out ulong h, out ulong m, out ulong s)
    {
        d = timeData / 86400;
        timeData %= 86400;
        h = timeData / 3600;
        timeData %= 3600;
        m = timeData / 60;
        s = timeData % 60;
    }

    public static string StampToDateTime(ulong timeStamp, string format = "yyyy-MM-dd")
    {
        DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long ticks = long.Parse(timeStamp + "0000000");
        TimeSpan value = new TimeSpan(ticks);
        return dateTime.Add(value).ToString(format);
    }

    public static int GetSystemTicksMS()
    {
        return Environment.TickCount & 0x7FFFFFFF;
    }

    public static float GetSystemTicksS()
    {
        return (float)(Environment.TickCount & 0x7FFFFFFF) / 1000f;
    }

    public static DateTime DateTimeFromSecords(int seconds)
    {
        DateTime result = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        result.AddSeconds(seconds);
        return result;
    }

    public static string FormatTimeHHMMSS(int seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return $"{timeSpan.Hours + timeSpan.Days * 24:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

    public static string FormatTimeDDHHMMSS(int seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return $"{timeSpan.Days:D2}:{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

    public static string FormatTimeMMSS(int seconds)
    {
        return $"{seconds / 60:D2}:{seconds % 60:D2}";
    }

    public static int GetRemainTime(int serverRemainTime, DateTime refreshDateTime)
    {
        int num = (int)Math.Round((float)serverRemainTime - (float)(DateTime.Now - refreshDateTime).TotalSeconds);
        if (num < 0)
        {
            num = 0;
        }

        return num;
    }

    public static int GetDayOffset(int startTime, int curTime)
    {
        DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(startTime);
        DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        DateTime dateTime3 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(curTime);
        DateTime dateTime4 = new DateTime(dateTime3.Year, dateTime3.Month, dateTime3.Day);
        return (int)(dateTime4 - dateTime2).TotalDays;
    }

    public static int GetStampTimeWeekly(int seconds)
    {
        return (int)TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(seconds).DayOfWeek;
    }

    public static int GetStampTimeHH(int seconds)
    {
        return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(seconds).Hour;
    }

    public static int GetStampTimeMM(int seconds)
    {
        return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(seconds).Minute;
    }

    public static int GetStampTimeSS(int seconds)
    {
        return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(seconds).Second;
    }

    public static int GetStampTimeWeeklyNotZone(int seconds)
    {
        return (int)new DateTime(1970, 1, 1).AddSeconds(seconds).DayOfWeek;
    }

    public static int GetDayOffsetNotZone(int startTime, int curTime)
    {
        DateTime dateTime = new DateTime(1970, 1, 1).AddSeconds(startTime);
        DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        DateTime dateTime3 = new DateTime(1970, 1, 1).AddSeconds(curTime);
        DateTime dateTime4 = new DateTime(dateTime3.Year, dateTime3.Month, dateTime3.Day);
        return (int)(dateTime4 - dateTime2).TotalDays;
    }

    public static int GetStampTimeHHNotZone(int seconds)
    {
        return new DateTime(1970, 1, 1).AddSeconds(seconds).Hour;
    }

    public static int GetStampTimeMMNotZone(int seconds)
    {
        return new DateTime(1970, 1, 1).AddSeconds(seconds).Minute;
    }

    public static int GetStampTimeSSNotZone(int seconds)
    {
        return new DateTime(1970, 1, 1).AddSeconds(seconds).Second;
    }
}