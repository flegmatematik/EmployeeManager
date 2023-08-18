using Microsoft.AspNetCore.Components.Forms;

namespace EmployeeManager.Client.Services.EmployeeService
{
    public interface IEmployeeService
    {
        List<Employee> Employees { get; set; }
        List<Position> Positions { get; set; }

        Task GetEmployees();
        Task<Employee> GetEmployeeById(int id);
        Task CreateEmployee(Employee employee);
        Task UpdateEmployee(Employee employee);
        Task DeleteEmployee(int id);


        Task GetPositions();
        Task<Position> GetPositionById(int id);
        Task CreatePosition(Position position);
        Task UpdatePosition(Position position);
        Task DeletePosition(int id);

        Task ImportData(JsonFile jsonstring);
    }
}
