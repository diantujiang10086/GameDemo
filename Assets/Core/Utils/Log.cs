using System;
using UnityDebug = UnityEngine.Debug;
public class Log
{
    public static void Debug(string msg)
    {
        UnityDebug.Log(msg);
    }
    public static void Warning(string msg) 
    {
        UnityDebug.LogWarning(msg);
    }
    public static  void Error(string msg)
    {
        UnityDebug.LogError(msg);
    }
    public static void Error(Exception ex)
    {
        UnityDebug.LogError(ex);
    }
}