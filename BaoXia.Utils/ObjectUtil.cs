using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace BaoXia.Utils;

public static class ObjectUtil
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static string GetPropertyPath<ObjectType>(Expression<Func<ObjectType, object?>> propertyExpression)
	{
		//ArgumentNullException.ThrowIfNull(propertyExpression);

		var propertyNames = new Stack<string>();
		Expression? currentExpression = propertyExpression.Body;
		if (currentExpression is UnaryExpression unaryExpression
			&& (unaryExpression.NodeType == ExpressionType.Convert
				|| unaryExpression.NodeType == ExpressionType.ConvertChecked))
		{
			currentExpression = unaryExpression.Operand;
		}

		while (currentExpression is MemberExpression memberExpression
			&& memberExpression.Member is PropertyInfo propertyInfo)
		{
			propertyNames.Push(propertyInfo.Name);
			currentExpression = memberExpression.Expression;
		}

		if (propertyNames.Count < 1
			|| currentExpression != propertyExpression.Parameters[0])
		{
			throw new ArgumentException(
				"表达式必须是从参数开始的属性访问路径。",
				nameof(propertyExpression));
		}

		return string.Join('.', propertyNames);
	}

	public static object? CreateObject(Type objectType)
	{
		if (objectType.Equals(typeof(string)))
		{
			return string.Empty;
		}
		var @object = Activator.CreateInstance(objectType);
		{ }
		return @object;
	}

	public static ObjectType? CreateObject<ObjectType>()
	{
		return (ObjectType?)CreateObject(typeof(ObjectType));
	}

	public static object? CreateObject(object? @object)
	{
		if (@object == null)
		{
			return @object;
		}
		if (@object is string stringObject)
		{
			return new string(stringObject);
		}
		{
			@object = Activator.CreateInstance(@object.GetType());
		}
		return @object;
	}


	#endregion
}
