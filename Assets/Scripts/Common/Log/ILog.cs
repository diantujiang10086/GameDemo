using System;

public interface ILog
{
    void Debug(string message);
    void Warning(string message);
    void Error(string message);
    void Error(Exception exception);
}
