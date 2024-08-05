using BaoXia.Utils.Notification;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Interfaces;

using Notification = BaoXia.Utils.Notification.Notification;

public interface INotificationCenter
{
	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public void PostNotification(
		string? queueName,
		string notificationName,
		IEnumerable<string>? tagNames = null,
		string? description = null,
		Dictionary<string, object>? paramDictionary = null,
		object? paramObject = null,
		string? senderName = null,
		double sendDelaySeconds = 0.0,
		//
		Action<List<Object>?>? toNotificationSended = null,
		Func<List<Object>?, Task>? toNotificationSendedAsync = null);

	public void PostNotification(
		string notificationName,
		IEnumerable<string>? tagNames = null,
		string? description = null,
		//
		Dictionary<string, object>? paramDictionary = null,
		object? paramObject = null,
		string? senderName = null,
		double sendDelaySeconds = 0.0,
		//
		Action<List<Object>?>? toNotificationSended = null,
		Func<List<Object>?, Task>? toNotificationSendedAsync = null);

	public void Post(
		NotificationListenParam listenParam,
		object? paramObject,
		object sender);

	public NotificationListener ListenNotification(
		string? queueName,
		//
		string notificationName,
		IEnumerable<string>? tagNamesWithIntersection,
		IEnumerable<string>? tagNamesWithUnion,
		//
		Func<Notification, CancellationToken, object?>? toDidReceivedNotification,
		Func<Notification, CancellationToken, Task<object?>>? toDidReceivedNotificationAsync);

	public NotificationListener ListenNotification(
		string queueName,
		//
		string notificationName,
		IEnumerable<string>? tagNamesWithIntersection,
		IEnumerable<string>? tagNamesWithUnion,
		//
		Func<Notification, CancellationToken, List<Object>?> toDidReceivedNotification);

	public NotificationListener ListenNotificationAsync(
		string? queueName,
		//
		string notificationName,
		IEnumerable<string>? tagNamesWithIntersection,
		IEnumerable<string>? tagNamesWithUnion,
		//
		Func<Notification, CancellationToken, Task<object?>> toDidReceivedNotificationAsync);

	public NotificationListener ListenNotification(
		string notificationName,
		IEnumerable<string>? tagNamesWithIntersection,
		IEnumerable<string>? tagNamesWithUnion,
		//
		Func<Notification, CancellationToken, object?>? toDidReceivedNotification,
		Func<Notification, CancellationToken, Task<object?>>? toDidReceivedNotificationAsync);

	public NotificationListener ListenNotification(
		string notificationName,
		IEnumerable<string>? tagNamesWithIntersection,
		IEnumerable<string>? tagNamesWithUnion,
		//
		Func<Notification, CancellationToken, Object?> toDidReceivedNotification);

	public NotificationListener ListenNotificationAsync(
		string notificationName,
		IEnumerable<string>? tagNamesWithIntersection,
		IEnumerable<string>? tagNamesWithUnion,
		//
		Func<Notification, CancellationToken, Task<object?>> toDidReceivedNotificationAsync);

	public NotificationListener Listen(
		NotificationListenParam listenParam,
		Func<Notification, CancellationToken, object?>? toDidReceivedNotification);

	public List<NotificationListener> Listen(
		IEnumerable<NotificationListenParam> listenParams,
		Func<Notification, CancellationToken, object?>? toDidReceivedNotification);
	public List<NotificationListener> ListenTo(
		Func<Notification, CancellationToken, object?>? toDidReceivedNotification,
		params NotificationListenParam[] listenParams);

	public NotificationListener ListenAsync(
		NotificationListenParam listenParam,
		Func<Notification, CancellationToken, Task<object?>>? toDidReceivedNotificationAsync);

	public List<NotificationListener> ListenAsync(
		IEnumerable<NotificationListenParam> listenParams,
		Func<Notification, CancellationToken, Task<object?>>? toDidReceivedNotificationAsync);

	public List<NotificationListener> ListenToAsync(
		Func<Notification, CancellationToken, Task<object?>>? toDidReceivedNotificationAsync,
		params NotificationListenParam[] listenParams);

	public bool CancelListenWithListener(NotificationListener listener);

	#endregion
}