using BaoXia.Utils.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Notification;

/// <summary>
/// 通知中心。
/// </summary>
public class NotificationCenter : INotificationCenter
{
	////////////////////////////////////////////////
	// @静态常量
	////////////////////////////////////////////////

	#region 静态常量

	public const double NotificationQueue_PeekNotificationIntervalSecondsMaxDefault = 0.1;

	public const int NotificationQueue_SendNotificationConcurrentCountMaxDefault = 0;

	public const double NotificationQueue_SendNotificationTimeoutSecondsMaxDefault = 1.0;

	#endregion


	////////////////////////////////////////////////
	// @静态变量
	////////////////////////////////////////////////

	#region 静态变量

	public string? Name { get; set; }

	public double NotificationQueuePeekNotificationIntervalSecondsMax { get; set; }
		= NotificationQueue_PeekNotificationIntervalSecondsMaxDefault;
	public int NotificationQueueSendNotificationConcurrentCountMax { get; set; }
		= NotificationQueue_SendNotificationConcurrentCountMaxDefault;
	public double NotificationQueueSendNotificationTimeoutSecondsMax { get; set; }
		= NotificationQueue_SendNotificationTimeoutSecondsMaxDefault;

	public Func<String?, NotificationQueue>? ToCreateNotificationQueue { get; set; }

	protected ConcurrentDictionary<string, NotificationQueue> _notificationQueues = new();

	protected NotificationQueue? _notificationQueueUndefined;

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public NotificationCenter()
	{ }

	public NotificationCenter(
		string name,
		Func<String?, NotificationQueue>? toCreateNotificationQueue)
	{
		this.Name = name;
		this.ToCreateNotificationQueue = toCreateNotificationQueue;
	}

	public NotificationQueue CreateNotificationQueue(string? queueName)
	{
		var toCreateNotificationQueue = this.ToCreateNotificationQueue;
		if (toCreateNotificationQueue != null)
		{
			return toCreateNotificationQueue(queueName);
		}
		return this.DidCreateNotificationQueue(queueName);
	}

	public NotificationQueue GetOrCreateNotificationQueue(string? queueName)
	{
		NotificationQueue? notificationQueue = null;
		////////////////////////////////////////////////
		// 1/2，指定队列的消息处理方式：
		////////////////////////////////////////////////
		if (queueName?.Length > 0)
		{
			notificationQueue = _notificationQueues.GetOrAdd(
				queueName,
				(string queueNameInQueues) =>
				{
					return this.CreateNotificationQueue(queueNameInQueues);
				});
		}
		////////////////////////////////////////////////
		// 2/2，未指定队列的消息处理方式：
		////////////////////////////////////////////////
		else
		{
			if (_notificationQueueUndefined == null)
			{
				lock (this)
				{
					_notificationQueueUndefined ??= this.CreateNotificationQueue(queueName);
				}
			}
			notificationQueue = _notificationQueueUndefined;
		}
		if (notificationQueue == null)
		{
			throw new ArgumentNullException("无法创建名为“" + queueName + "”的消息队列。");
		}
		return notificationQueue;
	}

	#endregion


