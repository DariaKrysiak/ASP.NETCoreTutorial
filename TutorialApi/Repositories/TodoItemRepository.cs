using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TutorialApi.Models;

namespace TutorialApi.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly TodoContext _dbContext;

        public TodoItemRepository(TodoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<TodoItem>> GetItemsListAsync()
        {
            return _dbContext.TodoItems.ToListAsync();
        }

        public Task<TodoItem> GetItemByIdAsync(long id)
        {
            return _dbContext.TodoItems.FindAsync(id);
        }

        public Task PostItemAsync(TodoItem item)
        {
            _dbContext.TodoItems.Add(item);
            return _dbContext.SaveChangesAsync();
        }

        public Task PutItemAsync(TodoItem item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteItemAsync(TodoItem item)
        {
            _dbContext.TodoItems.Remove(item);
            return _dbContext.SaveChangesAsync();
        }
    }
}
