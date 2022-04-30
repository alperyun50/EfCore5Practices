using EfDataAccess.Data;
using EfModel.Models;
using EfModel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            // (eager loading) in more efficient way to load 
            List<Book> objList = _dbObject.Books.Include(u => u.Publisher).Include(u => u.BookAuthors).ThenInclude(u => u.Author).ToList();
            
            //// collect all category table items
            //List<Book> objList = _dbObject.Books.ToList();
            //foreach (var obj in objList)
            //{
            //    // less efficient way to load all publishers
            //    //obj.Publisher = _dbObject.Publishers.FirstOrDefault(u => u.Publisher_Id == obj.Publisher_Id);

            //    // (explicit loading) that will load all publishers in more efficient way
            //    _dbObject.Entry(obj).Reference(u => u.Publisher).Load();
            //    _dbObject.Entry(obj).Collection(u => u.BookAuthors).Load();

            //    foreach(var bookAuth in obj.BookAuthors)
            //    {
            //        _dbObject.Entry(bookAuth).Reference(u => u.Author).Load();
            //    }
            //}

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
            obj.Book = _dbObject.Books.Include(x => x.BookDetail).FirstOrDefault(u => u.Book_Id == id);
            // load bookdetails
            //obj.Book.BookDetail = _dbObject.BookDetails.FirstOrDefault(u => u.BookDetail_Id == obj.Book.BookDetail_Id);

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

        // many to many
        public IActionResult ManageAuthors(int id) 
        {
            BookAuthorVM obj = new BookAuthorVM
            {
                BookAuthorList = _dbObject.BookAuthors.Include(u => u.Author).Include(u => u.Book).Where(u => u.Book_Id == id).ToList(),
               
                BookAuthor = new BookAuthor()
                {
                    Book_Id = id
                },

                Book = _dbObject.Books.FirstOrDefault(u => u.Book_Id == id)
            };

            List<int> tempListOfAssignedAuthors = obj.BookAuthorList.Select(u => u.Author_Id).ToList();
            // Not in clause in Linq
            // get all the authors whos id is not in templistofassignedauthors
            var tempList = _dbObject.Authors.Where(u => !tempListOfAssignedAuthors.Contains(u.Author_Id)).ToList();

            obj.AuthorList = tempList.Select(i => new SelectListItem
            {
                Text = i.FullName,
                Value = i.Author_Id.ToString()
            });

            return View(obj);
        }


        // many to many operation
        [HttpPost]
        public IActionResult ManageAuthors(BookAuthorVM bookAuthorVM) 
        {
            if(bookAuthorVM.BookAuthor.Book_Id != 0 && bookAuthorVM.BookAuthor.Author_Id != 0)
            {
                _dbObject.BookAuthors.Add(bookAuthorVM.BookAuthor);
                _dbObject.SaveChanges();
            }

            return RedirectToAction(nameof(ManageAuthors), new { @id = bookAuthorVM.BookAuthor.Book_Id });
        }


        // many to many operation
        [HttpDelete]
        public IActionResult RemoveAuthors(int authorId, BookAuthorVM bookAuthorVM)
        {
            int bookId = bookAuthorVM.Book.Book_Id;
            BookAuthor bookAuthor = _dbObject.BookAuthors.FirstOrDefault(u => u.Author_Id == authorId && u.Book_Id == bookId);

            _dbObject.BookAuthors.Remove(bookAuthor);
            _dbObject.SaveChanges();

            return RedirectToAction(nameof(ManageAuthors), new { @id = bookId });
        }


        public IActionResult PlayGround()
        {
            var bookTemp = _dbObject.Books.FirstOrDefault();
            bookTemp.Price = 100;

            var bookCollection = _dbObject.Books;
            double totalPrice = 0;

            foreach (var book in bookCollection)
            {
                totalPrice += book.Price;
            }

            var bookList = _dbObject.Books.ToList();
            foreach (var book in bookList)
            {
                totalPrice += book.Price;
            }

            var bookCollection2 = _dbObject.Books;
            var bookCount1 = bookCollection2.Count();

            var bookCount2 = _dbObject.Books.Count();

            // returns all the records
            IEnumerable<Book> BookList1 = _dbObject.Books;
            // then filter is applied in memory
            var filteredBook1 = BookList1.Where(x => x.Price > 500).ToList();

            // returns filtered records (more efficiently)
            IQueryable<Book> BookList2 = _dbObject.Books;
            var filteredBook2 = BookList2.Where(x => x.Price > 500).ToList();

            //// if you want to change entity state manually
            //var category = _dbObject.Categories.FirstOrDefault();
            //_dbObject.Entry(category).State = EntityState.Modified;

            //_dbObject.SaveChanges();


            // updating related data
            var bookTemp1 = _dbObject.Books.Include(b => b.BookDetail).FirstOrDefault(b => b.Book_Id == 4);
            bookTemp1.BookDetail.NumberOfChapters = 222;

            _dbObject.Books.Update(bookTemp1);
            _dbObject.SaveChanges();


            // attaching related data (more efficient way for partial update)
            var bookTemp2 = _dbObject.Books.Include(b => b.BookDetail).FirstOrDefault(b => b.Book_Id == 4);
            bookTemp2.BookDetail.Weight = 3333;

            _dbObject.Books.Attach(bookTemp2);
            _dbObject.SaveChanges();


            // Views example
            var viewList = _dbObject.BookDetailsFromViews.ToList();
            var viewList1 = _dbObject.BookDetailsFromViews.FirstOrDefault();
            var viewList2 = _dbObject.BookDetailsFromViews.Where(u => u.Price > 500);


            // Raw Sql example
            var bookRaw = _dbObject.Books.FromSqlRaw("Select * from dbo.books").ToList();
            // if you have to use parameter(for security issues exp. sql injection)
            int id = 2;
            var bookTemp3 = _dbObject.Books.FromSqlInterpolated($"Select * from dbo.books Where Book_Id = {id}").ToList();

            // for stored procedures
            var booksSproc = _dbObject.Books.FromSqlInterpolated($"EXEC dbo.getAllBookDetails {id}").ToList();


            // .Net 5 only (if BookAuthors is a ICollection)
            var bookFilter1 = _dbObject.Books.Include(e => e.BookAuthors.Where(p => p.Author_Id == 1)).ToList();


            return RedirectToAction(nameof(Index));
        }

    }
}
