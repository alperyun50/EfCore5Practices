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
            foreach(var obj in objList)
            {
                // less efficient way to load all publishers
                //obj.Publisher = _dbObject.Publishers.FirstOrDefault(u => u.Publisher_Id == obj.Publisher_Id);
                
                // (explicit loading) that will load all publishers in more efficient way
                _dbObject.Entry(obj).Reference(u => u.Publisher).Load();
            }
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookVM obj)
        {
                if (obj.Book.Book_Id == 0)
                {
                    // this is create
                    _dbObject.Books.Add(obj.Book);
                }
                else
                {
                    // this is update
                    _dbObject.Books.Update(obj.Book);
                }

                _dbObject.SaveChanges();
               
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public IActionResult Details(int? id)
        {
            BookVM obj = new BookVM();
            
            if (id == null)
            {
                return View(obj);
            }

            // for edit
            obj.Book = _dbObject.Books.FirstOrDefault(u => u.Book_Id == id);
            // load bookdetails
            obj.Book.BookDetail = _dbObject.BookDetails.FirstOrDefault(u => u.BookDetail_Id == obj.Book.BookDetail_Id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(BookVM obj)
        {
            if (obj.Book.BookDetail.BookDetail_Id == 0)
            {
                // this is create
                _dbObject.BookDetails.Add(obj.Book.BookDetail);
                _dbObject.SaveChanges();

                // that will update BookDetail_Id in Books table
                var BookFromDb = _dbObject.Books.FirstOrDefault(u => u.Book_Id == obj.Book.Book_Id);
                BookFromDb.BookDetail_Id = obj.Book.BookDetail.BookDetail_Id;
                _dbObject.SaveChanges();
            }
            else
            {
                // this is update
                _dbObject.BookDetails.Update(obj.Book.BookDetail);
                _dbObject.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }



        //[HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _dbObject.Books.FirstOrDefault(x => x.Book_Id == id);

            _dbObject.Books.Remove(objFromDb);
            _dbObject.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
