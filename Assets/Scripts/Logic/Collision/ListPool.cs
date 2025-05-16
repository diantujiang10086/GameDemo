using System.Collections.Generic;

public static class ListPool<T>
{
    private static readonly Stack<List<T>> pool = new(500);

    public static List<T> Get()
    {
        return pool.Count > 0 ? pool.Pop() : new List<T>(4);
    }

    public static void Release(List<T> list)
    {
        list.Clear();
        pool.Push(list);
    }
}

