using System;

public class IdGeneraterSystem : ISystem, IAwake
{
    private int value;
    private long timestamp;
    private static IdGeneraterSystem instance;
    public void Awake()
    {
        instance = this;
        timestamp = (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
    }

    public static long GenerateId()
    {
        return instance.timestamp+++instance.value;
    }

}