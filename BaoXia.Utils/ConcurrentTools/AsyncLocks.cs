using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BaoXia.Utils.ConcurrentTools;

public class AsyncLocks<KeyType> where KeyType : notnull
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	private readonly ConcurrentDictionary<KeyType, AsyncLock> _lockers = [];

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public AsyncLock GetLocker(KeyType lockerKey)
	{
		var storeLocker = _lockers.GetOrAdd(lockerKey, new AsyncLock(1));
		{ }
		return storeLocker;
	}

	public async Task<ResultType> LockKeyAsync<ResultType>(
		KeyType key, AsyncLock? lockerGot, Func<AsyncLock?, Task<ResultType>> toExecuteAsync)
	{
		return await AsyncLock.LockAsync<AsyncLock, ResultType>(lockerGot, () => GetLocker(key), toExecuteAsync);
	}

	public async Task LockKeyAsync(
		KeyType key, AsyncLock? lockerGot, Func<AsyncLock?, Task> toExecuteAsync)
	{
		await AsyncLock.LockAsync<AsyncLock>(lockerGot, () => GetLocker(key), toExecuteAsync);
	}

	#endregion
}