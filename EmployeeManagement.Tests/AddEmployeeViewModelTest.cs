using System.Collections.Generic;
using EmployeeManagement.Application;
using EmployeeManagement.BusinessModel;
using EmployeeManagement.ServiceGateway;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EmployeeManagement.Tests
{
    [TestClass]
    public class AddEmployeeViewModelTest
    {
        private Mock<IDialogService> _mockDialogService;
        private Mock<IEmployeeServiceGateway> _mockEmployeeServiceGateway;
        private Mock<IEmployeeViewModel> _mockEmployeeViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockDialogService = new Mock<IDialogService>();
            _mockEmployeeServiceGateway = new Mock<IEmployeeServiceGateway>();
            _mockEmployeeViewModel = new Mock<IEmployeeViewModel>();
        }


        [TestMethod]
        public void AddNewEmployee_WithCorrectInputValues_ShouldAddANewEmployeeInTheSystem()
        {
            //Arrange

            Employee inputEmployee = new Employee()
                {Name = "Test", Email = "abc@test.in", Gender = "Male", Status = "Active"};
            Employee returnedEmployee = inputEmployee;
            returnedEmployee.Id = 5;

            _mockEmployeeServiceGateway.Setup(x => x.AddEmployee(inputEmployee)).ReturnsAsync(returnedEmployee).Verifiable();
            _mockEmployeeViewModel.Setup(x=>x.ResetEmployees()).Verifiable();
            _mockDialogService.Setup(x=>x.ShowSuccessMessageBox(It.IsAny<string>(),It.IsAny<string>())).Verifiable();

            var addEmployeeViewModel = new AddEmployeeViewModel(_mockEmployeeViewModel.Object,_mockEmployeeServiceGateway.Object, _mockDialogService.Object);
            addEmployeeViewModel.NewEmployee = inputEmployee;

            //Act

            var addedEmployee=addEmployeeViewModel.AddNewEmployee().Result;

            //Assert
            Assert.IsTrue(addedEmployee.Id==returnedEmployee.Id);
            _mockEmployeeServiceGateway.Verify(x => x.AddEmployee(inputEmployee), Times.Exactly(1));
            _mockDialogService.Verify(x=>x.ShowSuccessMessageBox(It.IsAny<string>(), It.IsAny<string>()),Times.Once);
            _mockDialogService.Verify(x => x.ShowErrorMessageBox(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        [TestMethod]
        public void AddNewEmployee_WithInvalidValues_ShouldShowErrorMessageToUser()
        {
            //Arrange

            Employee inputEmployee = It.IsAny<Employee>();

            _mockDialogService.Setup(x => x.ShowErrorMessageBox(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            var addEmployeeViewModel = new AddEmployeeViewModel(_mockEmployeeViewModel.Object, _mockEmployeeServiceGateway.Object, _mockDialogService.Object);

            //Act

            var addedEmployee = addEmployeeViewModel.AddNewEmployee().Result;

            //Assert
            _mockEmployeeServiceGateway.Verify(x => x.AddEmployee(inputEmployee), Times.Exactly(0));
            _mockDialogService.Verify(x => x.ShowSuccessMessageBox(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mockDialogService.Verify(x => x.ShowErrorMessageBox(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

    }
}
