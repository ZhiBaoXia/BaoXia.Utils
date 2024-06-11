using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class HttpRequestExtensionTest
{
	[TestMethod]
	public void TryGetClientConnectionPortFromIpAddressTest()
	{
		if (HttpRequestExtension.TryGetClientConnectionPortFromIpAddress(
			"127.0.0.1:1001",
			out var clientConnectionPort01))
		{
			Assert.IsTrue(clientConnectionPort01 == 1001);
		}
		else
		{
			Assert.Fail();
		}

		if (HttpRequestExtension.TryGetClientConnectionPortFromIpAddress(
			"127.0.0.1",
			out var clientConnectionPort02))
		{
			Assert.Fail();
		}
		else
		{
			Assert.IsTrue(clientConnectionPort02 == 0);
		}

		if (HttpRequestExtension.TryGetClientConnectionPortFromIpAddress(
			"127.0.0.1,127.0.0.2:1001",
			out var clientConnectionPort03))
		{
			Assert.IsTrue(clientConnectionPort03 == 1001);
		}
		else
		{
			Assert.Fail();
		}

		if (HttpRequestExtension.TryGetClientConnectionPortFromIpAddress(
			"127.0.0.1:1001,127.0.0.2:1002",
			out var clientConnectionPort04))
		{
			Assert.IsTrue(clientConnectionPort04 == 1002);
		}
		else
		{
			Assert.Fail();
		}

		if (HttpRequestExtension.TryGetClientConnectionPortFromIpAddress(
			"127.0.0.1,127.0.0.2:1002,127.0.0.3:1003",
			out var clientConnectionPort05))
		{
			Assert.IsTrue(clientConnectionPort05 == 1003);
		}
		else
		{
			Assert.Fail();
		}
	}
}