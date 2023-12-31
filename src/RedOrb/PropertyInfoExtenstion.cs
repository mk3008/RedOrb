﻿using Carbunql.Values;
using System.Reflection;

namespace RedOrb;

internal static class PropertyInfoExtenstion
{
	public static ParameterValue ToParameterValue<T>(this PropertyInfo prop, T instance, string placeholderIdentifer)
	{
		var value = prop.GetValue(instance);
		var key = placeholderIdentifer + prop.Name;
		return new ParameterValue(key, value);
	}

	public static ParameterValue ToParameterNullValue(this PropertyInfo prop, string placeholderIdentifer)
	{
		var key = placeholderIdentifer + prop.Name;
		return new ParameterValue(key, null);
	}

	public static bool IsNullable(this PropertyInfo prop)
	{
		var proptype = prop.PropertyType;
		var type = Nullable.GetUnderlyingType(proptype);
		return type != null;
	}
}
