using KanbanBoardAPI.Data;
using KanbanBoardAPI.Models;
using KanbanBoardAPI.Models.DTOs;
// using KanbanBoardAPI.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace KanbanBoardAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IHubContext<TaskHub> _hubContext;

    public TasksController(AppDbContext context, IHubContext<TaskHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
        var tasks = await _context.Tasks.ToListAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            IsDone = dto.IsDone,
            ColumnId = dto.ColumnId,
            Description = dto.Description,
            Tags = dto.Tags,
            DueDate = dto.DueDate
        };

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        await _hubContext.Clients.All.SendAsync("TaskCreated", task);

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpdateDto dto)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        task.Title = dto.Title;
        task.IsDone = dto.IsDone;
        task.ColumnId = dto.ColumnId;
        task.Description = dto.Description;
        task.Tags = dto.Tags;
        task.DueDate = dto.DueDate;

        await _context.SaveChangesAsync();

        await _hubContext.Clients.All.SendAsync("TaskUpdated", task);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        await _hubContext.Clients.All.SendAsync("TaskDeleted", id);

        return NoContent();
    }
}
