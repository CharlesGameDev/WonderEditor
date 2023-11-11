using System;
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
            (float) Math.Round(vector.x * 4, MidpointRounding.ToEven) / grid,
            (float) Math.Round(vector.y * 4, MidpointRounding.ToEven) / grid,
            (float) Math.Round(vector.z * 4, MidpointRounding.ToEven) / grid
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
}
