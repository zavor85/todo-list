using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using System.Threading.Tasks;
using MongoDB.Driver.Core.Operations;
using MongoDB.Driver.Linq;
using WebApplication;

namespace Todo_ListTests
{
    [TestClass]
    public class MongoDbServiceTest
    {
        private IMongoCollection<ToDo> _toDoCollection { get; set; }
        private readonly IMongoDbService _mongoDbService;

        public MongoDbServiceTest()
        {
            var mongoClient = new MongoClient("mongodb+srv://todo:todo@zavor.yusuq.mongodb.net/todo?retryWrites=true&w=majority");
            var mongoDb = mongoClient.GetDatabase("todo-test");
            _toDoCollection = mongoDb.GetCollection<ToDo>("todo-list-test");
            _mongoDbService = new MongoDbService("todo-test", "todo-list-test");
        }

        [TestMethod]
        public async Task AddTodoTest()
        {
            var todo = new ToDo() { ToDoTitle = "test" };
            await _mongoDbService.AddToDo(todo);

            var findToDo = await _mongoDbService.FindTodo(todo);
            var result = await _toDoCollection.FindAsync(t => t.Id == findToDo.Id);

            var list = result.ToList();
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public async Task DeleteToDoTest()
        {
            var todo = new ToDo();
            await _mongoDbService.AddToDo(todo);
            var findToDo = await _mongoDbService.FindTodo(todo);

            await _mongoDbService.DeleteToDo(findToDo.Id);
            var notExistToDo = await  _toDoCollection.FindAsync(t =>
                t.Id == findToDo.Id);

            Assert.AreEqual(false, await notExistToDo.AnyAsync());
        }

        [TestMethod]
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

        [TestMethod]
        public async Task GetAllToDosTest()
        {
            await _mongoDbService.DeleteAll();
            var todo = new ToDo();
            var todo1 = new ToDo();
            await _mongoDbService.AddToDo(todo);
            await _mongoDbService.AddToDo(todo1);

            var allToDos = await _mongoDbService.GetAllToDos();

            Assert.AreEqual(2, await _toDoCollection.EstimatedDocumentCountAsync());
        }

        [TestMethod]
        public async Task GetToDoByIdTest()
        {
            var todo = new ToDo();
            await _mongoDbService.AddToDo(todo);
            var findToDo = await _mongoDbService.FindTodo(todo);

            var getToDo = await _mongoDbService.GetToDoById(findToDo.Id);

            Assert.AreEqual(findToDo.Id, getToDo.Id);
        }

        [TestMethod]
        public async Task UpdateToDo()
        {
            var todo = new ToDo();
            await _mongoDbService.AddToDo(todo);
            var findToDo = await _mongoDbService.FindTodo(todo);
            findToDo.IsCompleted = true;

            await _mongoDbService.UpdateToDo(findToDo.Id, findToDo);
            var collectionTodo = await _toDoCollection.FindAsync(t => t.Id == findToDo.Id);

            Assert.IsTrue(collectionTodo.ToList().Last().IsCompleted);
        }

        [TestMethod]
        public async Task FindToDoTest()
        {
            var todo = new ToDo();
            await _mongoDbService.AddToDo(todo);
            var findToDo = await _mongoDbService.FindTodo(todo);

            var collectionTodo = await _toDoCollection.FindAsync(t => t.Id == findToDo.Id);

            Assert.AreEqual(findToDo.Id, collectionTodo.ToList().Last().Id);
        }
    }
}
