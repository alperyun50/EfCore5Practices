using EfDataAccess.Data;
using EfModel.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfCore5Practices.Controllers
{
    public class PublisherController : Controller
    {
        private readonly AppDbContext _dbObject;

        public PublisherController(AppDbContext dbObject)
        {
            _dbObject = dbObject;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // collect all category table items
            List<Publisher> objList = _dbObject.Publishers.ToList();
            return View(objList);
        }


        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Publisher obj = new Publisher();

            if (id == null)
            {
                return View(obj);
            }

            obj = _dbObject.Publishers.FirstOrDefault(u => u.Publisher_Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Publisher obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Publisher_Id == 0)
                {
                    // this is create
                    _dbObject.Publishers.Add(obj);
                }
                else
                {
                    // this is update
                    _dbObject.Publishers.Update(obj);
                }

                _dbObject.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }


        //[HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _dbObject.Publishers.FirstOrDefault(x => x.Publisher_Id == id);

            _dbObject.Publishers.Remove(objFromDb);
            _dbObject.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
