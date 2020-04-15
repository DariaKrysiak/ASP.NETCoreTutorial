using System;
using System.Collections.Generic;

namespace TutorialApi.Models
{
    public class TodoItem : ITodo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        public Nullable<long> PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}
