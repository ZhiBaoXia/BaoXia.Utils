namespace BaoXia.Utils.Notification
{
	public enum NotificationQueueExceptionProcessType
	{
		/// <summary>
		/// 当消息队列发生异常时，继续处理下个消息。
		/// </summary>
		ContinueProcessNextNotification,

		/// <summary>
		/// 当消息队列发生异常时，重新尝试处理当前消息，直到处理成功。
		/// </summary>
		RetryToProcessCurrentNotificationUtilProcessSuccess,

		/// <summary>
		/// 当消息队列发生异常时，清空当前队列。
		/// </summary>
		ClearCurrentNotificationQueue,
	}
}
