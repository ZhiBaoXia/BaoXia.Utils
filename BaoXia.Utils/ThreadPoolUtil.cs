using BaoXia.Utils.Models;
using System.Threading;

namespace BaoXia.Utils;

public class ThreadPoolUtil
{

	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static ThreadPoolInfo GetThreadPoolInfo()
	{
		var threadPoolInfo = new ThreadPoolInfo();

		ThreadPool.GetMaxThreads(out var workerThreadsCountMax, out var completionPortThreadsCountMax);
		threadPoolInfo.SystemWorkerThreadsCountMax = workerThreadsCountMax;
		threadPoolInfo.SystemIOThreadsCountMax = completionPortThreadsCountMax;

		ThreadPool.GetAvailableThreads(out var workerThreadsCount, out var completionPortThreadsCount);
		threadPoolInfo.SystemWorkerThreadsCount = workerThreadsCountMax - workerThreadsCount;
		threadPoolInfo.SystemIOThreadsCount = completionPortThreadsCountMax - completionPortThreadsCount;

		threadPoolInfo.SystemThreadTasksCountWaitingToWork = (int)ThreadPool.PendingWorkItemCount;
		threadPoolInfo.SystemThreadsCountInPool = ThreadPool.ThreadCount;

		return threadPoolInfo;
	}

	#endregion
}
