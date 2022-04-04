using EfDataAccess.Data;
using EfModel.Models;
using EfModel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EfCore5Practices.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext _dbObject;

        public BookController(AppDbContext dbObject)
        {
            _dbObject = dbObject;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // collect all category table items
            List<Book> objList = _dbObject.Books.ToList();
            return View(objList);
        }


        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            BookVM obj = new BookVM();
            // added for dropdownlist
            obj.PublisherList = _dbObject.Publishers.Select(i => new SelectListItem 
                                           {
                                               Text = i.Name,
                                               Value = i.Publisher_Id.ToString()
                                           });

            if (id == null)
            {
                return View(obj);
            }

            // for edit
            obj.Book = _dbObject.Books.FirstOrDefault(u => u.Book_Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Upsert(Book obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (obj.Book_Id == 0)
        //        {
        //            // this is create
        //            _dbObject.Books.Add(obj);
        //        }
        //        else
        //        {
        //            // this is update
        //            _dbObject.Books.Update(obj);
        //        }

        //        _dbObject.SaveChanges();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(obj);
        //}


        ////[HttpDelete]
        //public IActionResult Delete(int id)
        //{
        //    var objFromDb = _dbObject.Books.FirstOrDefault(x => x.Book_Id == id);

        //    _dbObject.Books.Remove(objFromDb);
        //    _dbObject.SaveChanges();

        //    return RedirectToAction(nameof(Index));
        //}

    }
}
