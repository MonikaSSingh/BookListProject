﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Book Book { get; set; }

        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Book = new Book();
            if(id==null)
            {
                return View(Book);
            }

            Book = _db.Book.FirstOrDefault(u => u.ID == id);
            if(Book==null)
            {
                return NotFound();
            }
            return View(Book);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert()
        {
            if(ModelState.IsValid)
            {
                if(Book.ID==0)
                {
                    _db.Book.Add(Book);
                }
                else
                {
                    _db.Book.Update(Book);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Book);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Book.ToListAsync() });
        }


        [HttpDelete]

        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDB = await _db.Book.FirstOrDefaultAsync(u => u.ID == id);

            if (bookFromDB == null)
            {
                return Json(new { success = false, message = "Error while Deleteing" });
            }
            _db.Book.Remove(bookFromDB);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Delete Successfully" });

        }
        #endregion
    }
}