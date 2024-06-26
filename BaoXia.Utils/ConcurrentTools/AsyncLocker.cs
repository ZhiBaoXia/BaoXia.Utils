using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.ConcurrentTools;

public class AsyncLocker
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static async Task<ResultType> LockAsync<SemaphoreSlimType, ResultType>(
		SemaphoreSlimType? lockerGot,
		Func<SemaphoreSlimType?>? toGetLocker,
		Func<SemaphoreSlimType?, Task<ResultType>> toExecuteAsync)
		where SemaphoreSlimType : SemaphoreSlim
	{
		var isLockerNeedRelease = false;
		if (lockerGot == null)
		{
			// !!!
			lockerGot = toGetLocker?.Invoke();
			if (lockerGot != null)
			{
				await lockerGot.WaitAsync();
				isLockerNeedRelease = true;
				// !!!
			}
		}
		try
		{
			var result = await toExecuteAsync(lockerGot);
			{ }
			return result;
		}
		finally
		{
			if (isLockerNeedRelease)
			{
				// !!!
				lockerGot?.Release();
				//lockerGot = null;
				//isLockerNeedRelease = false;
				// !!!
			}
		}
	}

	public static async Task LockAsync<SemaphoreSlimType>(
		SemaphoreSlimType? lockerGot,
		Func<SemaphoreSlimType?>? toGetLocker,
		Func<SemaphoreSlimType?, Task> toExecuteAsync)
		where SemaphoreSlimType : SemaphoreSlim
	{
		var isLockerNeedRelease = false;
		if (lockerGot == null)
		{
			// !!!
			lockerGot = toGetLocker?.Invoke();
			if (lockerGot != null)
			{
				await lockerGot.WaitAsync();
				isLockerNeedRelease = true;
				// !!!
			}
			// !!!
		}
		try
		{
			await toExecuteAsync(lockerGot);
		}
		finally
		{
			if (isLockerNeedRelease)
			{
				// !!!
				lockerGot?.Release();
				//lockerGot = null;
				//isLockerNeedRelease = false;
				// !!!
			}
		}
	}

	#endregion
}