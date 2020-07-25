using System.Collections.Generic;
using System.Threading.Tasks;
using TutorialApi.Models;

namespace TutorialApi.Repositories
{
    public interface ITodoItemRepository
    {
        Task<List<TodoItem>> GetItemsListAsync();

        Task<TodoItem> GetItemByIdAsync(long id);

        Task PostItemAsync(TodoItem item);

        Task PutItemAsync(TodoItem item);

        Task DeleteItemAsync(TodoItem item);
    }
}
