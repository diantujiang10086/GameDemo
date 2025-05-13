
using System;

public class ConsoleLog : ILog
{
    public void Debug(string message)
    {
        System.Console.WriteLine(message);
    }

    public void Error(string message)
    {
        System.Console.WriteLine(message);
    }

    public void Error(Exception exception)
    {
        System.Console.WriteLine(exception);
    }

    public void Warning(string message)
    {
        System.Console.WriteLine(message);
    }
}

