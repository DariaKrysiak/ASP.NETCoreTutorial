using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TutorialApi.Models
{
    public interface IPerson
    {
        long Id { get; set; }
        string FirstName { get; set; }
    }
}
