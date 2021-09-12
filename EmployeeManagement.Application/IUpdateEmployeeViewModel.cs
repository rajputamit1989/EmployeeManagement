using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EmployeeManagement.BusinessModel;

namespace EmployeeManagement.Application
{
    public interface IUpdateEmployeeViewModel
    {
        Task<Employee> UpdateEmployee();
    }
}
