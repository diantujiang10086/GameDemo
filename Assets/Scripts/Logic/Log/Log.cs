using System;
public class Log
{
    public static ILog log = new ConsoleLog();
    public static void Debug(string message)
    {
        log.Debug(message);
    }

    public static void Error(string message)
    {
        log.Error(message);
    }

    public static void Error(Exception exception)
    {
        log.Error(exception);
    }

    public static void Warning(string message)
    {
        log.Warning(message);
    }
}

