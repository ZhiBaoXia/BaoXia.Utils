using BaoXia.Utils.Extensions;
using BaoXia.Utils.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BaoXia.Utils.Test;

[TestClass]
public class TaskIntervalInfosByTaskRunDurationExntesionTest
{
	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	[TestMethod]
	public void GetMinTaskIntervalSecondsTest()
	{
		Assert.IsNull(Array.Empty<TaskIntervalInfoByTaskRunDuration>().GetMinTaskIntervalSeconds());
		Assert.AreEqual(
			1.5,
			new TaskIntervalInfoByTaskRunDuration[]
			{
				new(10.0, 3.0),
				new(30.0, 1.5),
				new(0.0, 60.0)
			}.GetMinTaskIntervalSeconds());
	}

	[TestMethod]
	public void IsTaskIntervalInfosValidWithValidItemsTest()
	{
		var taskIntervalInfos = new TaskIntervalInfoByTaskRunDuration[]
		{
			new(10.0, 0.001),
			new(0.0, 60.0)
		};

		Assert.IsTrue(taskIntervalInfos.IsTaskIntervalInfosValid(0.001, int.MaxValue / 1000.0));
		Assert.IsTrue(
			new TaskIntervalInfoByTaskRunDuration[] { new(0.0, int.MaxValue / 1000.0) }
				.IsTaskIntervalInfosValid(0.001, int.MaxValue / 1000.0));
	}

	[TestMethod]
	public void IsTaskIntervalInfosValidWithInvalidCollectionTest()
	{
		IEnumerable<TaskIntervalInfoByTaskRunDuration> nullTaskIntervalInfos = null!;
		Assert.IsFalse(nullTaskIntervalInfos.IsTaskIntervalInfosValid(0.001, int.MaxValue / 1000.0));
		Assert.IsFalse(Array.Empty<TaskIntervalInfoByTaskRunDuration>().IsTaskIntervalInfosValid(0.001, int.MaxValue / 1000.0));
		Assert.IsFalse(
			new TaskIntervalInfoByTaskRunDuration[] { null! }.IsTaskIntervalInfosValid(0.001, int.MaxValue / 1000.0));
	}

	[TestMethod]
	public void IsTaskIntervalInfosValidWithInvalidItemValuesTest()
	{
		Assert.IsFalse(
			new TaskIntervalInfoByTaskRunDuration[] { new(double.NaN, 1.0) }.IsTaskIntervalInfosValid(0.001, int.MaxValue / 1000.0));
		Assert.IsFalse(
			new TaskIntervalInfoByTaskRunDuration[] { new(double.PositiveInfinity, 1.0) }.IsTaskIntervalInfosValid(0.001, int.MaxValue / 1000.0));
		Assert.IsFalse(
			new TaskIntervalInfoByTaskRunDuration[] { new(0.0, double.NaN) }.IsTaskIntervalInfosValid(0.001, int.MaxValue / 1000.0));
		Assert.IsFalse(
			new TaskIntervalInfoByTaskRunDuration[] { new(0.0, double.PositiveInfinity) }.IsTaskIntervalInfosValid(0.001, int.MaxValue / 1000.0));
		Assert.IsFalse(
			new TaskIntervalInfoByTaskRunDuration[] { new(0.0, 0.0) }.IsTaskIntervalInfosValid(0.001, int.MaxValue / 1000.0));
		Assert.IsFalse(
			new TaskIntervalInfoByTaskRunDuration[] { new(0.0, -1.0) }.IsTaskIntervalInfosValid(0.001, int.MaxValue / 1000.0));
	}

	[TestMethod]
	public void IsTaskIntervalInfosValidWithInvalidUnlimitedDurationTierCountTest()
	{
		Assert.IsFalse(
			new TaskIntervalInfoByTaskRunDuration[] { new(10.0, 1.0) }.IsTaskIntervalInfosValid(0.001, int.MaxValue / 1000.0));
		Assert.IsFalse(
			new TaskIntervalInfoByTaskRunDuration[]
			{
				new(0.0, 1.0),
				new(-1.0, 2.0)
			}.IsTaskIntervalInfosValid(0.001, int.MaxValue / 1000.0));
	}

	[TestMethod]
	public void IsTaskIntervalInfosValidWithMinimumIntervalOutsideSafeRangeTest()
	{
		Assert.IsFalse(
			new TaskIntervalInfoByTaskRunDuration[] { new(0.0, 0.0009) }.IsTaskIntervalInfosValid(0.001, int.MaxValue / 1000.0));
		Assert.IsFalse(
			new TaskIntervalInfoByTaskRunDuration[] { new(0.0, int.MaxValue / 1000.0 + 0.001) }
				.IsTaskIntervalInfosValid(0.001, int.MaxValue / 1000.0));
	}

	[TestMethod]
	public void IsTaskIntervalInfosValidWithInvalidSafeRangeTest()
	{
		var taskIntervalInfos = new TaskIntervalInfoByTaskRunDuration[] { new(0.0, 1.0) };

		Assert.IsFalse(taskIntervalInfos.IsTaskIntervalInfosValid(0.0, 1.0));
		Assert.IsFalse(taskIntervalInfos.IsTaskIntervalInfosValid(2.0, 1.0));
		Assert.IsFalse(taskIntervalInfos.IsTaskIntervalInfosValid(double.NaN, 1.0));
		Assert.IsFalse(taskIntervalInfos.IsTaskIntervalInfosValid(0.001, double.PositiveInfinity));
	}

	#endregion
}
