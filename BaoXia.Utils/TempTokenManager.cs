using BaoXia.Utils.ConcurrentTools;
using BaoXia.Utils.Extensions;
using BaoXia.Utils.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BaoXia.Utils;

public class TempTokenManager
	<TempTokenInfoClass,
	TempTokenCreateParamClass>
	where TempTokenInfoClass : TempTokenInfo, new()
	where TempTokenCreateParamClass : TempTokenCreateParam
{
	////////////////////////////////////////////////
	// @静态常量
	////////////////////////////////////////////////

	#region 静态常量

	public const double TokenCleanIntervalSecondsDefault = 1.0;

	public const int TokenValueLengthDefault = 16;

	#endregion


	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	protected readonly ConcurrentDictionary<string, TempTokenInfoClass> _tokenInfes;

	protected readonly SemaphoreSlim _tokenInfesLocker;

	protected readonly LoopTask _taskToCleanInfes;

	public string Name { get; set; }

	public Func<double> ToGetTempTokenLiveSecondsMax { get; set; }

	public Func<int>? ToGetTokenValueLength { get; set; }

	public int Count => _tokenInfes.Count;

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public TempTokenManager
		(string name,
		Func<double> toGetTempTokenLiveSecondsMax,
		Func<int>? toGetTokenValueLength,
		Func<double>? toGetTokenCleanIntervalSeconds = null)
	{
		_tokenInfes = new();
		_tokenInfesLocker = new SemaphoreSlim(1);

		_taskToCleanInfes = new(
		async (cancellationToken) =>
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return true;
			}

			// !!!
			await CleanInvalidTokensAsync(DateTimeOffset.Now);
			// !!!

			return true;
		},
		() =>
		{
			var tokenCleanIntervalSeconds = toGetTokenCleanIntervalSeconds?.Invoke()
				?? TokenCleanIntervalSecondsDefault;
			if (tokenCleanIntervalSeconds <= 0)
			{
				tokenCleanIntervalSeconds = TokenCleanIntervalSecondsDefault;
			}
			return tokenCleanIntervalSeconds;
		});


		Name = name;
		ToGetTempTokenLiveSecondsMax = toGetTempTokenLiveSecondsMax;
		ToGetTokenValueLength = toGetTokenValueLength;
	}

	public async Task<TempTokenInfoClass> CreateTokenInfoAsync(
		TempTokenCreateParamClass tokenCreateParam,
		DateTimeOffset createTime)
	{
		var tokenInfo
			= await AsyncLocker.LockAsync(
			_tokenInfesLocker,
			null,
			async (_) =>
			{
				TempTokenInfoClass tokenInfo;
				while (true)
				{
					tokenInfo = await DidCreateTempTokenInfoAsync(tokenCreateParam, createTime);
					if (!_tokenInfes.ContainsKey(tokenInfo.TokenValue))
					{
						break;
					}
				}
				////////////////////////////////////////////////
				// !!!
				_tokenInfes.AddOrSet(
					tokenInfo.TokenValue,
					tokenInfo);
				// !!!
				////////////////////////////////////////////////
				return tokenInfo;
			});
		return tokenInfo;
	}

	public async Task<string> CreateTokenAsync(
		TempTokenCreateParamClass tokenCreateParam,
		DateTimeOffset createTime)
	{
		var tokenInfo = await CreateTokenInfoAsync(
			tokenCreateParam, createTime);
		{ }
		return tokenInfo.TokenValue;
	}

	public bool TryGetTokenInfo(
		string? tokenValue,
		out TempTokenInfoClass? tokenInfo)
	{
		//
		tokenInfo = null;
		//
		if (string.IsNullOrWhiteSpace(tokenValue))
		{
			// !!!
			return false;
			// !!!
		}

		if (_tokenInfes.TryGetValue(
			tokenValue,
			out var tokenInfoExisted))
		{
			// !!!
			tokenInfo = tokenInfoExisted;
			return true;
			// !!!
		}
		return false;
	}

	public TempTokenInfoClass? GetTokenInfo(
			string? tokenValue)
	{
		_ = TryGetTokenInfo(
			tokenValue,
			out var tokenInfo);
		{ }
		return tokenInfo;
	}

	public async Task<TempTokenInfoClass?> GetValidTokenInfoAsync(
		string? tokenValue,
		ClientIpInfo clientIpInfo,
		DateTimeOffset? checkTime = null)
	{
		_ = TryGetTokenInfo(
			tokenValue,
			out var tokenInfo);
		{ }
		var isTokenInfoValid = await IsTokenInfoValidAsync(
			tokenInfo,
			clientIpInfo,
			checkTime);
		if (isTokenInfoValid)
		{
			return tokenInfo;
		}
		return null;
	}

	public List<TempTokenInfoClass>? GetInvalidTokenInfesAt(DateTimeOffset checkTime)
	{
		List<TempTokenInfoClass>? invalidTokenInfes = null;
		var tokenInfoLiveSecondsMax = ToGetTempTokenLiveSecondsMax();
		foreach (var tokenInfo in _tokenInfes.Values)
		{
			var tokenLiveSeconds = (checkTime - tokenInfo.CreateTime).TotalSeconds;
			if (tokenLiveSeconds > tokenInfoLiveSecondsMax)
			{
				// !!!
				invalidTokenInfes ??= [];
				invalidTokenInfes.Add(tokenInfo);
				// !!!
			}
		}
		return invalidTokenInfes;
	}

	public async Task<bool> IsTokenInfoValidAsync(
		TempTokenInfoClass? tokenInfo,
		ClientIpInfo clientIpInfo,
		DateTimeOffset? checkTime = null)
	{
		if (tokenInfo == null)
		{
			return false;
		}

		checkTime ??= DateTimeOffset.Now;
		var isTokenInfoValid = await DidIsTokenInfoValidAsync(
			tokenInfo,
			clientIpInfo,
			checkTime.Value);
		{ }
		return isTokenInfoValid;
	}

	public async Task<bool> IsTokenValidAsync(
		string? tokenValue,
		ClientIpInfo clientIpInfo,
		DateTimeOffset? checkTime = null)
	{
		if (string.IsNullOrWhiteSpace(tokenValue))
		{
			return false;
		}
		if (!_tokenInfes.TryGetValue(tokenValue, out var tokenInfo))
		{
			return false;
		}
		return await IsTokenInfoValidAsync(
			tokenInfo,
			clientIpInfo,
			checkTime);
	}

	public async Task CleanInvalidTokensAsync(DateTimeOffset? checkTime = null)
	{
		await DidCleanInvalidTokensAsync(
			checkTime
			?? DateTimeOffset.Now);
	}

	#endregion


	////////////////////////////////////////////////
	// @事件节点
	////////////////////////////////////////////////

	#region 事件节点
	protected virtual async Task<TempTokenInfoClass> DidCreateTempTokenInfoAsync(
		TempTokenCreateParamClass tokenCreateParam,
		DateTimeOffset createTime)
	{
		var tokenValue = await DidGenerateTokenValueAsync(
			tokenCreateParam, createTime);
		var tokenInfo = new TempTokenInfoClass()
		{
			TokenValue = tokenValue,
			ClientIpInfo = tokenCreateParam.ClientIpInfo,
			CreateTime = createTime
		};
		return tokenInfo;
	}

	protected virtual async Task<string> DidGenerateTokenValueAsync(
		TempTokenCreateParamClass tokenCreateParam,
		DateTimeOffset createTime)
	{
		var tokenValueLength = ToGetTokenValueLength?.Invoke() ?? TokenValueLengthDefault;
		if (tokenValueLength <= 0)
		{
			tokenValueLength = TokenValueLengthDefault;
		}

		var tokenValue = StringUtil.RandomStringInLength(tokenValueLength);
		{ }
		return await Task.FromResult(tokenValue);
	}

	protected virtual async Task<bool> DidIsTokenInfoValidAsync(
		TempTokenInfoClass tokenInfo,
		ClientIpInfo clientIpInfo,
		DateTimeOffset checkTime)
	{
		var tokenLiveSecondsMax = ToGetTempTokenLiveSecondsMax();
		if (tokenLiveSecondsMax <= 0)
		{
			return true;
		}
		var tokenLiveSeconds = (checkTime - tokenInfo.CreateTime).TotalSeconds;
		if (tokenLiveSeconds <= tokenLiveSecondsMax)
		{
			return true;
		}
		return await Task.FromResult(false);
	}

	protected virtual async Task DidCleanInvalidTokensAsync(DateTimeOffset checkTime)
	{
		var tokenLiveSecondsMax = ToGetTempTokenLiveSecondsMax();
		if (tokenLiveSecondsMax <= 0)
		{
			return;
		}
		var invalidTokenInfes = GetInvalidTokenInfesAt(checkTime);
		if (invalidTokenInfes == null)
		{
			return;
		}

		foreach (var invalidTokenInfo
			in
			invalidTokenInfes)
		{
			_tokenInfes.Remove(
				invalidTokenInfo.TokenValue,
				out _);
		}

		await Task.CompletedTask;
	}

	#endregion
}