using EmployeeManagement.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.ServiceGateway
{
    public interface IEmployeeServiceGateway
    {
        Task<List<Employee>> GetEmployees(int? page = null);
        Task<Employee> GetEmployeeById(int employeeId);

        Task<List<Employee>> GetEmployeesByName(string name);

        Task<Employee> AddEmployee(Employee employee);

        Task<Employee> UpdateEmployee(Employee employee);

        Task<Employee> DeleteEmployee(int employeeId);

    }
}
