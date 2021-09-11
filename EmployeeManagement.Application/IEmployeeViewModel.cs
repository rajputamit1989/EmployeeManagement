using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EmployeeManagement.BusinessModel;

namespace EmployeeManagement.Application
{
    public interface IEmployeeViewModel
    {
    Task GetAndSetEmployees(int? page=null);
        //Task<Employee> GetEmployeeById(int employeeId);

        //Task<ObservableCollection<Employee>> GetEmployeesByName(string name);

        Task DeleteEmployee(Employee employee);
    }
}
