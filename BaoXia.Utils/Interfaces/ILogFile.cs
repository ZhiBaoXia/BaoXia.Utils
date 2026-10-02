namespace BaoXia.Utils.Interfaces;

public interface ILogFile
{
	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public void Logs(object invoker, string description, object? infoObject);

	public void FlushLogBuffer(bool isClearBufferOnly = false);

	#endregion
}
