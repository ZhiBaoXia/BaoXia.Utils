using System;

namespace BaoXia.Utils.Models
{
	public class LogRecord
	{
		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		/// <summary>
		/// 日志记录时间。
		/// </summary>
		public DateTime LogTime { get; set; }

		/// <summary>
		/// 日志调用者名称。
		/// </summary>
		public string? Invoker { get; set; }

		/// <summary>
		/// 日志内容。
		/// </summary>
		public string? Content { get; set; }


		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		/// <summary>
		/// 默认构造函数。
		/// </summary>
		public LogRecord()
		{ }


		/// <summary>
		/// 使用调用者和日志内容初始化日志记录对象。
		/// </summary>
		/// <param name="invoker">当前日志的调用者名称。</param>
		/// <param name="content">当前日志内容。</param>
		public LogRecord(
			string invoker,
			string content)
		{
			this.LogTime = DateTime.Now;
			this.Invoker = invoker;
			this.Content = content;
		}
	}
}
