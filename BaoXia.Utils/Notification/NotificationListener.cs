using BaoXia.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Notification
{
	public class NotificationListener : IDisposable
	{

		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		public NotificationQueue? NotificationQueue { get; set; }

		public string? NotificationQueueName
		{
			get
			{
				var notificationQueue = this.NotificationQueue;
				if (notificationQueue != null)
				{
					return notificationQueue.Name;
				}
				return null;
			}
		}

		public string NotificationName { get; set; }

		public IEnumerable<string>? NotificationTagNamesWithIntersection { get; set; }

		public IEnumerable<string>? NotificationTagNamesWithUnion { get; set; }

		public Func<Notification, CancellationToken, object?>? ToDidReceivedNotification { get; set; }

		public Func<Notification, CancellationToken, Task<object?>>? ToDidReceivedNotificationAsync { get; set; }

		#endregion


		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		public NotificationListener(
			NotificationQueue notificationQueue,
			//
			string notificationName,
			IEnumerable<string>? tagNamesWithIntersection,
			IEnumerable<string>? tagNamesWithUnion,
			//
			Func<Notification, CancellationToken, object?>? toDidReceivedNotification,
			Func<Notification, CancellationToken, Task<object?>>? toDidReceivedNotificationAsync)
		{
			this.NotificationQueue = notificationQueue;
			this.NotificationName = notificationName;
			this.NotificationTagNamesWithIntersection = tagNamesWithIntersection;
			this.NotificationTagNamesWithUnion = tagNamesWithUnion;

			this.ToDidReceivedNotification = toDidReceivedNotification;
			this.ToDidReceivedNotificationAsync = toDidReceivedNotificationAsync;
		}

		~NotificationListener()
		{
			this.CancelListen();
		}

		public bool IsListenTo(
			string notificationName,
			IEnumerable<string>? notificationNameTagNames)
		{
			////////////////////////////////////////////////
			// 1/4，目标消息名称：
			////////////////////////////////////////////////
			var listenNotificationName = this.NotificationName;
			if (listenNotificationName?.Length > 0)
			{
				if (listenNotificationName.EqualsIgnoreCase(notificationName) != true)
				{
					return false;
				}
			}

			////////////////////////////////////////////////
			// 2/4，目标消息标签信息，交集：
			////////////////////////////////////////////////
			var listenNotificationTagNamesWithIntersection
				= this.NotificationTagNamesWithIntersection;
			if (listenNotificationTagNamesWithIntersection != null)
			{
				foreach (var listenNotificationTagName in listenNotificationTagNamesWithIntersection)
				{
					if (listenNotificationTagName?.Length > 0)
					{
						var isTagNameExisted = false;
						if (notificationNameTagNames != null)
						{
							foreach (var notificationNameTagName in notificationNameTagNames)
							{
								if (listenNotificationTagName.EqualsIgnoreCase(notificationNameTagName))
								{
									// !!!
									isTagNameExisted = true;
									break;
									// !!!
								}
							}
						}
						if (isTagNameExisted != true)
						{
							return false;
						}
					}
				}
			}

			////////////////////////////////////////////////
			// 3/4，目标消息标签信息，并集：
			////////////////////////////////////////////////
			var listenNotificationTagNamesWithUnion
				= this.NotificationTagNamesWithUnion;
			if (listenNotificationTagNamesWithUnion != null)
			{
				var isAnyListenNotificationTagNameExisted = false;
				foreach (var listenNotificationTagName in listenNotificationTagNamesWithUnion)
				{
					if (listenNotificationTagName?.Length > 0)
					{
						if (notificationNameTagNames != null)
						{
							foreach (var notificationNameTagName in notificationNameTagNames)
							{
								if (listenNotificationTagName.EqualsIgnoreCase(notificationNameTagName))
								{
									// !!!
									isAnyListenNotificationTagNameExisted = true;
									break;
									// !!!
								}
							}
						}
						if (isAnyListenNotificationTagNameExisted == true)
						{
							break;
						}
					}
				}
				if (isAnyListenNotificationTagNameExisted != true)
				{
					return false;
				}
			}


			return true;
		}

		public bool IsListenTo(Notification notification)
		{
			return this.IsListenTo(
				notification.Name,
				notification.TagNames);
		}

		public bool CancelListen()
		{
			var notificationQueue = this.NotificationQueue;
			if (notificationQueue != null)
			{
				if (notificationQueue.UnregisterListener(this))
				{
					return true;
				}
			}
			return false;
		}

		#endregion


		////////////////////////////////////////////////
		// @实现“IDisposable”
		////////////////////////////////////////////////

		#region 实现“IDisposable”

		public void Dispose()
		{
			this.CancelListen();

			////////////////////////////////////////////////
			GC.SuppressFinalize(this);
			////////////////////////////////////////////////

		}
		#endregion
	}
}
