using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Todolist.Entities;
using Todolist.Models;
using Todolist.Service;
using System;
using System.Collections.Generic;

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

        //[HttpGet]
        public IActionResult Index(int pg = 1, string SearchText = "")
        {
            //var todo = await _context.Todos.AsNoTracking().Select(e => new Todo
            //{
            //    Id = e.Id,
            //    Title = e.Title,
            //    Description = e.Description
            //}).ToListAsync();

            List<Todo> todos;

            if (SearchText != "")
            {
                todos = _context.Todos
                    .Where(x => x.Title.Contains(SearchText) || x.Description.Contains(SearchText))
                    .ToList();
            }
            else
                todos = _context.Todos.ToList();

            const int pageSize = 3;
            if (pg < 1)
                pg = 1;

            int resCount = todos.Count();

            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;

            var data = todos.Skip(resSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;


            return View(data);
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
