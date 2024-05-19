using BaoXia.Utils.Test.DotNet7.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace BaoXia.Utils.Test.DotNet7.MVC.Controllers
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
                        var logger = BaoXia.Utils.Environment.GetService<ILogger<HomeController>>();
                        {
                                Assert.IsTrue(logger != null);
                        }

                        logger = BaoXia.Utils.Environment.CreateServiceScope()!.ServiceProvider.GetService<ILogger<HomeController>>();
                        {
                                Assert.IsTrue(logger != null);
                        };

                        //
                        var currentApplicationAssembly = BaoXia.Utils.Environment.CurrentApplicationAssembly;
                        if (currentApplicationAssembly == null)
                        {
                                //
                                _logger.Log(LogLevel.Debug, "无法获取到当前应用程序的程序集信息。");
                                //
                                return new StatusCodeResult((int)System.Net.HttpStatusCode.InternalServerError);
                        }
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