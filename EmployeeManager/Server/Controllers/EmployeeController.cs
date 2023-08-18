using EmployeeManager.Server.Data;
using EmployeeManager.Shared.JsonObjects;
using EmployeeManager.Shared.Models;
using ipbase;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace EmployeeManager.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private Ipbase Ipbase;

        public EmployeeController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployees()
        {
            var employees = await _context.Employees
                .Include(x => x.Position)
                .ToListAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _context.Employees
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.EmployeeId == id);
            if(employee == null)
            {
                return NotFound(string.Format("Employee with id {0} was not found", id));
            }
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<List<Employee>>> CreateEmployee(Employee employee)
        {
            employee.Position = null;
            employee.IPCountryCode = GetCountyCode(employee.IPCountryCode);

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return Ok(await GetDbEmployees());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Employee>>> UpdateEmployee(Employee employee, int id)
        {
            var dbEmployee = await _context.Employees
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.EmployeeId == id);
            if(dbEmployee == null)
            {
                return NotFound(string.Format("Employee with id {0} was not found", id));
            }

            dbEmployee.Name = employee.Name;
            dbEmployee.Surname = employee.Surname;
            dbEmployee.BirthDate = employee.BirthDate;
            dbEmployee.PositionId = employee.PositionId;
            dbEmployee.IPAddress = employee.IPAddress;
            dbEmployee.IPCountryCode = GetCountyCode(employee.IPAddress);

            await _context.SaveChangesAsync();

            return Ok(await GetDbEmployees());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Employee>>> DeleteEmployee(int id)
        {
            var dbEmployee = await _context.Employees
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (dbEmployee == null)
            {
                return NotFound(string.Format("Employee with id {0} was not found", id));
            }
            
            _context.Employees.Remove(dbEmployee);

            await _context.SaveChangesAsync();

            return Ok(await GetDbEmployees());
        }

        [HttpGet("positions")]
        public async Task<ActionResult<List<Position>>> GetPositions()
        {
            var positions = await _context.Positions.ToListAsync();
            return Ok(positions);
        }

        [HttpGet("positions/{id}")]
        public async Task<ActionResult<Employee>> GetPositionById(int id)
        {
            var position = await _context.Positions
                .FirstOrDefaultAsync(x => x.PositionId == id);
            if (position == null)
            {
                return NotFound(string.Format("Position with id {0} was not found", id));
            }
            return Ok(position);
        }

        [HttpPost("positions")]
        public async Task<ActionResult<List<Position>>> CreatePosition(Position position)
        {
            if(_context.Positions.FirstOrDefault(x => x.PositionName == position.PositionName) == null)
            {
                _context.Positions.Add(position);
                await _context.SaveChangesAsync();
            }
            return Ok(await GetDbPositions());
        }

        [HttpPut("positions/{id}")]
        public async Task<ActionResult<List<Position>>> UpdatePosition(Position position, int id)
        {
            var dbPosition = await _context.Positions
                .FirstOrDefaultAsync(x => x.PositionId == id);
            if (dbPosition == null)
            {
                return NotFound(string.Format("Position with id {0} was not found", id));
            }

            dbPosition.PositionName = position.PositionName;

            await _context.SaveChangesAsync();

            return Ok(await GetDbPositions());
        }

        [HttpDelete("positions/{id}")]
        public async Task<ActionResult<List<Employee>>> DeletePosition(int id)
        {
            var dbPosition = await _context.Positions
                .FirstOrDefaultAsync(x => x.PositionId == id);
            if (dbPosition == null)
            {
                return NotFound(string.Format("Position with id {0} was not found", id));
            }
            if (_context.Employees.Any(x => x.PositionId == id))
            {
                return Problem(string.Format("Position with id {0} is assigned to a existing employee. ", id));
            }

            _context.Positions.Remove(dbPosition);

            await _context.SaveChangesAsync();

            return Ok(await GetDbPositions());
        }

        [HttpPost("importdata")]
        public async Task<ActionResult<List<Employee>>> ImportData(JsonFile jsonData)
        {
            if(jsonData == null)
            {
                return BadRequest("Json Parameter is missing");
            }

            foreach (var jsonPosition in jsonData.PositionsList)
            {
                Position position = new Position { PositionName = jsonPosition };
                await CreatePosition(position);
            }
            foreach (var jsonEmployee in jsonData.EmployeeList)
            {
                Employee employee = JsonToEmployee(jsonEmployee);
                await CreateEmployee(employee);
            }

            return Ok(await GetDbEmployees());
        }


        private async Task<List<Employee>> GetDbEmployees()
        {
            return await _context.Employees.Include(x => x.Position).ToListAsync();
        }

        private async Task<List<Position>> GetDbPositions()
        {
            return await _context.Positions.ToListAsync();
        }

        private string GetCountyCode(string ipAddress)
        {
            try
            {
                if (ipAddress != null)
                {
                    if (Ipbase == null)
                    {
                        Ipbase = new Ipbase(_configuration.GetValue<string>("IPBaseAPIKey"));
                    }
                    var IpBaseJson = Ipbase?.Info(ipAddress);
                    if (IpBaseJson != null)
                    {
                        dynamic jsonObject = JsonConvert.DeserializeObject(IpBaseJson);
                        var code = jsonObject.data.location.country.alpha3;
                        return code;
                    }
                }
                return "Not recognized";
            }
            catch (Exception ex)
            {
                return "Not recognized";
            }
        }

        private Employee JsonToEmployee(JsonEmployee json)
        {
            Employee employee = new Employee
            {
                Name = json.Name,
                Surname = json.Surname,
                BirthDate = DateTime.Parse(json.BirthDate),
                IPAddress = json.IpAddress,
                PositionId = _context.Positions.FirstOrDefault(x => x.PositionName == json.Position)?.PositionId
            };

            return employee;
        }
    }
}
