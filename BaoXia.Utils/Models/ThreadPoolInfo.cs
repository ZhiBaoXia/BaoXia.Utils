namespace BaoXia.Utils.Models;

public class ThreadPoolInfo
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public int SystemThreadsCountMax
	{
		get
		{
			return this.SystemWorkerThreadsCountMax
			    + this.SystemIOThreadsCountMax;
		}
	}

	public int SystemThreadsCount
	{
		get
		{
			return this.SystemWorkerThreadsCount
			    + this.SystemIOThreadsCount;
		}
	}

	public int SystemWorkerThreadsCountMax { get; set; }

	public int SystemWorkerThreadsCount { get; set; }

	public int SystemIOThreadsCountMax { get; set; }

	public int SystemIOThreadsCount { get; set; }

	public int SystemThreadTasksCountWaitingToWork { get; set; }

	public int SystemThreadsCountInPool { get; set; }

	#endregion
}