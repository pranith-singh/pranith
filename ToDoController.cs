using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ToDoController : Controller
{
    private readonly ApplicationDbContext _context;

    public ToDoController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ToDo
    public async Task<IActionResult> Index()
    {
        return View(await _context.ToDoItems.ToListAsync());
    }

    // GET: ToDo/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ToDo/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description")] ToDoItem toDoItem)
    {
        if (ModelState.IsValid)
        {
            _context.Add(toDoItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(toDoItem);
    }

    // GET: ToDo/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var toDoItem = await _context.ToDoItems.FindAsync(id);
        if (toDoItem == null)
        {
            return NotFound();
        }
        return View(toDoItem);
    }

    // POST: ToDo/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsCompleted")] ToDoItem toDoItem)
    {
        if (id != toDoItem.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(toDoItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoItemExists(toDoItem.Id))
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
        return View(toDoItem);
    }

    private bool ToDoItemExists(int id)
    {
        return _context.ToDoItems.Any(e => e.Id == id);
    }
}
