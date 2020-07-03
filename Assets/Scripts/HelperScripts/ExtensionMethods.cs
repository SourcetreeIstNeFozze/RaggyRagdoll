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

	/// <summary>
	/// Finds a game object by name no matter where it is in the chierarchy (i.e. not, only children, but also grand children etc)
	/// </summary>
	/// <param name="parent"></param>
	/// <param name="childToFind"></param>
	/// <returns></returns>
	public static Transform FindChild(Transform parent, string childToFind)
	{
		if (parent.name.Equals(childToFind)) return parent;
		foreach (Transform child in parent)
		{
			Transform result = FindChild(child, childToFind);
			if (result != null) return result;
		}
		return null;
	}

	public static Vector3 Vector3To180Spectrum(Vector3 vector)
	{
		return (new Vector3(FloatTo180Spectrum(vector.x), FloatTo180Spectrum(vector.y), FloatTo180Spectrum(vector.z)));
	}
	public static float FloatTo180Spectrum(float value)
	{
		//reduce the value if its bigger than 360
		//value = value - (Mathf.Sign(value) * (value % 360) * 360);

		//convert
		if (value > 180)
		{
			return value - 360;
		}

		if (value < -180)
		{
			return value + 360;
		}
		return value;

	}
}
