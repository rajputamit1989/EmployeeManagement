using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EmployeeManagement.BusinessModel;

namespace EmployeeManagement.Application
{
    public interface IAddEmployeeViewModel
    {
        Task<Employee> AddNewEmployee();
    }
}
