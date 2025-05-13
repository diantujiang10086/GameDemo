using System;
using UnityDebug = UnityEngine.Debug;
public class UnityLog : ILog
{
    public void Debug(string message)
    {
        UnityDebug.Log(message);
    }
    public void Warning(string message)
    {
        UnityDebug.LogWarning(message);
    }
    public void Error(string message)
    {
        UnityDebug.LogError(message);
    }
    public void Error(Exception exception)
    {
        UnityDebug.LogError(exception);
    }
}