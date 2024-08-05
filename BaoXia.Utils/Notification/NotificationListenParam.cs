namespace BaoXia.Utils.Notification;

public class NotificationListenParam
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public string? QueueName { get; }

	public string NotificationName { get; }

	public string[]? TagNamesWithIntersection { get; }

	public string[]? TagNamesWithUnion { get; }

	#endregion


	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public NotificationListenParam(
		string? queueName,
		string notificationName,
		string[]? tagNamesWithIntersection,
		string[]? tagNamesWithUnion)
	{
		QueueName = queueName;
		NotificationName = notificationName;
		TagNamesWithIntersection = tagNamesWithIntersection;
		TagNamesWithUnion = tagNamesWithUnion;
	}

	public NotificationListenParam(
		string? queueName,
		string notificationName)
		: this(queueName,
			  notificationName,
			  null,
			  null)
	{ }

	#endregion
}