using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using HangfireAutofacDemo.Jobs;
using Microsoft.AspNetCore.Mvc;
using HangfireAutofacDemo.Models;
using HangfireAutofacDemo.Services;

namespace HangfireAutofacDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly CountService _countService;
        private readonly IBackgroundJobClient _client;

        public HomeController(CountService countService, IBackgroundJobClient client)
        {
            _countService = countService;
            _client = client;
        }

        public IActionResult Index()
        {
            // enqueue the job and propagate tenant context to it for use by the job filter
            _client.Enqueue<CountJob>(v => v.Execute(Request.Query["tenant"].FirstOrDefault()));
            ViewData["Count"] = _countService.Next();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
