using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication
{
    
    [Route("todo-api/")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly IMongoDbService _mongoDbService = new MongoDbService("todo", "todo-list"); // (DBName, DBCollection)

        [HttpGet]
        public async Task<List<ToDo>> Get()
        {
            var allToDos =  await _mongoDbService.GetAllToDos();
            return allToDos;
        }

        [HttpGet("{id}")]
        public async Task<IToDo> Get(int id)
        {
            var toDo = await _mongoDbService.GetToDoById(id);
            return toDo;
        }

        [HttpPost]
        public async Task Post([FromBody] ToDo todo)
        {
            await _mongoDbService.AddToDo(todo);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _mongoDbService.DeleteToDo(id);
        }

        [HttpDelete]
        public async Task Delete()
        {
            await _mongoDbService.DeleteAll();
        }

        [HttpPut("{id}")]
        public async Task Update(int id, ToDo todo)
        {
            await _mongoDbService.UpdateToDo(id, todo);
            
        }
    }
}
