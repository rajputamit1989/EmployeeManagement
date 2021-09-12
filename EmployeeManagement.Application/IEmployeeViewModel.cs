﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EmployeeManagement.BusinessModel;

namespace EmployeeManagement.Application
{
    public interface IEmployeeViewModel
    {
        Task GetAndSetEmployees();
        Task DeleteEmployee(Employee employee);
        Task ResetEmployees();
    }
}
