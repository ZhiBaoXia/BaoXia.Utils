using BaoXia.Utils.Interfaces;
using System;

namespace BaoXia.Utils.Notification;

public class NotificationListenParam
	<NotificationParamObjectClass>(
	string? queueName,
	string notificationName,
	string[]? tagNamesWithIntersection,
	string[]? tagNamesWithUnion,
	string? description = null)
	: INotificationListenParam
	where NotificationParamObjectClass : class
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public Type ParamObjectType => typeof(NotificationParamObjectClass);

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public NotificationListenParam(
		string? queueName,
		string notificationName)
		: this(queueName,
			  notificationName,
			  null,
			  null,
			  null)
	{ }

	public NotificationListenParam(
		string? queueName,
		string notificationName,
		string? description)
		: this(queueName,
			  notificationName,
			  null,
			  null,
			  description)
	{ }

	public NotificationParamObjectClass? GetParamObjectFrom(object? paramObject)
	{
		return paramObject as NotificationParamObjectClass;
	}

	#endregion

	////////////////////////////////////////////////
	// @实现“INotificationListenParam”
	////////////////////////////////////////////////

	#region 实现“INotificationListenParam”

	public string? QueueName { get; } = queueName;

	public string NotificationName { get; } = notificationName;

	public string[]? TagNamesWithIntersection { get; } = tagNamesWithIntersection;

	public string[]? TagNamesWithUnion { get; } = tagNamesWithUnion;

	public string? Description { get; set; } = description;

	#endregion
}