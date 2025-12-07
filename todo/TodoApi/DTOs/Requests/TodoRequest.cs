// DTOs/Requests/TodoRequest.cs
namespace TodoApi.DTOs.Requests
{
    public class TodoRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
    }
}