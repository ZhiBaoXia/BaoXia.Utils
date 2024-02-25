using BaoXia.Utils.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaoXia.Utils.Extensions;

public static class ControllerExtension
{
	/// <summary>
	/// 获取客户端的Ip信息。
	/// </summary>
	/// <param name="controller">当前控制器对象。</param>
	/// <returns>当前控制器对象的客户端Ip信息。</returns>
	public static ClientIpInfo GetClientIpInfo(
		this Controller? controller)
	{
		if (controller == null)
		{
			return new ClientIpInfo();
		}
		return controller.HttpContext.Request.GetClientIpInfo();
	}
}