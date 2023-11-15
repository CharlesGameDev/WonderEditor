using System;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static Vector3 ToVector3(this float[] array)
    {
        return new Vector3()
        {
            x = array[0],
            y = array[1],
            z = array[2]
        };
    }

    public static float[] RadToDeg(this float[] array)
    {
        float[] degrees = new float[array.Length];
        for (int i = 0; i < array.Length; i++)
            degrees[i] = Mathf.Rad2Deg * array[i];

        return degrees;
    }

    public static Quaternion ToRotation(this float[] array)
    {
        return Quaternion.Euler
        (
            (float)(Mathf.Rad2Deg * array[0]),
            (float)(Mathf.Rad2Deg * array[1]),
            (float)(Mathf.Rad2Deg * array[2])
        );
    }

    public static Vector3 PutOnGrid(this Vector3 vector, float grid)
    {
        return new Vector3(
            (float) Math.Round(vector.x * grid, MidpointRounding.ToEven) / grid,
            (float) Math.Round(vector.y * grid, MidpointRounding.ToEven) / grid,
            (float) Math.Round(vector.z * grid, MidpointRounding.ToEven) / grid
        );
    }

    public static Vector3 RoundToInt(this Vector3 vector)
    {
        return new Vector3(
            Mathf.RoundToInt(vector.x),
            Mathf.RoundToInt(vector.y),
            Mathf.RoundToInt(vector.z)
        );
    }

    public static float[] ToArray(this Vector3 vector)
    {
        return new float[3]
        {
            vector.x,
            vector.y,
            vector.z
        };
    }

    public static Vector3 ToRadians(this Vector3 vector)
    {
        return new Vector3(
            Mathf.Deg2Rad * vector.x,
            Mathf.Deg2Rad * vector.y,
            Mathf.Deg2Rad * vector.z
        );
    }

    public static float[] ToArrayRad(this Quaternion q)
    {
        return q.eulerAngles.ToRadians().ToArray();
    }

    public static bool ChangeKey<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                               TKey oldKey, TKey newKey)
    {
        if (!dict.Remove(oldKey, out TValue value))
            return false;

        dict[newKey] = value;
        return true;
    }
}
