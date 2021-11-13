using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Rayn.ViewModels;

namespace Rayn.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return this.View();
    }

    public IActionResult Usage()
    {
        return this.View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
