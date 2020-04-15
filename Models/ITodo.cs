using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TutorialApi.Models
{
    public interface ITodo
    {
        long Id { get; set; }
        string Name { get; set; }
        bool IsComplete { get; set; }
    }
}
