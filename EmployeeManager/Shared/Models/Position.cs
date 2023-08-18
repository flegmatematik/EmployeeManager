using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Shared.Models
{
    public class Position
    {
        public int PositionId { get; set; }

        [Required]
        public string PositionName { get; set; } = string.Empty;
    }
}
