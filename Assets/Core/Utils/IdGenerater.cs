using System;

public class IdGenerater
{
    private static int value;
    private static long timestamp;

    public static void Initialize()
    {
        value = 0;
        timestamp = (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
    }

    public static long GenerateId()
    {
        return timestamp+++value;
    }

}