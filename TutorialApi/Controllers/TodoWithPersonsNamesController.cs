using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorialApi.Models;
using TutorialApi.Repositories;
using TutorialApi.WebModels;

namespace TutorialApi.Controllers
{
    [Route("api/Todo")]
    [ApiController]
    public class TodoWithPersonsNamesController : ControllerBase
    {
        private readonly ITodoItemRepository _itemRepository;
        private readonly IPersonRepository _personRepository;

        public TodoWithPersonsNamesController(ITodoItemRepository itemRepository, IPersonRepository personRepository)
        {
            _itemRepository = itemRepository;
            _personRepository = personRepository;
        }

        [HttpGet("withPersonsNames")]
        public async Task<ActionResult<IEnumerable<TodoItemWithPersonsName>>> GetTodoItemsWithPersonsNames()
        {
            var items = await _itemRepository.GetItemsListAsync();

            if (items.Count == 0)
            {
                throw new InvalidOperationException("Items list is empty");
            }

            var itemsWithPersonsNames = new List<TodoItemWithPersonsName>();

            foreach (var item in items)
            {
                var itemWithPersonsName = new TodoItemWithPersonsName()
                {
                    TodoItemName = item.Name,
                    IsComplete = item.IsComplete,
                    PersonName = await _personRepository.GetPersonName(item)
                };

                itemsWithPersonsNames.Add(itemWithPersonsName);
            }

            return new OkObjectResult(itemsWithPersonsNames);
        }
    }
}
