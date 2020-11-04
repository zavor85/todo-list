using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApplication
{
    public class MongoDbService : IMongoDbService
    {
        private IMongoCollection<ToDo> _toDoCollection { get; set; }

        public MongoDbService(string dbName, string collectionName)
        {
            var mongoClient = new MongoClient("mongodb+srv://todo:todo@zavor.yusuq.mongodb.net/todo?retryWrites=true&w=majority");
            var mongoDb = mongoClient.GetDatabase(dbName);
            _toDoCollection = mongoDb.GetCollection<ToDo>(collectionName);
        }

        public async Task AddToDo(IToDo toDo)
        {
            var documents = await _toDoCollection.FindAsync(new BsonDocument());
            var documentList = documents.ToList();
            if (documentList.Count == 0)
            {
                toDo.Id = 0;
            }
            else
            {
                toDo.Id = documentList.Last().Id + 1;
            }
            
            await _toDoCollection.InsertOneAsync((ToDo)toDo);
        }

        public async Task DeleteToDo(int id) => await _toDoCollection.DeleteOneAsync(t=> t.Id == id);

        public async Task DeleteAll() => await _toDoCollection.DeleteManyAsync(new BsonDocument());

        public async Task<List<IToDo>> GetAllToDos()
        {
            var toDos = new List<IToDo>();
            var documents = await _toDoCollection.FindAsync(new BsonDocument());
            await documents.ForEachAsync(todo => toDos.Add(todo));
            return toDos;
        }

        public async Task<IToDo> GetToDoById(int id)
        {
            var toDo = await _toDoCollection.FindAsync(todo => todo.Id == id);
            return toDo.ToList().Last();
        }

        public async Task UpdateToDo(int id, ToDo todo)
        {
            var filter = Builders<ToDo>.Filter.Eq(t=>t.Id, id);
            var update = Builders<ToDo>.Update.Set(t => t.IsCompleted, todo.IsCompleted);
            await _toDoCollection.FindOneAndUpdateAsync(filter, update);
        }

        public async Task<ToDo> FindTodo(ToDo todo)
        {
            var result = await _toDoCollection.FindAsync(t => t.Id == todo.Id);
            return result.ToList().Last();
        }
    }
}
