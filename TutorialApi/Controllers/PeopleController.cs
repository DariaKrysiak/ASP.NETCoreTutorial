using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TutorialApi.Models;
using TutorialApi.Repositories;

namespace TutorialApi.Controllers
{
    [Route("api/people")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonRepository _itemRepository;

        public PeopleController(IPersonRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> GetPeople()
        {
            var people = await _itemRepository.GetPeopleListAsync();

            if (people.Count == 0)
            {
                throw new InvalidOperationException("People list is empty");
            }

            return people;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            var person = await _itemRepository.GetPersonByIdAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            await _itemRepository.PostPersonAsync(person);

            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        }
    }
}
