using EmployeeManager.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace EmployeeManager.Client.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigation;

        public EmployeeService(HttpClient httpClient, NavigationManager navigation)
        {
            _httpClient = httpClient;
            _navigation = navigation;
        }

        public List<Employee> Employees { get; set; } = new List<Employee>();
        public List<Position> Positions { get; set; } = new List<Position>();

        public async Task GetEmployees()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Employee>>("api/employee");
            if (result != null)
            {
                Employees = result;
            }
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<Employee>($"api/employee/{id}");
            if (result != null)
            {
                return result;
            }
            throw new Exception("Employee not found");
        }

        public async Task CreateEmployee(Employee employee)
        {
            var result = await _httpClient.PostAsJsonAsync("api/employee", employee);
            await SetEmployees(result);
        }

        public async Task UpdateEmployee(Employee employee)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/employee/{employee.EmployeeId}", employee);
            await SetEmployees(result);
        }

        public async Task DeleteEmployee(int id)
        {
            var result = await _httpClient.DeleteAsync($"api/employee/{id}");
            await SetEmployees(result);
        }

        public async Task GetPositions()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Position>>("api/employee/positions");
            if (result != null)
            {
                Positions = result;
            }
        }

        public async Task<Position> GetPositionById(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<Position>($"api/employee/positions/{id}");
            if (result != null)
                return result;
            throw new Exception("Position not found");
        }


        public async Task CreatePosition(Position position)
        {
            var result = await _httpClient.PostAsJsonAsync("api/employee/position", position);
            await SetPositions(result);
        }

        public async Task UpdatePosition(Position position)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/employee/positions/{position.PositionId}", position);
            await SetPositions(result);
        }

        public async Task DeletePosition(int id)
        {
            var result = await _httpClient.DeleteAsync($"api/employee/positions/{id}");
            await SetPositions(result);
        }

        public async Task ImportData(JsonFile jsonstring)
        {
            var result = await _httpClient.PostAsJsonAsync("api/employee/importdata", jsonstring);
            if(jsonstring.EmployeeList.Any())
                await SetEmployees(result);
            else
                await SetPositions(result);
        }

        private async Task SetEmployees(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<Employee>>();
            Employees = response;
            _navigation.NavigateTo("employees");
        }

        private async Task SetPositions(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<Position>>();
            Positions = response;
            _navigation.NavigateTo("positions");
        }
    }
}
