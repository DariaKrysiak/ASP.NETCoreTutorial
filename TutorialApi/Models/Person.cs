using System.Collections.Generic;

namespace TutorialApi.Models
{
    public class Person
    {
        public long Id { get; set; }
        public string FirstName { get; set; }

        public virtual ICollection<TodoItem> TodoItems { get; set; }
    }
}
