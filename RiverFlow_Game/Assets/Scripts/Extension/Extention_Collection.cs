using System.Collections;
using System.Collections.Generic;

public static class Extention_Collection
{
    public static bool Contain<T>(this T[] array, T toFind)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Equals(toFind)) { return true; }
        }
        return false;
    }

    public static T Last<T>(this T[] array) { return array[array.Length - 1]; }
    public static T Last<T>(this List<T> list) { return list[list.Count - 1]; }

    public static T FromEnd<T>(this T[] array, int nbr) { return array[array.Length - (1 + nbr)]; }
    public static T FromEnd<T>(this List<T> list, int nbr) { return list[list.Count - (1 + nbr)]; }

    public static T Random<T>(this T[] array) { return array[UnityEngine.Random.Range(0, array.Length)]; }
    public static T Random<T>(this List<T> list) { return list[UnityEngine.Random.Range(0, list.Count)]; }
}
