using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Shared.Models
{
    public class Employee
    {

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; } = DateTime.Now;

        public int? PositionId { get; set; }

        public Position? Position { get; set; }

        [Required]
        public string IPAddress { get; set; } = string.Empty;

        public string IPCountryCode { get; set; } = string.Empty;
    }
}
