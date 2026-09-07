namespace BaoXia.Utils.Models;

public class TaskIntervalInfoByDurationSeconds
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public double MaxDurationSeconds { get; set; }

	public double TaskIntervalSeconds { get; set; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public TaskIntervalInfoByDurationSeconds()
	{ }

	public TaskIntervalInfoByDurationSeconds(double maxDurationSeconds, double updateIntervalSeconds)
	{
		MaxDurationSeconds = maxDurationSeconds;
		TaskIntervalSeconds = updateIntervalSeconds;
	}

	#endregion
}