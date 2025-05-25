public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Tags { get; set; }

    public DateTime? DueDate { get; set; }

    public bool IsDone { get; set; }

    public int ColumnId { get; set; }

    public Column Column { get; set; } = null!;

    public string UserId { get; set; }
}

// This code defines a TaskItem model for a Kanban board application.
// There are extra properties that i will utilize later in further updates.
