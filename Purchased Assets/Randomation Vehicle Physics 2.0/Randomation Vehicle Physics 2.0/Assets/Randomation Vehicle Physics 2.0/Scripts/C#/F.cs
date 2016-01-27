﻿using UnityEngine;
using System.Collections;

//Static class with extra functions
public static class F
{
	//Same as Mathf.Pow, but only excepts natural numbers (positive integers) as the exponent
	//It only remains for legacy support, it is slower than Mathf.Pow
	public static float PowNatural(float n, int exp)
	{
        Debug.LogWarning("F.PowNatural is obsolete and will be removed in a future update.");

		float result = n;
		exp = Mathf.Max(exp, 0);

		if (exp == 0)
		{
			result = 1;
		}
		else if (exp == 2)
		{
			result = n * n;
		}
		else if (exp > 2)
		{
			for (int i = 0; i < exp; i++)
			{
				if (i > 0)
				{
					result *= n;
				}
			}
		}

		return result;
	}

	//Returns the number with the greatest absolute value
	public static float MaxAbs(params float[] nums)
	{
		float result = 0;

		for (int i = 0; i < nums.Length; i++)
		{
			if (Mathf.Abs(nums[i]) > Mathf.Abs(result))
			{
				result = nums[i];
			}
		}

		return result;
	}

	//Returns the topmost parent with a certain component
	public static Component GetTopmostParentComponent<T>(Transform tr) where T : Component
	{
		Component getting = null;

		while (tr.parent != null)
		{
			if (tr.parent.GetComponent<T>() != null)
			{
				getting = tr.parent.GetComponent<T>();
			}

			tr = tr.parent;
		}

		return getting;
	}
}
