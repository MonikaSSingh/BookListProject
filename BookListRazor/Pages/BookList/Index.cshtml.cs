using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListRazor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookListRazor.Pages.BookList
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Book> Books { get; set; }

        public string Message;

        public async Task OnGet()
        {
            Books = await _db.Book.ToListAsync();
            Message = "Getting from Index File";
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var GetData = await _db.Book.FindAsync(id);

            if(GetData==null)
            {
                return NotFound();
            }
            else
            {
               _db.Book.Remove(GetData);
               await _db.SaveChangesAsync();

                return RedirectToPage("Index");
            }
        }
    }
}