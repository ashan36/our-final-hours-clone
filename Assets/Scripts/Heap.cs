﻿using System;
using System.Collections.Generic;


public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int ItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public int Count
    {
        get { return ItemCount; }
    }

    public void Add(T item)
    {
        item.HeapIndex = ItemCount;
        items[ItemCount] = item;
        SortUp(item);
        ItemCount++;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
        SortDown(item);
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        ItemCount--;
        items[0] = items[ItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < ItemCount)
            {
                swapIndex = childIndexLeft;
                if (childIndexRight < ItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                if (item.CompareTo(items[swapIndex]) < 0)
                    Swap(item, items[swapIndex]);
                else return;
            }
            else return;
        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else break;
        }

        parentIndex = (item.HeapIndex - 1) / 2;
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        int tempIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = tempIndex;
    }

}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}