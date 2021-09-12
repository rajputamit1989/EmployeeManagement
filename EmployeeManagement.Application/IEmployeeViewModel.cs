using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EmployeeManagement.BusinessModel;

namespace EmployeeManagement.Application
{
    public interface IEmployeeViewModel
    {
        /// <summary>
        /// Get employees
        /// </summary>
        /// <returns></returns>
        Task GetAndSetEmployees();

        /// <summary>
        /// Delete particular employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task DeleteEmployee(Employee employee);

        /// <summary>
        /// Reset all employees and reset the records
        /// </summary>
        /// <returns></returns>
        Task ResetEmployees();
    }
}
