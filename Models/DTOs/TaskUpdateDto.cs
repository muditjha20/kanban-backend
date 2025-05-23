namespace KanbanBoardAPI.Models.DTOs
{
    public class TaskUpdateDto
    {
        public string Title { get; set; } = null!;
        public bool IsDone { get; set; }
        public int ColumnId { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
