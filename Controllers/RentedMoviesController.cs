using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TvShowsApp.Models;
using OfficeOpenXml;
using System.IO;
using System.Drawing;
//---------------
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Authorization;



namespace TvShowsApp.Controllers
{
    [Authorize]
    public class RentedMoviesController : Controller
    {
        private readonly TvShowsContext _context;

        public RentedMoviesController(TvShowsContext context)
        {
            _context = context;
        }

        // GET: RentedMovies
        public async Task<IActionResult> Index()
        {
            var tvShowsContext = _context.RentedMovie.Include(r => r.Cus).Include(r => r.TvShow);
            return View(await tvShowsContext.ToListAsync());
        }

        // GET: RentedMovies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentedMovie = await _context.RentedMovie
                .Include(r => r.Cus)
                .Include(r => r.TvShow)
                .FirstOrDefaultAsync(m => m.RentedMovieId == id);
            if (rentedMovie == null)
            {
                return NotFound();
            }

            return View(rentedMovie);
        }

        // GET: RentedMovies/Create
        public IActionResult Create(int Id)
        {
            ViewData["CusId"] = new SelectList(_context.Customers, "CustomersId", "FirstName");
            //ViewData["TvShowId"] = new SelectList
            //(_context.TvShow.Where(c => !_context.RentedMovie.Select(b=> b.TvShowId).Contains(c.TvId)), "TvId", "Title");
            //return View();
            ViewBag.TvShowId = Id;
            return View();
        }

