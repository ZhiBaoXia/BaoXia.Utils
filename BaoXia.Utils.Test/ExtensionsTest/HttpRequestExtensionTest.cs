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
			Assert.AreEqual(1001, clientConnectionPort01);
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
			Assert.AreEqual(0, clientConnectionPort02);
		}

		if (HttpRequestExtension.TryGetClientConnectionPortFromIpAddress(
			"127.0.0.1,127.0.0.2:1001",
			out var clientConnectionPort03))
		{
			Assert.AreEqual(1001, clientConnectionPort03);
		}
		else
		{
			Assert.Fail();
		}

		if (HttpRequestExtension.TryGetClientConnectionPortFromIpAddress(
			"127.0.0.1:1001,127.0.0.2:1002",
			out var clientConnectionPort04))
		{
			Assert.AreEqual(1002, clientConnectionPort04);
		}
		else
		{
			Assert.Fail();
		}

		if (HttpRequestExtension.TryGetClientConnectionPortFromIpAddress(
			"127.0.0.1,127.0.0.2:1002,127.0.0.3:1003",
			out var clientConnectionPort05))
		{
			Assert.AreEqual(1003, clientConnectionPort05);
		}
		else
		{
			Assert.Fail();
		}
	}
}