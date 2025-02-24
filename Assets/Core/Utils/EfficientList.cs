using System;

public class EfficientList<T>
{
    private int count;
    private T[] items;

    // 获取列表的元素数量
    public int Count => count;

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            return items[index];
        }
    }

    public EfficientList(int capacity = 2)
    {
        items = new T[capacity];
    }

    public int Add(T item)
    {
        if (count >= items.Length)
        {
            var newItems = new T[items.Length * 2];
            Array.Copy(items, newItems, count);
            items = newItems;
        }

        var index = count++;
        items[index] = item;
        return index;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        return items[index];
    }

    public void Remove(int index)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

        if (index != count - 1)
        {
            items[index] = items[count - 1];
        }

        count--;
    }

}
