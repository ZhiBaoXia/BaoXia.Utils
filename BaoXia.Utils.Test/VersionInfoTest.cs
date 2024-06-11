using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test;

[TestClass]
public class VersionInfoTest
{

	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	[TestMethod]
	public void CompareToTest()
	{
		var version_1_0_0 = new VersionInfo { VersionString = "1.0.0" };
		var version_1_0_0b = new VersionInfo { VersionString = "1.0.0" };
		var version_1_0_1 = new VersionInfo { VersionString = "1.0.1" };
		var version_1_1_0 = new VersionInfo { VersionString = "1.1.0" };
		var version_1_1_1 = new VersionInfo { VersionString = "1.1.1" };
		var version_2_1_1 = new VersionInfo { VersionString = "2.1.1" };
		var version_1_0 = new VersionInfo { VersionString = "1.0" };
		var version_1_0_0_0 = new VersionInfo { VersionString = "1.0.0.0" };

#pragma warning disable CS1718 // 对同一变量进行了比较
		Assert.IsTrue(version_1_0_0 == version_1_0_0);
#pragma warning restore CS1718 // 对同一变量进行了比较
		Assert.IsTrue(version_1_0_0 == version_1_0_0b);
		Assert.IsTrue(version_1_0_0 < version_1_0_1);
		Assert.IsTrue(version_1_0_0 < version_1_1_0);
		Assert.IsTrue(version_1_0_0 < version_1_1_1);
		Assert.IsTrue(version_1_0_0 < version_2_1_1);
		Assert.IsTrue(version_1_0_0 > version_1_0);
		Assert.IsTrue(version_1_0_0 < version_1_0_0_0);

#pragma warning disable CS1718 // 对同一变量进行了比较
		Assert.IsTrue(version_1_0_0 == version_1_0_0);
#pragma warning restore CS1718 // 对同一变量进行了比较
		Assert.IsTrue(version_1_0_0 == version_1_0_0b);
		Assert.IsTrue(version_1_0_0 <= version_1_0_1);
		Assert.IsTrue(version_1_0_0 <= version_1_1_0);
		Assert.IsTrue(version_1_0_0 <= version_1_1_1);
		Assert.IsTrue(version_1_0_0 <= version_2_1_1);
		Assert.IsTrue(version_1_0_0 >= version_1_0);
		Assert.IsTrue(version_1_0_0 <= version_1_0_0_0);

		Assert.IsTrue(version_1_1_1 > version_1_0_0b);
		Assert.IsTrue(version_1_1_1 > version_1_0_1);
		Assert.IsTrue(version_1_1_1 > version_1_1_0);
#pragma warning disable CS1718 // 对同一变量进行了比较
		Assert.IsTrue(version_1_1_1 == version_1_1_1);
#pragma warning restore CS1718 // 对同一变量进行了比较
		Assert.IsTrue(version_1_1_1 < version_2_1_1);
		Assert.IsTrue(version_1_1_1 > version_1_0);
		Assert.IsTrue(version_1_1_1 < version_1_0_0_0);


		Assert.IsTrue(version_1_1_1 >= version_1_0_0b);
		Assert.IsTrue(version_1_1_1 >= version_1_0_1);
		Assert.IsTrue(version_1_1_1 >= version_1_1_0);
#pragma warning disable CS1718 // 对同一变量进行了比较
		Assert.IsTrue(version_1_1_1 == version_1_1_1);
#pragma warning restore CS1718 // 对同一变量进行了比较
		Assert.IsTrue(version_1_1_1 <= version_2_1_1);
		Assert.IsTrue(version_1_1_1 >= version_1_0);
		Assert.IsTrue(version_1_1_1 <= version_1_0_0_0);
	}

	#endregion
}
