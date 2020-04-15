using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TutorialApi.Models
{
    public class Person : IPerson
    {
        public Person()
        {
            this.TodoItems = new HashSet<TodoItem>();
        }

        public long Id { get; set; }
        public string FirstName { get; set; }

        public virtual ICollection<TodoItem> TodoItems { get; set; }
    }
}
