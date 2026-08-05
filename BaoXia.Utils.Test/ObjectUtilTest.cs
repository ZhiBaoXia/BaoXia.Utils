using BaoXia.Utils.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test;

[TestClass]
public class ObjectUtilTest
{
	[TestMethod]
	public void GetPropertyPathTest()
	{
		var nestedPropertyPath = ObjectUtil.GetPropertyPath<ObjectPropertyInfo>(
			objectPropertyInfo => objectPropertyInfo.PropertyInfo.Name);
		Assert.AreEqual(
			$"{nameof(ObjectPropertyInfo.PropertyInfo)}.{nameof(System.Reflection.PropertyInfo.Name)}",
			nestedPropertyPath);

		var propertyPath = ObjectUtil.GetPropertyPath<ObjectPropertyInfo>(
			objectPropertyInfo => objectPropertyInfo.Id);
		Assert.AreEqual(
			nameof(ObjectPropertyInfo.Id),
			propertyPath);
	}

	[TestMethod]
	public void GetPropertyPathWithInvalidExpressionTest()
	{
		Assert.ThrowsExactly<ArgumentException>(() =>
			ObjectUtil.GetPropertyPath<ObjectPropertyInfo>(
				objectPropertyInfo => DateTime.MinValue));
		Assert.ThrowsExactly<ArgumentException>(() =>
			ObjectUtil.GetPropertyPath<ObjectPropertyInfo>(
				objectPropertyInfo => objectPropertyInfo.ToString()));
	}
}
