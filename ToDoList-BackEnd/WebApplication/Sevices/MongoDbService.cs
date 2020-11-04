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
            var mongoClient = new MongoClient("your connection string");
            var mongoDb = mongoClient.GetDatabase(dbName);
            _toDoCollection = mongoDb.GetCollection<ToDo>(collectionName);
        }

        public async Task AddToDo(ToDo toDo)
        {
            var documentList = await _toDoCollection.AsQueryable().ToListAsync();
            if (documentList.Count == 0)
            {
                toDo.Id = 0;
            }
            else
            {
                toDo.Id = documentList.Last().Id + 1;
            }
            
            await _toDoCollection.InsertOneAsync(toDo);
        }

        public async Task DeleteToDo(int id) => await _toDoCollection.DeleteOneAsync(t=> t.Id == id);

        public async Task DeleteAll() => await _toDoCollection.DeleteManyAsync(new BsonDocument());

        public async Task<List<ToDo>> GetAllToDos()
        {
            var todoList = await _toDoCollection.AsQueryable().ToListAsync();
            return todoList;
        }

        public async Task<IToDo> GetToDoById(int id)
        {
            var toDo = await _toDoCollection.Find(todo => todo.Id == id).ToListAsync();
            return toDo.Last();
        }

        public async Task UpdateToDo(int id, ToDo todo)
        {
            var filter = Builders<ToDo>.Filter.Eq(t=>t.Id, id);
            var update = Builders<ToDo>.Update.Set(t => t.IsCompleted, todo.IsCompleted);
            await _toDoCollection.FindOneAndUpdateAsync(filter, update);
        }
    }
}