	////////////////////////////////////////////////
	// @实现“INotificationCenter”
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
		Func<List<Object>?, Task>? toNotificationSendedAsync = null)
	{
		var notification = new Notification(
			queueName,
			notificationName,
			tagNames,
			//
			description,
			paramDictionary,
			paramObject,
			//
			senderName,
			DateTime.Now,
			sendDelaySeconds,
			//
			toNotificationSended,
			toNotificationSendedAsync);

		////////////////////////////////////////////////
		// 获取指定的消息队列：
		////////////////////////////////////////////////
		var notificationQueue
			= this.GetOrCreateNotificationQueue(queueName);

		// !!!
		notificationQueue.Enqueue(notification);
		notificationQueue.StartProcessNotificationQueue();
		// !!!
	}

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
		Func<List<Object>?, Task>? toNotificationSendedAsync = null)
	{
		this.PostNotification(
		       null,
		       notificationName,
			tagNames,
			description,
			paramDictionary,
			paramObject,
			senderName,
			sendDelaySeconds,
			//
			toNotificationSended,
			toNotificationSendedAsync);
	}

	public void Post(
		INotificationListenParam listenParam,
		object? paramObject,
		object sender)
	{
		if (sender is not string senderName)
		{
			senderName = sender?.GetType().FullName ?? string.Empty;
		}
		PostNotification(
			listenParam.QueueName,
			listenParam.NotificationName,
			null,
			null,
			null,
			paramObject,
			senderName);
	}

	public NotificationListener ListenNotification(
		string? queueName,
		//
		string notificationName,
		IEnumerable<string>? tagNamesWithIntersection,
		IEnumerable<string>? tagNamesWithUnion,
		//
		Func<Notification, CancellationToken, object?>? toDidReceivedNotification,
		Func<Notification, CancellationToken, Task<object?>>? toDidReceivedNotificationAsync)
	{
		////////////////////////////////////////////////
		// 获取指定的消息队列：
		////////////////////////////////////////////////
		var notificationQueue
			= this.GetOrCreateNotificationQueue(queueName);
		var listener = notificationQueue.RegisterListenerToListenNotificationName(
				notificationName,
				//
				tagNamesWithIntersection,
				tagNamesWithUnion,
			       //
			       toDidReceivedNotification,
			       toDidReceivedNotificationAsync);
		{ }
		return listener;
	}

	public NotificationListener ListenNotification(
		string queueName,
		//
		string notificationName,
		IEnumerable<string>? tagNamesWithIntersection,
		IEnumerable<string>? tagNamesWithUnion,
		//
		Func<Notification, CancellationToken, List<Object>?> toDidReceivedNotification)
	{
		return this.ListenNotification(
			queueName,
			//
			notificationName,
			tagNamesWithIntersection,
			tagNamesWithUnion,
			//
			toDidReceivedNotification,
			null);
	}

	public NotificationListener ListenNotificationAsync(
		string? queueName,
		//
		string notificationName,
		IEnumerable<string>? tagNamesWithIntersection,
		IEnumerable<string>? tagNamesWithUnion,
		//
		Func<Notification, CancellationToken, Task<object?>> toDidReceivedNotificationAsync)
	{
		return this.ListenNotification(
			queueName,
			//
			notificationName,
			tagNamesWithIntersection,
			tagNamesWithUnion,
			//
			null,
			toDidReceivedNotificationAsync);
	}

	public NotificationListener ListenNotification(
		string notificationName,
		IEnumerable<string>? tagNamesWithIntersection,
		IEnumerable<string>? tagNamesWithUnion,
		//
		Func<Notification, CancellationToken, object?>? toDidReceivedNotification,
		Func<Notification, CancellationToken, Task<object?>>? toDidReceivedNotificationAsync)
	{
		return this.ListenNotification(
			null,
			//
			notificationName,
			tagNamesWithIntersection,
			tagNamesWithUnion,
			//
			toDidReceivedNotification,
			toDidReceivedNotificationAsync);
	}

	public NotificationListener ListenNotification(
		string notificationName,
		IEnumerable<string>? tagNamesWithIntersection,
		IEnumerable<string>? tagNamesWithUnion,
		//
		Func<Notification, CancellationToken, Object?> toDidReceivedNotification)
	{
		return this.ListenNotification(
			notificationName,
			tagNamesWithIntersection,
			tagNamesWithUnion,
			//
			toDidReceivedNotification,
			null);
	}

	public NotificationListener ListenNotificationAsync(
		string notificationName,
		IEnumerable<string>? tagNamesWithIntersection,
		IEnumerable<string>? tagNamesWithUnion,
		//
		Func<Notification, CancellationToken, Task<object?>> toDidReceivedNotificationAsync)
	{
		return this.ListenNotification(
			notificationName,
			tagNamesWithIntersection,
			tagNamesWithUnion,
			//
			null,
			toDidReceivedNotificationAsync);
	}


	public NotificationListener Listen(
		INotificationListenParam listenParam,
		Func<Notification, CancellationToken, object?>? toDidReceivedNotification)
	{
		return ListenNotification(
			listenParam.QueueName,
			listenParam.NotificationName,
			listenParam.TagNamesWithIntersection,
			listenParam.TagNamesWithUnion,
			toDidReceivedNotification,
			null);
	}

	public List<NotificationListener> Listen(
		IEnumerable<INotificationListenParam> listenParams,
		Func<Notification, CancellationToken, object?>? toDidReceivedNotification)
	{
		var notificationListeners = new List<NotificationListener>();
		foreach (var listenParam in listenParams)
		{
			var notificationListener
				= Listen(
					listenParam,
					toDidReceivedNotification);
			// !!!
			notificationListeners.Add(notificationListener);
			// !!!
		}
		return notificationListeners;
	}

	public List<NotificationListener> ListenTo(
		Func<Notification, CancellationToken, object?>? toDidReceivedNotification,
		params INotificationListenParam[] listenParams)
	{
		var notificationListeners = new List<NotificationListener>();
		foreach (var listenParam in listenParams)
		{
			var notificationListener
				= Listen(
					listenParam,
					toDidReceivedNotification);
			// !!!
			notificationListeners.Add(notificationListener);
			// !!!
		}
		return notificationListeners;
	}

	public NotificationListener ListenAsync(
		INotificationListenParam listenParam,
		Func<Notification, CancellationToken, Task<object?>>? toDidReceivedNotificationAsync)
	{
		return ListenNotification(
			listenParam.QueueName,
			listenParam.NotificationName,
			listenParam.TagNamesWithIntersection,
			listenParam.TagNamesWithUnion,
			null,
			toDidReceivedNotificationAsync);
	}

	public List<NotificationListener> ListenAsync(
		IEnumerable<INotificationListenParam> listenParams,
		Func<Notification, CancellationToken, Task<object?>>? toDidReceivedNotificationAsync)
	{
		var notificationListeners = new List<NotificationListener>();
		foreach (var listenParam in listenParams)
		{
			var notificationListener
				= ListenAsync(
					listenParam,
					toDidReceivedNotificationAsync);
			// !!!
			notificationListeners.Add(notificationListener);
			// !!!
		}
		return notificationListeners;
	}

	public List<NotificationListener> ListenToAsync(
		Func<Notification, CancellationToken, Task<object?>>? toDidReceivedNotificationAsync,
		params INotificationListenParam[] listenParams)
	{
		var notificationListeners = new List<NotificationListener>();
		foreach (var listenParam in listenParams)
		{
			var notificationListener
				= Listen(
					listenParam,
					toDidReceivedNotificationAsync);
			// !!!
			notificationListeners.Add(notificationListener);
			// !!!
		}
		return notificationListeners;
	}

	public bool CancelListenWithListener(NotificationListener listener)
	{
		var notificationQueue = listener.NotificationQueue;
		if (notificationQueue == _notificationQueueUndefined)
		{
			if (listener?.CancelListen() == true)
			{
				return true;
			}
		}
		else if (notificationQueue?.Name?.Length > 0
			&& _notificationQueues.TryGetValue(notificationQueue.Name, out var notificationQueueExisted))
		{
			if (notificationQueueExisted != null
				&& notificationQueueExisted == notificationQueue)
			{
				if (listener?.CancelListen() == true)
				{
					return true;
				}
			}
		}
		return false;
	}

	#endregion


	////////////////////////////////////////////////
	// @事件节点
	////////////////////////////////////////////////

	#region 事件节点

	protected virtual NotificationQueue DidCreateNotificationQueue(string? notificationQueueName)
	{
		var isUnorderedQueue
			= notificationQueueName == null
			|| notificationQueueName.Length <= 0;
		var peekNotificationIntervalSecondsMaxDefault
			= this.NotificationQueuePeekNotificationIntervalSecondsMax;
		var sendNotificationConcurrentCountMaxDefault
			= this.NotificationQueueSendNotificationConcurrentCountMax;
		var sendNotificationTimeoutSecondsMaxDefault
			= this.NotificationQueueSendNotificationTimeoutSecondsMax;

		var notificationQueue = new NotificationQueue(
				notificationQueueName,
			       isUnorderedQueue,
			       //
			       null,
			       peekNotificationIntervalSecondsMaxDefault,
			       //
			       null,
			       sendNotificationConcurrentCountMaxDefault,
			       //
			       null,
			       sendNotificationTimeoutSecondsMaxDefault,
			       //
			       (notificationQueue, notification, exception) =>
			       {
				       this.DidReceiveExceptionThrewBySendNotificationInQueue(
					       notificationQueue,
					       notification,
					       exception);
			       });
		{ }
		return notificationQueue;
	}

	protected virtual void DidReceiveExceptionThrewBySendNotificationInQueue(
		NotificationQueue notificationQueue,
		Notification notification,
		Exception exception)
	{ }

	#endregion
}