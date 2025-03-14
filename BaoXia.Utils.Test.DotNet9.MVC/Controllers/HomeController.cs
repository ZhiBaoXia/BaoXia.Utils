using BaoXia.Utils.Extensions;
using BaoXia.Utils.Test.DotNet9.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BaoXia.Utils.Test.DotNet9.MVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			//
			var clientIpInfo = this.GetClientIpInfo();
			//
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