        // POST: RentedMovies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentedMovieId,CusId,TvShowId,RentalDate,ReturnDate")] RentedMovie rentedMovie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rentedMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CusId"] = new SelectList(_context.Customers, "CustomersId", "FirstName", rentedMovie.CusId);
            ViewData["TvShowId"] = new SelectList(_context.TvShow, "TvId", "Title", rentedMovie.TvShowId);
            return View(rentedMovie);
        }

        // GET: RentedMovies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentedMovie = await _context.RentedMovie.FindAsync(id);
            if (rentedMovie == null)
            {
                return NotFound();
            }
            ViewData["CusId"] = new SelectList(_context.Customers, "CustomersId", "FirstName", rentedMovie.CusId);
            ViewData["TvShowId"] = new SelectList(_context.TvShow, "TvId", "Title", rentedMovie.TvShowId);
            return View(rentedMovie);
        }

        // POST: RentedMovies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RentedMovieId,CusId,TvShowId,RentalDate,ReturnDate")] RentedMovie rentedMovie)
        {
            if (id != rentedMovie.RentedMovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentedMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentedMovieExists(rentedMovie.RentedMovieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CusId"] = new SelectList(_context.Customers, "CustomersId", "FirstName", rentedMovie.CusId);
            ViewData["TvShowId"] = new SelectList(_context.TvShow, "TvId", "Title", rentedMovie.TvShowId);
            return View(rentedMovie);
        }

        // GET: RentedMovies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentedMovie = await _context.RentedMovie
                .Include(r => r.Cus)
                .Include(r => r.TvShow)
                .FirstOrDefaultAsync(m => m.RentedMovieId == id);
            if (rentedMovie == null)
            {
                return NotFound();
            }

            return View(rentedMovie);
        }

        // POST: RentedMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentedMovie = await _context.RentedMovie.FindAsync(id);
            _context.RentedMovie.Remove(rentedMovie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentedMovieExists(int id)
        {
            return _context.RentedMovie.Any(e => e.RentedMovieId == id);
        }

        //-------------------------------
        [HttpPost]
        public void Rent(int TvShowId, int CusId)
        {
            RentedMovie rentedMovie = new RentedMovie();
            rentedMovie.TvShowId = TvShowId;
            rentedMovie.CusId = CusId;
            rentedMovie.RentalDate = DateTime.Now;
            _context.Add(rentedMovie);
            _context.SaveChanges();
            // await _context.SaveChangesAsync();

            TvShow TvShow = _context.TvShow.FirstOrDefault(a => a.TvId == TvShowId);
            TvShow.Available = true;
            _context.Update(TvShow);
            _context.SaveChanges();
            // await _context.SaveChangesAsync();
            // return RedirectToAction("Index", "TvShows");
        }
        [HttpPost]
        // public async Task<IActionResult> Return(int TvShowId, int CusId)
        public void Return(int TvShowId, int CusId)
        {
            RentedMovie rentedMovie =
                _context.RentedMovie.FirstOrDefault(a => a.ReturnDate == default && a.TvShowId == TvShowId);
            rentedMovie.ReturnDate = DateTime.Now;
            _context.Update(rentedMovie);
            _context.SaveChanges();
            //  await _context.SaveChangesAsync();

            TvShow TvShow = _context.TvShow.FirstOrDefault(a => a.TvId == TvShowId);
            TvShow.Available = false;
            _context.Update(TvShow);
            _context.SaveChanges();
            //  return RedirectToAction("Index", "TvShows");
        }
        // -------------------------------------------------------------- dole za excel

        [HttpGet]
        public IActionResult ExportExcel()
        {

            string[] col_names = new string[]

            {
                "Name",
                "Title",
                "Date Rented",
                "Date Returned",
                "Today"
          };

            byte[] result;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("final");
                for (int i = 0; i < col_names.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Style.Font.Size = 14;
                    worksheet.Cells[1, i + 1].Value = col_names[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 243, 214));
                }

                int row = 2;

                foreach (RentedMovie item in _context.RentedMovie.Include(c => c.Cus).Include(a => a.TvShow).ToList())
                {
                    if (item.ReturnDate == null)
                    {
                        TimeSpan validDate = DateTime.Now.AddDays(7).Subtract(item.RentalDate);
                        if (validDate > TimeSpan.FromDays(7))
                        {
                            for (int col = 1; col <= col_names.Length; col++)
                            {
                                worksheet.Cells[row, col].Style.Font.Size = 12;
                                worksheet.Cells[row, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                            }
                            worksheet.Cells[row, 1].Value = item.Cus.FirstName;
                            worksheet.Cells[row, 2].Value = item.TvShow.Title;
                            worksheet.Cells[row, 3].Value = item.RentalDate.ToString();
                            worksheet.Cells[row, 4].Value = item.ReturnDate.ToString();
                            worksheet.Cells[row, 5].Value = DateTime.Now.AddDays(7).ToString();

                            if (row % 2 == 0)
                            {
                                for (int col = 1; col <= col_names.Length; col++)
                                {
                                    worksheet.Cells[row, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(154, 211, 157));
                                }
                            }
                        }
                        row++;
                    }

                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                result = package.GetAsByteArray();
            }
            return File(result, "application/vnd.ms-excel", "test.xlsx");
        }
        // --------------------------------------------------------------------- dole za mail

        [HttpGet]       
        public IActionResult SendMail()
        {
            foreach (RentedMovie item in _context.RentedMovie.Include(c => c.Cus).Include(a => a.TvShow).ToList())
            {
                if (item.ReturnDate == null)
                {
                    TimeSpan validDate = DateTime.Now.AddDays(7).Subtract(item.RentalDate);
                    if (validDate > TimeSpan.FromDays(7))
                    {
                        var message = new MimeMessage();

                        message.From.Add(new MailboxAddress("Filip", "foxitvezbi@gmail.com"));

                        // message.To.Add(new MailboxAddress("Stefan", "stefanmitrikeski1994@gmail.com"));
                        message.To.Add(new MailboxAddress(item.Cus.FirstName, item.Cus.E_mail));

                        message.Subject = "Warning";

                        message.Body = new TextPart("plain")
                        {
                            Text = "Dear "+ item.Cus.FirstName+ " " +item.Cus.LastName + " you are late with returning of the movie "
                            + item.TvShow.Title+
                            "  please return it as soon as posible. "
                        };
                        using (var client = new SmtpClient())
                        {
                            client.Connect("smtp.gmail.com", 587, false);
                            client.Authenticate("foxitvezbi@gmail.com", "123456foxit");

                            client.Send(message);
                            client.Disconnect(true);
                            // client.Dispose();
                        }
                    }
                }
            }
            var tvShowsContext = _context.RentedMovie.Include(r => r.Cus).Include(r => r.TvShow);
            return RedirectToAction("Index", "RentedMovies");
        }

        //---------------------------------------------------------
    }
}
