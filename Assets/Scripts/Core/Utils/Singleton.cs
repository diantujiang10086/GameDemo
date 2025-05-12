using System;

public class Singleton<T>
{
    private static T instance;
    private static object objectLock = new object();

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                lock(objectLock)
                {
                    instance = (T)Activator.CreateInstance(typeof(T));
                }
            }
            return instance;
        }
    }
}
