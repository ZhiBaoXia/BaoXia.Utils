using BaoXia.Utils.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test;

[TestClass]
public class TempTokenManagerTest
{
	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	[TestMethod]
	public async Task TempTokenManageTest()
	{
		var tokenLiveSecondsMax = 3.0;
		var tokenValueLength = 12;
		var clientIpInfo = new ClientIpInfo("123.456.789.012", 123);
		var tokenManager
			= new TempTokenManager
			<TempTokenInfo, TempTokenCreateParam>
			("测试令牌管理器",
			() => tokenLiveSecondsMax,
			() => tokenValueLength,
			() => 1.0);

		var tokenInfo
			= await tokenManager.CreateTokenInfoAsync(
				new TempTokenCreateParam(clientIpInfo),
				DateTimeOffset.Now);
		{
			// !!!
			Assert.AreEqual(1, tokenManager.Count);
			Assert.AreEqual(tokenValueLength, tokenInfo.TokenValue.Length);
			// !!!
		}
		var isTokenValid
			= await tokenManager.IsTokenValidAsync(
				tokenInfo.TokenValue,
				clientIpInfo,
				DateTimeOffset.Now);
		{
			// !!!
			Assert.IsTrue(isTokenValid);
			// !!!
		}
		isTokenValid
			= await tokenManager.IsTokenValidAsync(
				tokenInfo.TokenValue + "-0",
				clientIpInfo,
				DateTimeOffset.Now);
		{
			// !!!
			Assert.IsFalse(isTokenValid);
			// !!!
		}

		// !!!
		await Task.Delay(1000 * (int)(tokenLiveSecondsMax + 1.0), CancellationToken.None);
		// !!!

		{
			// !!!
			Assert.AreEqual(0, tokenManager.Count);
			// !!!
		}
		isTokenValid
			= await tokenManager.IsTokenValidAsync(
				tokenInfo.TokenValue,
				clientIpInfo,
				DateTimeOffset.Now);
		{
			// !!!
			Assert.IsFalse(isTokenValid);
			// !!!
		}
	}

	#endregion
}
