namespace KanbanBoardAPI.Models.DTOs
{
    public class TaskCreateDto
    {
        public string Title { get; set; } = null!;
        public int ColumnId { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsDone { get; set; }
    }
}
