using System.Collections.Generic;
using System.Threading.Tasks;
using TutorialApi.Models;

namespace TutorialApi.Repositories
{
    public interface IPersonRepository
    {
        Task<List<Person>> GetPeopleListAsync();

        Task<Person> GetPersonByIdAsync(long id);

        Task PostPersonAsync(Person person);

        Task<string> GetPersonName(TodoItem todoItem);
    }
}
