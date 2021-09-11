using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.BusinessModel;

namespace EmployeeManagement.ServiceGateway
{
    public class EmployeeServiceGateway :IEmployeeServiceGateway
    {
        public Task<List<Employee>> GetEmployees(int? page = null)
        {
            var employees=new List<Employee>();
            employees.Add(new Employee(){Id = 1,Email = "asd@fdfd.com",Gender = "Male",Name="Amit",Status="Active"});
            employees.Add(new Employee() { Id = 2, Email = "fdsf@fdfd.com", Gender = "Male", Name = "Abhay", Status = "Active" });
            employees.Add(new Employee() { Id = 3, Email = "sdfdsf@fdfd.com", Gender = "Male", Name = "Dhiru", Status = "Active" });

            return Task.FromResult(employees) ;
        }

        public Task<Employee> GetEmployeeById(int employeeId)
        {
            var employees = new List<Employee>();
            employees.Add(new Employee() { Id = 1, Email = "asd@fdfd.com", Gender = "Male", Name = "Amit", Status = "Active" });
            employees.Add(new Employee() { Id = 2, Email = "fdsf@fdfd.com", Gender = "Male", Name = "Abhay", Status = "Active" });
            employees.Add(new Employee() { Id = 3, Email = "sdfdsf@fdfd.com", Gender = "Male", Name = "Dhiru", Status = "Active" });
            return Task.FromResult(employees.FirstOrDefault(x => x.Id.Equals(employeeId)));
        }

        public Task<List<Employee>> GetEmployeesByName(string name)
        {
            var employees = new List<Employee>();
            employees.Add(new Employee() { Id = 1, Email = "asd@fdfd.com", Gender = "Male", Name = "Amit", Status = "Active" });
            employees.Add(new Employee() { Id = 2, Email = "fdsf@fdfd.com", Gender = "Male", Name = "Abhay", Status = "Active" });
            employees.Add(new Employee() { Id = 3, Email = "sdfdsf@fdfd.com", Gender = "Male", Name = "Dhiru", Status = "Active" });
            return Task.FromResult(employees.Where(x => x.Name.Equals(name)).ToList());

        }

        public Task<Employee> AddEmployee(int employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> UpdateEmployee(int employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> DeleteEmployee(int employeeId)
        {
            throw new NotImplementedException();
        }
    }
}
