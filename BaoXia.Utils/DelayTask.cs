using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils;

public class DelayTask : IDisposable
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	protected Timer? _delayTimer;


	protected DateTime _timeToRunPlaned;
	public DateTime TimeToRunPlaned => _timeToRunPlaned;


	#endregion

	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public void RunAfter<TimerParamType>(
		    TimeSpan delay,
		    Action<TimerParamType?> toTimerFired,
		    TimerParamType? timerParam)
		where TimerParamType : class
	{
		_delayTimer?.Dispose();
		_timeToRunPlaned = DateTime.Now.Add(delay);
		_delayTimer = new Timer(
			(timerParam) =>
			{
				toTimerFired(timerParam as TimerParamType);
			},
			timerParam,
			delay,
			Timeout.InfiniteTimeSpan);
	}

	public void RunAfterAsync<TimerParamType>(
			TimeSpan delay,
			Func<TimerParamType?, Task> toTimerFiredAsync,
			TimerParamType? timerParam)
		where TimerParamType : class
	{
		_delayTimer?.Dispose();
		_timeToRunPlaned = DateTime.Now.Add(delay);
		_delayTimer = new Timer(
			async (timerParam) =>
			{
				await toTimerFiredAsync(timerParam as TimerParamType);
			},
			timerParam,
			delay,
			Timeout.InfiniteTimeSpan);
	}

	public void RunAfter(
		    TimeSpan delay,
		    Action toTimerFired)
	{
		RunAfter<object>(
			delay,
			(_) =>
			{
				toTimerFired();
			},
			null);
	}

	public void RunAfterAsync(
		    TimeSpan delay,
		    Func<Task> toTimerFiredAsync)
	{
		RunAfterAsync<object>(
			delay,
			async (_) =>
			{
				await toTimerFiredAsync();
			},
			null);
	}

	public void RunAfter(
		    double delaySeconds,
		    Action toTimerFired)
	{
		RunAfter(
		       TimeSpan.FromSeconds(delaySeconds),
		       toTimerFired);
	}

	public void RunAfterAsync(
		    double delaySeconds,
		    Func<Task> toTimerFiredAsync)
	{
		RunAfterAsync(
		       TimeSpan.FromSeconds(delaySeconds),
		       toTimerFiredAsync);
	}

	public void Cancel()
	{
		_delayTimer?.Dispose();
	}

	#endregion

	////////////////////////////////////////////////
	// @重载
	////////////////////////////////////////////////

	#region 重载

	public void Dispose()
	{
		_delayTimer?.Dispose();

		GC.SuppressFinalize(this);
	}

	#endregion
}
