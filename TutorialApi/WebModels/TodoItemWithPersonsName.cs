using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TutorialApi.WebModels
{
    public class TodoItemWithPersonsName
    {
        public string TodoItemName { get; set; }
        public bool IsComplete { get; set; }
        public string PersonName { get; set; }
    }
}
