namespace BaoXia.Utils.Interfaces;

public interface INotificationListenParam
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public string? QueueName { get; }

	public string NotificationName { get; }

	public string[]? TagNamesWithIntersection { get; }

	public string[]? TagNamesWithUnion { get; }

	public string? Description { get; set; }

	#endregion
}