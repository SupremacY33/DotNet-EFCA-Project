using SchoolManagement.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Core.Entities
{
    public class Students : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateOnly JoinDate { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public required string Gender { get; set; }
        public required string Nationality { get; set; }
        public required string Religion { get; set; }
        public required string Address { get; set; }

    }
}
