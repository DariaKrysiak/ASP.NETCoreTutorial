using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorialApi.Models;
using TutorialApi.WebModels;

namespace TutorialApi.Controllers
{
    [Route("api/Todo")]
    [ApiController]
    public class TodoWithPersonsNamesController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoWithPersonsNamesController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet("withPersonsNames")]
        public async Task<ActionResult<IEnumerable<TodoItemWithPersonsName>>> GetTodoItemsWithPersonsNames()
        {
            var items = await _context.TodoItems.ToListAsync();

            if (items == null)
            {
                return new NotFoundResult();
            }

            var itemsWithPersonsNames = new List<TodoItemWithPersonsName>();

            foreach (var item in items)
            {
                var itemWithPersonsName = new TodoItemWithPersonsName()
                {
                    TodoItemName = item.Name,
                    IsComplete = item.IsComplete,
                    PersonName = await GetPersonName(item)
                };

                itemsWithPersonsNames.Add(itemWithPersonsName);
            }

            return new OkObjectResult(itemsWithPersonsNames);
        }

        private async Task<string> GetPersonName(TodoItem todoItem)
        {
            var person = await _context.People
                .Where(p => p.Id.Equals(todoItem.PersonId))
                .SingleOrDefaultAsync();
            return person.FirstName;
        }
    }
}
