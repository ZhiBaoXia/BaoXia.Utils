using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoXia.Utils.Notification;

public class Notification(
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
	Func<List<Object>, Task>? toNotificationSendedAsync) : NotificationIdentity(queueName,
			  name,
			  tagNames)
{
	////////////////////////////////////////////////
	// @自身属性，业务相关
	////////////////////////////////////////////////

	#region 自身属性，业务相关

	public string? Description { get; set; } = description;

	public Dictionary<string, object>? ParamDictionary { get; set; } = paramDictionary;

	public Object? ParamObject { get; set; } = paramObject;

	#endregion

	////////////////////////////////////////////////
	// @自身属性，消息系统相关
	////////////////////////////////////////////////

	#region 自身属性，消息系统相关

	public string? SenderName { get; set; } = senderName;

	public DateTime CreateTime { get; set; } = createTime;

	public double SendDelaySeconds { get; set; } = sendDelaySeconds;

	public DateTime SendTimeScheduled { get; set; } = createTime.AddSeconds(sendDelaySeconds);

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

	public Action<List<Object>>? ToNotificationSended { get; set; } = toNotificationSended;

	public Func<List<Object>, Task>? ToNotificationSendedAsync { get; set; } = toNotificationSendedAsync;

	#endregion
}
