using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Todolist.Entities;
using Todolist.Models;

namespace Todolist.Controllers
{
    public class TodoController : Controller
    {
        private readonly ILogger<TodoController> _logger;
        private readonly TodoDbContext _context;

        public TodoController(ILogger<TodoController> logger, TodoDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var todos = await _context.Todos.AsNoTracking().Select(e => new TodoViewModel
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description
            }).ToListAsync();

            return View(todos);
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
    }
}
