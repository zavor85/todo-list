using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApplication.Models;

namespace WebApplication.Sevices
{
    public class MongoDbService 
    {
        private IMongoCollection<ToDo> _toDoCollection { get; }

        public MongoDbService(string dbName, string collectionName)
        {
            var mongoClient = new MongoClient("connection string to mongodb atlas or compass");
            var mongoDb = mongoClient.GetDatabase(dbName);
            _toDoCollection = mongoDb.GetCollection<ToDo>(collectionName);
        }

        public async Task AddToDo(ToDo toDo)
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
            
            await _toDoCollection.InsertOneAsync(toDo);
        }

        public async Task DeleteToDo(int id) => await _toDoCollection.DeleteOneAsync(t=> t.Id == id);

        public async Task DeleteAll() => await _toDoCollection.DeleteManyAsync(new BsonDocument());

        public async Task<List<ToDo>> GetAllToDos()
        {
            var toDos = new List<ToDo>();
            var documents = await _toDoCollection.FindAsync(new BsonDocument());
            await documents.ForEachAsync(todo => toDos.Add(todo));
            return toDos;
        }

        public async Task<ToDo> GetToDoById(int id)
        {
            var toDo = await _toDoCollection.FindAsync(todo => todo.Id == id);
            return toDo.First();
        }

        public async Task<ToDo> UpdateToDo(int id, ToDo todo)
        {
            var filter = Builders<ToDo>.Filter.Eq(t=>t.Id, id);
            var update = Builders<ToDo>.Update.Set(t => t.IsCompleted, todo.IsCompleted);
            await _toDoCollection.FindOneAndUpdateAsync(filter, update);
            var toDo = await _toDoCollection.FindAsync(t => t.Id == id);
            return toDo.First();
        }
    }
}
