namespace WebApplication
{
    public interface IToDo
    {
        int Id { get; set; }
        bool IsCompleted { get; set; }
        string? ToDoTitle { get; set; }
    }
}