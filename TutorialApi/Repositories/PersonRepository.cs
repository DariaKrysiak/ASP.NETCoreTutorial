using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TutorialApi.Models;

namespace TutorialApi.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly TodoContext _dbContext;

        public PersonRepository(TodoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Person>> GetPeopleListAsync()
        {
            return _dbContext.People.ToListAsync();
        }

        public Task<Person> GetPersonByIdAsync(long id)
        {
            return _dbContext.People.FindAsync(id);
        }

        public Task PostPersonAsync(Person person)
        {
            _dbContext.People.Add(person);
            return _dbContext.SaveChangesAsync();
        }
    }
}
