using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace BaoXia.Utils
{
	public class EMail
	{
		/// <summary>
		/// 发送电子邮件。
		/// </summary>
		/// <param name="smtpServerAddress">目标STMP邮件服务器，地址。</param>
		/// <param name="smtpServerPort">目标STMP邮件服务器，端口号。</param>
		/// <param name="isSmtpServerSSLEnable">目标STMP邮件服务器，是否支持SSL。</param>
		/// <param name="objectEMailAddresses">目标邮件地址数组。</param>
		/// <param name="copyObjectEMailAddresses">抄送邮件地址数组。</param>
		/// <param name="subject">邮件主题。</param>
		/// <param name="body">邮件正文。</param>
		/// <param name="attachmentFilePaths">附件文件的文件路径集合。</param>
		public static bool SendMailTo(
			string smtpServerAddress,
			int smtpServerPort,
			bool isSmtpServerSSLEnable,
			//
			ICollection<string> objectEMailAddresses,
			ICollection<string>? copyObjectEMailAddresses,
			string subject,
			string body,
			ICollection<string>? attachmentFilePaths,
			//
			string? authorName,
			string authorEMailAccount,
			string authorEMailPassword)
		{
			if (objectEMailAddresses.Count < 1)
			{
				return false;
			}
			if (subject.Length < 1)
			{
				return false;
			}
			if (body.Length < 1)
			{
				return false;
			}
			if (authorEMailAccount == null
				|| authorEMailAccount.Length < 1)
			{
				return false; ;
			}
			if (authorEMailPassword == null
				|| authorEMailPassword.Length < 1)
			{
				return false; ;
			}



			//初始化MailMessage实例
			var message = new MailMessage
			{

				// 邮件优先级：
				// message.Priority = MailPriority.High;

				//发件人地址
				From = new MailAddress(
					authorEMailAccount,
					authorName)
			};

			//向收件人地址集合添加邮件地址
			if (objectEMailAddresses != null)
			{
				foreach (var objectEMailAddress in objectEMailAddresses)
				{
					message.To.Add(objectEMailAddress);
				}
			}

			//向抄送收件人地址集合添加邮件地址
			if (copyObjectEMailAddresses != null)
			{
				foreach (var copyObjectEMailAddress in copyObjectEMailAddresses)
				{
					message.CC.Add(copyObjectEMailAddress);
				}
			}


			// 标题：
			message.Subject = subject;
			//标题内容使用的编码方式：
			message.SubjectEncoding = Encoding.UTF8;

			// 正文：
			message.Body = body;
			// 正文的编码方式：
			message.BodyEncoding = Encoding.UTF8;
			// 正文是否为HTML，否，避免邮箱屏蔽：
			message.IsBodyHtml = false;

			// 附件信息：
			if (attachmentFilePaths?.Count > 0)
			{
				foreach (string attachmentFilePath
					in
					attachmentFilePaths)
				{
					var attachFile = new Attachment(attachmentFilePath);
					{
						message.Attachments.Add(attachFile);
					}
				}
			}

			var smtp = new SmtpClient
			{
				// SMTP邮件服务器：
				Host = smtpServerAddress,
				// 加密端口：
				Port = smtpServerPort,

				EnableSsl = isSmtpServerSSLEnable,

				// 主机账号和密码：
				Credentials = new System.Net.NetworkCredential(
					authorEMailAccount,
					authorEMailPassword)
			};

			// 将邮件发送到SMTP邮件服务器
			// !!!
			smtp.Send(message);
			// !!!

			return true;
		}
	}
}