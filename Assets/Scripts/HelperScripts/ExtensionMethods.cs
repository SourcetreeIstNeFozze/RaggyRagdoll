using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static float Remap (this float value, float valueRangeMin, float valueRangeMax, float newRangeMin, float newRangeMax)
    {
        return (value - valueRangeMin) / (valueRangeMax - valueRangeMin) * (newRangeMax - newRangeMin) + newRangeMin;
    }

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

	public static T GetRandomElement<T>(List<T> list)
	{
		int randomIndex = Random.Range(0, list.Count);
		return list[randomIndex];
	}

	public static T GetRandomElement<T>(T[] array)
	{
		int randomIndex = Random.Range(0, array.Length);
		return array[randomIndex];
	}
}
