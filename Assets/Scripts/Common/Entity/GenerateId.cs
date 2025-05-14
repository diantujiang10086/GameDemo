
using System;

public class GenerateId : Singleton<GenerateId>, IAwake
{
    private long count = 0;
    private long timestamp;

    public void Awake()
    {
        timestamp = (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
    }

    public long Create()
    {
        return timestamp + count++;
    }

}