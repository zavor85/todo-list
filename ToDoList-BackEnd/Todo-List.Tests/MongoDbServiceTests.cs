using MongoDB.Driver;
using System.Threading.Tasks;
using WebApplication;
using Xunit;

namespace Todo_List.Tests
{
    public class MongoDbServiceTests
    {
        private IMongoCollection<ToDo> _toDoCollection { get; set; }
        private IMongoDbService _mongoDbService = new MongoDbService("todo", "todo-list");
        public MongoDbServiceTests(string dbName, string collectionName)
        {
            var mongoClient = new MongoClient("mongodb+srv://todo:todo@zavor.yusuq.mongodb.net/todo?retryWrites=true&w=majority");
            var mongoDb = mongoClient.GetDatabase(dbName);
            _toDoCollection = mongoDb.GetCollection<ToDo>(collectionName);
        }

        [Fact]
        public async Task AddTodoTest()
        {
            var todo = new ToDo(){ToDoTitle = "test"};
            await _mongoDbService.AddToDo(todo);
            var result = await _toDoCollection.FindAsync(t => t.Id == todo.Id);
            var list = result.ToList();
            Assert.Equal(1, list.Count);
        }
    }
}
