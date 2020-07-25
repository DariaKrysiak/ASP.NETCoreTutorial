using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TutorialApi.Models;
using TutorialApi.Repositories;

namespace TutorialApi.Controllers
{
    [Route("api/Todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoItemRepository _itemRepository;

        public TodoController(ITodoItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<TodoItem>>> GetTodoItems()
        {
            var items = await _itemRepository.GetItemsListAsync();

            if (items.Count == 0)
            {
                throw new InvalidOperationException("Items list is empty");
            }

            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _itemRepository.GetItemByIdAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            await _itemRepository.PostItemAsync(item);

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            await _itemRepository.PutItemAsync(item);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _itemRepository.GetItemByIdAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            await _itemRepository.DeleteItemAsync(todoItem);

            return NoContent();
        }
    }
}
