using DealRept.Models;
using DealRept.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DealRept.Controllers
{
    public class HomeController : Controller
    {
        private readonly DealDbContext context;

        public HomeController(DealDbContext ctx)
        {
            context = ctx;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

