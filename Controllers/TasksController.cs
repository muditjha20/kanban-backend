using KanbanBoardAPI.Data;
using KanbanBoardAPI.Models;
using KanbanBoardAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoardAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
        var userId = HttpContext.Items["UserUID"] as string;
        if (userId == null) return Unauthorized();

        var tasks = await _context.Tasks
            .Where(t => t.UserId == userId)
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var userId = HttpContext.Items["UserUID"] as string;
        if (userId == null) return Unauthorized();

        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (task == null) return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto dto)
    {
        var userId = HttpContext.Items["UserUID"] as string;
        if (userId == null) return Unauthorized();

        var task = new TaskItem
        {
            Title = dto.Title,
            IsDone = dto.IsDone,
            ColumnId = dto.ColumnId,
            Description = dto.Description,
            Tags = dto.Tags,
            DueDate = dto.DueDate,
            UserId = userId
        };

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpdateDto dto)
    {
        var userId = HttpContext.Items["UserUID"] as string;
        if (userId == null) return Unauthorized();

        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (task == null) return NotFound();

        task.Title = dto.Title;
        task.IsDone = dto.IsDone;
        task.ColumnId = dto.ColumnId;
        task.Description = dto.Description;
        task.Tags = dto.Tags;
        task.DueDate = dto.DueDate;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userId = HttpContext.Items["UserUID"] as string;
        if (userId == null) return Unauthorized();

        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (task == null) return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
