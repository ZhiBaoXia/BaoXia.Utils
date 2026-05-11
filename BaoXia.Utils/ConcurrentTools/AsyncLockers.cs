using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BaoXia.Utils.ConcurrentTools;

public class AsyncLockers<KeyType> where KeyType : notnull
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	private readonly ConcurrentDictionary<KeyType, AsyncLocker> _lockers = [];

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public AsyncLocker GetLocker(KeyType lockerKey)
	{
		var storeLocker = _lockers.GetOrAdd(lockerKey, new AsyncLocker(1));
		{ }
		return storeLocker;
	}

	public async Task<ResultType> LockKeyAsync<ResultType>(
		KeyType key, AsyncLocker? lockerGot, Func<AsyncLocker?, Task<ResultType>> toExecuteAsync)
	{
		return await AsyncLocker.LockAsync<AsyncLocker, ResultType>(lockerGot, () => GetLocker(key), toExecuteAsync);
	}

	public async Task LockKeyAsync(
		KeyType key, AsyncLocker? lockerGot, Func<AsyncLocker?, Task> toExecuteAsync)
	{
		await AsyncLocker.LockAsync<AsyncLocker>(lockerGot, () => GetLocker(key), toExecuteAsync);
	}

	#endregion
}