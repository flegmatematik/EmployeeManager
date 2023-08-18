using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Shared.Dtos
{
    public class ImportDataDTO
    {
        public string? JsonFile { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
    }
}
