using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.Sevices;


namespace WebApplication.Controllers
{
    
    [Route("todo-api/")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService = new MongoDbService("todo", "todo-list"); // (DBName, DBCollection)

        [HttpGet]
        public async Task<List<ToDo>> Get()
        {
            var allTodos = await _mongoDbService.GetAllToDos();
            return allTodos;
        }

        [HttpGet("{id}")]
        public async Task<ToDo> Get(int id)
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
        public async Task<ToDo> Update(int id, ToDo todo)
        {
            var newToDo = await _mongoDbService.UpdateToDo(id, todo);
            return newToDo;
        }
    }
}
