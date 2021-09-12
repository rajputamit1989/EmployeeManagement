using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EmployeeManagement.BusinessModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EmployeeManagement.ServiceGateway
{
    public class EmployeeServiceGateway : IEmployeeServiceGateway
    {
        private readonly HttpClient _client = new();
        public EmployeeServiceGateway()
        {
            _client.BaseAddress = new Uri("https://gorest.co.in/public-api/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56");
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<List<Employee>> GetEmployees(int? page = null)
        {
            HttpResponseMessage response = null;
            if (page.HasValue)
                response = await _client.GetAsync("users?page=" + page).ConfigureAwait(false);
            else
                response = await _client.GetAsync("users").ConfigureAwait(false);

            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false); ;
            List<Employee> employees = null;
            if (response.IsSuccessStatusCode)
            {
                var employeesString = Convert.ToString(JObject.Parse(responseData)["data"]);
                employees = !string.IsNullOrWhiteSpace(employeesString)
                    ? JsonConvert.DeserializeObject<List<Employee>>(employeesString)
                    : null;
            }
            return employees;
        }

        public async Task<Employee> GetEmployeeById(int employeeId)
        {
            HttpResponseMessage response = await _client.GetAsync("users/" + employeeId).ConfigureAwait(false);
            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false); ;
            Employee employee = null;
            if (response.IsSuccessStatusCode)
            {
                var employeeString = Convert.ToString(JObject.Parse(responseData)["data"]);
                employee = !string.IsNullOrWhiteSpace(employeeString)
                    ? JsonConvert.DeserializeObject<Employee>(employeeString)
                    : null;
            }
            return employee;
        }

        public async Task<List<Employee>> GetEmployeesByName(string name)
        {
            HttpResponseMessage response = await _client.GetAsync("users?name=" + name).ConfigureAwait(false);
            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false); ;
            List<Employee> employees = null;
            if (response.IsSuccessStatusCode)
            {
                var employeesString = Convert.ToString(JObject.Parse(responseData)["data"]);
                employees = !string.IsNullOrWhiteSpace(employeesString)
                    ? JsonConvert.DeserializeObject<List<Employee>>(employeesString)
                    : null;
            }
            return employees;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(
                "users", employee).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false); ;

            var employeeString = Convert.ToString(JObject.Parse(responseData)["data"]);
            employee = !string.IsNullOrWhiteSpace(employeeString)
                ? JsonConvert.DeserializeObject<Employee>(employeeString)
                : null;
            return employee;
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync(
                $"users/{employee.Id}", employee);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false); ;

            var employeeString = Convert.ToString(JObject.Parse(responseData)["data"]);
            employee = !string.IsNullOrWhiteSpace(employeeString)
                ? JsonConvert.DeserializeObject<Employee>(employeeString)
                : null;
            return employee;
        }

        public async Task<Employee> DeleteEmployee(int employeeId)
        {
            HttpResponseMessage response = await _client.DeleteAsync(
                $"users/{employeeId}");
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false); ;

            var employeeString = Convert.ToString(JObject.Parse(responseData)["data"]);
            var deletedEmployee = !string.IsNullOrWhiteSpace(employeeString)
                ? JsonConvert.DeserializeObject<Employee>(employeeString)
                : null;
            return deletedEmployee;
        }
    }
}
