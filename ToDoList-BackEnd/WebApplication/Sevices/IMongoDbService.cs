using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication
{
    public interface IMongoDbService
    {
        Task AddToDo(ToDo toDo);
        Task DeleteToDo(int id);
        Task DeleteAll();
        Task<List<ToDo>> GetAllToDos();
        Task<IToDo> GetToDoById(int id);
        Task UpdateToDo(int id, ToDo todo);
    }
}
