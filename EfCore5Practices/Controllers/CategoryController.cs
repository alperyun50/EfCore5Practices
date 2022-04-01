using EfDataAccess.Data;
using EfModel.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfCore5Practices.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _dbObject;

        public CategoryController(AppDbContext dbObject)
        {
            _dbObject = dbObject;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // collect all category table items
            List<Category> objList = _dbObject.Categories.ToList();
            return View(objList);
        }


        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Category obj = new Category();

            if(id == null)
            {
                return View(obj);
            }

            obj = _dbObject.Categories.FirstOrDefault(u => u.Category_Id == id);

            if(obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category obj)
        {
            if(ModelState.IsValid)
            {
                if(obj.Category_Id == 0)
                {
                    // this is create
                    _dbObject.Categories.Add(obj);
                }
                else
                {
                    // this is update
                    _dbObject.Categories.Update(obj);
                }

                _dbObject.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
         
            return View(obj);
        }


        //[HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _dbObject.Categories.FirstOrDefault(x => x.Category_Id == id);

            _dbObject.Categories.Remove(objFromDb);
            _dbObject.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        // ef core bulk operations
        public IActionResult CreateMultiple2()
        {
            List<Category> catList = new List<Category>();

            for(int i = 1; i <= 2; i++)
            {
                //_dbObject.Categories.Add(new Category { Name = Guid.NewGuid().ToString() });
                catList.Add(new Category { Name = Guid.NewGuid().ToString() });
            }

            _dbObject.Categories.AddRange(catList);
            _dbObject.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        // ef core bulk operations
        public IActionResult CreateMultiple5()
        {
            List<Category> catList = new List<Category>();

            for (int i = 1; i <= 5; i++)
            {
                //_dbObject.Categories.Add(new Category { Name = Guid.NewGuid().ToString() });
                catList.Add(new Category { Name = Guid.NewGuid().ToString() });
            }

            _dbObject.Categories.AddRange(catList);
            _dbObject.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        // ef core bulk operations
        public IActionResult RemoveMultiple2()
        {      
            // category table items ordered by descending values and catList include top two values
            IEnumerable<Category> catList = _dbObject.Categories.OrderByDescending(x => x.Category_Id).Take(2).ToList();

            _dbObject.Categories.RemoveRange(catList);
            _dbObject.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        // ef core bulk operations
        public IActionResult RemoveMultiple5()
        {
            // category table items ordered by descending values and catList include top five values
            IEnumerable<Category> catList = _dbObject.Categories.OrderByDescending(x => x.Category_Id).Take(5).ToList();

            _dbObject.Categories.RemoveRange(catList);
            _dbObject.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
