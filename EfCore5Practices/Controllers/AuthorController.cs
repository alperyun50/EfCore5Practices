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
            // collect all category table items
            List<Author> objList = _dbObject.Authors.ToList();
            return View(objList);
        }


        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Author obj = new Author();

            if (id == null)
            {
                return View(obj);
            }

            obj = _dbObject.Authors.FirstOrDefault(u => u.Author_Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Author obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Author_Id == 0)
                {
                    // this is create
                    _dbObject.Authors.Add(obj);
                }
                else
                {
                    // this is update
                    _dbObject.Authors.Update(obj);
                }

                _dbObject.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }


        //[HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _dbObject.Authors.FirstOrDefault(x => x.Author_Id == id);

            _dbObject.Authors.Remove(objFromDb);
            _dbObject.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
