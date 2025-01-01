using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void InitializeWithIndices(this List<int> list, int count)
    {
        list.Clear();
        for (int i = 0; i < count; i++)
        {
            list.Add(i);
        }
    }

    public static int SelectAndRemoveRandomElement(this List<int> list)
    {
        if (list.Count <= 0)
            return -1; // Return -1 to indicate the list is empty.

        int randomIndex = Random.Range(0, list.Count);
        int selectedElement = list[randomIndex];
        list.RemoveAt(randomIndex);

        return selectedElement;
    }
}
