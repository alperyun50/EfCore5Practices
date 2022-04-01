using EfDataAccess.Data;
using EfModel.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfCore5Practices.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AppDbContext _dbObject;

        public AuthorController(AppDbContext dbObject)
        {
            _dbObject = dbObject;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category obj)
        {
            return View();
        }


        //[HttpDelete]
        public IActionResult Delete(int id)
        {

            return RedirectToAction(nameof(Index));
        }

    }
}
