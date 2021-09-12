using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using AutoFixture;
using Castle.Core.Configuration;
using EmployeeManagement.Application;
using EmployeeManagement.BusinessModel;
using EmployeeManagement.ServiceGateway;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EmployeeManagement.Tests
{

    /// <summary>
    /// Convention for naming used is : "Method_Condition_Expectation"
    /// </summary>
    [TestClass]
    public class EmployeeViewModelTest
    {

        private  Mock<IDialogService> _mockDialogService;
        private  Mock<IEmployeeServiceGateway> _mockEmployeeServiceGateway;
        private  Fixture _fixture = new Fixture();

        [TestInitialize]
        public void TestInitialize()
        {
            _mockDialogService = new Mock<IDialogService>();
            _mockEmployeeServiceGateway = new Mock<IEmployeeServiceGateway>();
        }
        [TestMethod]
        public void GetAndSetEmployees_WithNoSearchCriteria_ShouldGetEmployees()
        {
            //Arrange
            var employees = AllEmployeesMockData();
            _mockEmployeeServiceGateway.Setup(x => x.GetEmployees(1)).ReturnsAsync(employees);
            var employeeViewModel = new EmployeeViewModel(_mockEmployeeServiceGateway.Object, _mockDialogService.Object);
            employeeViewModel.EmployeeSearchCriteria = null;

            //Act

            employeeViewModel.GetAndSetEmployees().GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(employeeViewModel.Employees);
            Assert.IsTrue(employeeViewModel.Employees.Count == 4);
            Assert.AreEqual(employeeViewModel.Employees[0]?.Name, employees[0]?.Name);
            _mockEmployeeServiceGateway.Verify(x => x.GetEmployees(1), Times.Exactly(2));
        }

        [TestMethod]
        public void GetEmployeesByName_ShouldGetFilteredEmployeesByName()
        {
            //Arrange
            var employees = AllEmployeesMockData();
            _mockEmployeeServiceGateway.Setup(x => x.GetEmployeesByName("Nam"))
                .ReturnsAsync(employees.Where(x=>x.Name.Contains("Nam")).ToList());

            var employeeViewModel = new EmployeeViewModel(_mockEmployeeServiceGateway.Object, _mockDialogService.Object);
            employeeViewModel.EmployeeSearchCriteria = "Nam";

            //Act

            employeeViewModel.GetAndSetEmployees().GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(employeeViewModel.Employees);
            Assert.IsTrue(employeeViewModel.Employees.Count == 3);
            Assert.AreEqual(employeeViewModel.Employees[0]?.Name, employees[0]?.Name);
            _mockEmployeeServiceGateway.Verify(x => x.GetEmployeesByName(employeeViewModel.EmployeeSearchCriteria), Times.Once);
            _mockEmployeeServiceGateway.Verify(x=>x.GetEmployees(1),Times.Once);
        }

        [TestMethod]
        public void GetEmployeesById_ShouldGetFilteredEmployeesById()
        {
            //Arrange
            var employees = AllEmployeesMockData();

            var expectedEmployee = employees.FirstOrDefault(x => x.Id == 2);
            _mockEmployeeServiceGateway.Setup(x => x.GetEmployeeById(2))
                .ReturnsAsync(expectedEmployee);

            var employeeViewModel = new EmployeeViewModel(_mockEmployeeServiceGateway.Object, _mockDialogService.Object);
            employeeViewModel.EmployeeSearchCriteria = "2";

            //Act

            employeeViewModel.GetAndSetEmployees().GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(employeeViewModel.Employees);
            Assert.IsTrue(employeeViewModel.Employees.Count == 1);
            Assert.AreEqual(employeeViewModel.Employees[0]?.Id, expectedEmployee?.Id);

            _mockEmployeeServiceGateway.Verify(x => x.GetEmployeeById(Convert.ToInt32(employeeViewModel.EmployeeSearchCriteria)), Times.Once);
            _mockEmployeeServiceGateway.Verify(x => x.GetEmployeesByName(employeeViewModel.EmployeeSearchCriteria), Times.Never);
            _mockEmployeeServiceGateway.Verify(x => x.GetEmployees(1), Times.Once);
        }


        [TestMethod]
        public void GettingEmployees_WhenNetworkFailure_ShouldShowErrorMessageToUser()
        {
            //Arrange
            var employees = AllEmployeesMockData();
            _mockEmployeeServiceGateway.Setup(x => x.GetEmployees(1))
                .ThrowsAsync(new Exception(It.IsAny<string>()));
            var employeeViewModel = new EmployeeViewModel(_mockEmployeeServiceGateway.Object, _mockDialogService.Object);


            _mockDialogService.Setup(x=>x.ShowErrorMessageBox(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            employeeViewModel.EmployeeSearchCriteria = null;

            //Act

            employeeViewModel.GetAndSetEmployees().GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(employeeViewModel.Employees);
            _mockDialogService.Verify(x=>x.ShowErrorMessageBox(It.IsAny<string>(), It.IsAny<string>()),Times.Exactly(2));
            _mockEmployeeServiceGateway.Verify(x => x.GetEmployees(1), Times.Exactly(2));
        }


        private List<Employee> AllEmployeesMockData()
        {
            var employees = new List<Employee>()
            {
                new Employee()
                {
                    Id = 1,
                    Email = "abc@test.com",
                    Gender = "Male",
                    Name = "Name1",
                    Status = "Active"
                },
                new Employee()
                {
                    Id =2,
                    Email = "abcdef@test.com",
                    Gender = "Male",
                    Name = "Name2",
                    Status = "Active"
                },

                new Employee()
                {
                    Id = 3,
                    Email = "xyz@test.com",
                    Gender = "Male",
                    Name = "Name3",
                    Status = "Active"
                },
                new Employee()
                {
                    Id = 4,
                    Email = "abc@test.com",
                    Gender = "Male",
                    Name = "Amit",
                    Status = "Active"
                },

            };
            return employees;

        }
    }
}
