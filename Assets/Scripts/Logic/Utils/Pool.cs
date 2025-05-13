using System;

public class Pool<T> where T : class
{
    private T[] items;
    private int capacity;
    private int number;
    private readonly bool isScaling;
    public int Count => number;

    public Pool(int initialCapacity, bool isScaling = false)
    {
        capacity = initialCapacity;
        items = new T[capacity];
        this.isScaling = isScaling;
        number = 0;
    }

    public T CreateElement(out int index)
    {
        if (number >= capacity)
        {
            if (!isScaling)
                throw new InvalidOperationException("Pool is full and scaling is disabled.");
            ExpandCapacity();
        }

        index = number++;
        var item = items[index];
        if (item == null)
        {
            item = Activator.CreateInstance(typeof(T)) as T;
            items[index] = item;
        }

        item = items[index];
        return item;
    }

    public T ElementAt(int index)
    {
        if (index < 0 || index >= number)
            return default;

        return items[index];
    }

    public bool RemoveAtSwapBack(int index, out int swapIndex)
    {
        swapIndex = -1;
        if (index < 0 || index >= number)
            return false;

        number--;
        if (index != number)
        {
            (items[index], items[number]) = (items[number], items[index]);
            swapIndex = index;
            return true;
        }
        return false;
    }

    private void ExpandCapacity()
    {
        capacity *= 2;
        Array.Resize(ref items, capacity);
    }
}
