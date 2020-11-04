using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace WebApplication
{
    public interface IMongoDbService
    {
        Task AddToDo(IToDo toDo);
        Task DeleteToDo(int id);
        Task DeleteAll();
        Task<List<IToDo>> GetAllToDos();
        Task<IToDo> GetToDoById(int id);
        Task UpdateToDo(int id, ToDo todo);
    }
}
