using EmployeeManager.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Shared.JsonObjects
{
    public class JsonFile
    {
        [JsonProperty("employes")]
        public List<JsonEmployee> EmployeeList { get; set; }

        [JsonProperty("postitions")]
        public List<string> PositionsList { get; set; }

        public JsonFile()
        {
            EmployeeList = new List<JsonEmployee>();
            PositionsList = new List<string>();
        }
    }

    public class JsonEmployee
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;

    }


}
