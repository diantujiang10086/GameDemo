using System;
using UnityDebug = UnityEngine.Debug;

public class Log
{
    public static void Debug(string message)
    {
        UnityDebug.Log(message);
    }
    public static void Warning(string message)
    {
        UnityDebug.LogWarning(message);
    }
    public static void Error(string message)
    {
        UnityDebug.LogError(message);
    }
    public static void Error(Exception exception)
    {
        UnityDebug.LogError(exception);
    }
}