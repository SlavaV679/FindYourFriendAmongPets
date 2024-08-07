using FindYourFriendAmongPets.API.Contracts;
using FindYourFriendAmongPets.Core.Abstractions;
using FindYourFriendAmongPets.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace FindYourFriendAmongPets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoItemController : ControllerBase
    {
        private readonly IToDoItemService _toDoItemService;

        public ToDoItemController(IToDoItemService toDoItemService)
        {
            _toDoItemService = toDoItemService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ToDoItemsResponse>>> GetToDoItems()
        {
            var toDoItems = await _toDoItemService.GetAllToDoItems();

            var response = toDoItems.Select(x => new ToDoItemsResponse(x.Id, x.Title, x.Description, x.DateCreated));

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateToDoItem([FromBody] ToDoItemsRequest request)
        {
            var (toDoItem, error) = ToDoItem.Create(
                Guid.NewGuid(),
                request.Title,
                request.Description,
                DateTime.Now.ToUniversalTime());

            if(!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var id = await _toDoItemService.CreateToDoItem(toDoItem);
            
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateToDoItem(Guid id, [FromBody] ToDoItemsRequest request)
        {
            var toDoItemId = await _toDoItemService.UpdateToDoItem(id, request.Title, request.Description);

            return Ok(toDoItemId);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteToDoItem(Guid id)
        {
            var toDoItemId = await _toDoItemService.DeleteToDoItem(id);
            
            return Ok(toDoItemId);
        }
    }
}
