using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Todolist.Entities;
using Todolist.Models;
using Todolist.Service;
using System;

namespace Todolist.Controllers
{
    public class TodoController : Controller
    {
        private readonly ILogger<TodoController> _logger;
        private readonly TodoDbContext _context;
        private readonly ITodoService _todoService;

        public TodoController(ILogger<TodoController> logger, TodoDbContext context, ITodoService todoService)
        {
            _logger = logger;
            _context = context;
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var todos = await _context.Todos.AsNoTracking().Select(e => new Todo
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description
            }).ToListAsync();

            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "tiltle_name" : "";
            ViewData["DescriptionSortParm"] = String.IsNullOrEmpty(sortOrder) ? "description_name" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var title = from s in _context.Todos
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                title = title.Where(s => s.Title.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "tiltle_name":
                    title = title.OrderByDescending(s => s.Title);
                    break;
                case "description_name":
                    title = title.OrderByDescending(s => s.Description);
                    break;
                default:
                    title = title.OrderBy(s => s.Title);
                    break;
            }

            //return View(todos);
            int pageSize = 3;
            return View(await PaginatedList<Todo>.CreateAsync(title.AsNoTracking(), page ?? 1, pageSize));

            //return View();

        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateEditTodoViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditTodoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newTodo = new Todo
                {
                    Title = model.Title,
                    Description = model.Description
                };
                await _context.Todos.AddAsync(newTodo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AjaxCreate()
        {
            var model = new CreateEditTodoViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjaxCreate(CreateEditTodoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newTodo = new Todo
                {
                    Title = model.Title,
                    Description = model.Description
                };
                await _context.Todos.AddAsync(newTodo);
                await _context.SaveChangesAsync();

                return Json(true);
            }
            return Json(false);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _todoService.Delete(id);

            return Json(result);
        }

    }
}
