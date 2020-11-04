using System;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication
{
    public class ToDo : IToDo
    {
        [BsonId]
        public int Id { get; set; }
        [BsonRequired]
        public string? ToDoTitle { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime ToDoPostTime = DateTime.Now;
    }
}
