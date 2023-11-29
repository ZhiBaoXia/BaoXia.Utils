using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoXia.Utils.Notification
{
	public class Notification : NotificationIdentity
	{

		////////////////////////////////////////////////
		// @自身属性，业务相关
		////////////////////////////////////////////////

		#region 自身属性，业务相关

		public string? Description { get; set; }

		public Dictionary<string, object>? ParamDictionary { get; set; }

		public Object? ParamObject { get; set; }

		#endregion

		////////////////////////////////////////////////
		// @自身属性，消息系统相关
		////////////////////////////////////////////////

		#region 自身属性，消息系统相关

		public string? SenderName { get; set; }

		public DateTime CreateTime { get; set; }

		public double SendDelaySeconds { get; set; }

		public DateTime SendTimeScheduled { get; set; }

		/// <summary>
		/// 只有指定了延迟时间时，才使用“SendTimeScheduled”字段进行延迟发送。
		/// </summary>
		public bool IsNeedSendBySendTimeScheduled
		{
			get
			{
				if (this.SendDelaySeconds > 0.0)
				{
					return true;
				}
				return false;
			}
		}

		public DateTime SendTimeBegin { get; set; }

		public DateTime SendTimeEnd { get; set; }

		public Action<List<Object>>? ToNotificationSended { get; set; }

		public Func<List<Object>, Task>? ToNotificationSendedAsync { get; set; }

		#endregion


		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		public Notification(
			string? queueName,
			string name,
			IEnumerable<string>? tagNames,
			//
			string? description,
			//
			Dictionary<string, object>? paramDictionary,
			object? paramObject,
			string? senderName,
			DateTime createTime,
			double sendDelaySeconds,
			//
			Action<List<Object>>? toNotificationSended,
			Func<List<Object>, Task>? toNotificationSendedAsync)
			: base(queueName,
				  name,
				  tagNames)
		{
			this.Description = description;
			this.ParamDictionary = paramDictionary;
			this.ParamObject = paramObject;
			this.SenderName = senderName;
			this.CreateTime = createTime;
			this.SendDelaySeconds = sendDelaySeconds;
			this.SendTimeScheduled = createTime.AddSeconds(sendDelaySeconds);


			this.ToNotificationSended = toNotificationSended;
			this.ToNotificationSendedAsync = toNotificationSendedAsync;
		}

		#endregion


	}
}
