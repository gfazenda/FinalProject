using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {
    
    public static T[] ShuffleArray<T>(T[] array,  int seed)
    {
        System.Random prng = new System.Random(seed);
        for (int i = 0; i < array.Length -1; i++)
        {
            int randomIndex = prng.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }
        return array;
    }
}

public struct Coord
{
    public int x;
    public int y;
    public Coord(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public static bool operator ==(Coord c1, Coord c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(Coord c1, Coord c2)
    {
        return !c1.Equals(c2);
    }
}
