﻿using System;
using System.Collections.Generic;

namespace PokerLib;

public static class ExtensionMethods
{
    private static readonly Random rng = new();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    public static T Pop<T>(this IList<T> list)
    {
        var item = list[0];
        list.RemoveAt(0);
        return item;
    }

    public static List<T> Pop<T>(this IList<T> list, int count)
    {
        var items = new List<T>();
        for (int i = 0; i < count; i++)
        {
            items.Add(list.Pop());
        }
        return items;
    }
}
