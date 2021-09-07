using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Rayn.Controllers
{
    public class HistoryController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
