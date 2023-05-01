using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearn.Areas.Admin.ViewModels.Course;
using Elearn.Data;
using Elearn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Elearn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;

        public CourseController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Course> courses = await _context.Course.Include(c => c.CourseImages).ToListAsync();

            List<CourseListVM> model = new List<CourseListVM>();

            foreach (var course in courses)
            {
                CourseListVM mappedData = new CourseListVM
                {
                    Id = course.Id,
                    Title = course.Title,
                    Price = course.Price,
                    Sales = course.Sales,
                    Image = course.CourseImages.FirstOrDefault(c => c.IsMain).Name
                };

                model.Add(mappedData);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = await GetAuthors();

            return View();
        }

        private async Task<SelectList> GetAuthors()
        {
            IEnumerable<Author> authors = await _context.Authors.ToListAsync();

            return new SelectList(authors, "Id", "FullName");
        }
    }
}