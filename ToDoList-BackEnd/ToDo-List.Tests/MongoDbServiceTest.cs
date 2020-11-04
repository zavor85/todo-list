using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using NUnit.Framework;
using WebApplication;

namespace ToDo_List.Tests
{
    public class MongoDbServiceTest
    {
        private IMongoCollection<ToDo> _toDoCollection { get; }
        private readonly IMongoDbService _mongoDbService;

        public MongoDbServiceTest()
        {
            var mongoClient = new MongoClient("your connection string");
            var mongoDb = mongoClient.GetDatabase("todo-test");
            _toDoCollection = mongoDb.GetCollection<ToDo>("todo-list-test");
            _mongoDbService = new MongoDbService("todo-test", "todo-list-test");
        }

        [Test]
        public async Task DeleteToDoTest()
        {
            var todo = new ToDo() { ToDoTitle = "test" };
            await _mongoDbService.AddToDo(todo);
            var findToDo = _toDoCollection.Find(t => t.ToDoTitle == "test").ToList().Last();


            await _mongoDbService.DeleteToDo(findToDo.Id);
            var notExistToDo = await _toDoCollection.Find(t =>
                t.Id == findToDo.Id).ToListAsync();

            Assert.AreEqual(false, notExistToDo.Any());
        }

        [Test]
        public async Task DeleteAllTest()
        {
            var todo = new ToDo();
            var todo1 = new ToDo();
            await _mongoDbService.AddToDo(todo);
            await _mongoDbService.AddToDo(todo1);

            await _mongoDbService.DeleteAll();
            var dbCount = await _toDoCollection.EstimatedDocumentCountAsync();

            Assert.AreEqual(0, dbCount);
        }

        [Test]
        public async Task AddTodoTest()
        {
            await _mongoDbService.DeleteAll();
            var todo = new ToDo() { ToDoTitle = "test" };
            await _mongoDbService.AddToDo(todo);

            var result = await _toDoCollection.EstimatedDocumentCountAsync();

            Assert.AreEqual(1, result);
        }

        [Test]
        public async Task GetAllToDosTest()
        {
            await _mongoDbService.DeleteAll();
            var todo = new ToDo();
            var todo1 = new ToDo();
            await _mongoDbService.AddToDo(todo);
            await _mongoDbService.AddToDo(todo1);

            await _mongoDbService.GetAllToDos();

            Assert.AreEqual(2, await _toDoCollection.EstimatedDocumentCountAsync());
        }

        [Test]
        public async Task GetToDoByIdTest()
        {
            var todo = new ToDo(){ToDoTitle = "test"};
            await _mongoDbService.AddToDo(todo);
            var findToDo = _toDoCollection.Find(t => t.ToDoTitle == "test").ToList().Last();

            var getToDo = await _mongoDbService.GetToDoById(findToDo.Id);

            Assert.AreEqual(findToDo.Id, getToDo.Id);
        }

        [Test]
        public async Task UpdateToDo()
        {
            var todo = new ToDo() { ToDoTitle = "test" };
            await _mongoDbService.AddToDo(todo);
            var findToDo = _toDoCollection.Find(t => t.ToDoTitle == "test").ToList().Last();
            findToDo.IsCompleted = true;

            await _mongoDbService.UpdateToDo(findToDo.Id, findToDo);
            var collectionTodo = _toDoCollection.Find(t => t.Id == findToDo.Id).ToList().Last();

            Assert.IsTrue(collectionTodo.IsCompleted);
        }
    }
}