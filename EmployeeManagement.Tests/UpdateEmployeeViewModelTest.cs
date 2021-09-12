using EmployeeManagement.Application;
using EmployeeManagement.BusinessModel;
using EmployeeManagement.ServiceGateway;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EmployeeManagement.Tests
{
    [TestClass]
    public class UpdateEmployeeViewModelTest
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
        public void UpdateEmployee_WithCorrectInputValues_ShouldUpdateExistingEmployeeInTheSystem()
        {
            //Arrange

            Employee oldEmployee = new Employee()
            { Name = "Test", Email = "abc@test.in", Gender = "Male", Status = "Active",Id = 1};
            Employee updatedEmployee = new Employee()
                { Name = "Test Updated", Email = "abc@test.in", Gender = "Male", Status = "Active", Id = 1 };

            _mockEmployeeServiceGateway.Setup(x => x.UpdateEmployee(It.IsAny<Employee>())).ReturnsAsync(updatedEmployee).Verifiable();
            _mockEmployeeViewModel.Setup(x => x.ResetEmployees()).Verifiable();
            _mockDialogService.Setup(x => x.ShowSuccessMessageBox(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            var updateEmployeeViewModel = new UpdateEmployeeViewModel(_mockEmployeeViewModel.Object, _mockEmployeeServiceGateway.Object, _mockDialogService.Object);
            updateEmployeeViewModel.SelectedEmployee = oldEmployee;

            //Act
            updateEmployeeViewModel.SelectedEmployee = updatedEmployee;
            var resultEmployee = updateEmployeeViewModel.UpdateEmployee().Result;

            //Assert
            Assert.IsTrue(resultEmployee.Id == oldEmployee.Id);
            _mockEmployeeServiceGateway.Verify(x => x.UpdateEmployee(updateEmployeeViewModel.SelectedEmployee), Times.Exactly(1));
            _mockDialogService.Verify(x => x.ShowSuccessMessageBox(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockDialogService.Verify(x => x.ShowErrorMessageBox(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        [TestMethod]
        public void UpdateEmployee_WithInvalidValues_ShouldShowErrorMessageToUser()
        {
            //Arrange

            Employee inputEmployee = It.IsAny<Employee>();

            _mockDialogService.Setup(x => x.ShowErrorMessageBox(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            var updateEmployeeViewModel = new UpdateEmployeeViewModel(_mockEmployeeViewModel.Object, _mockEmployeeServiceGateway.Object, _mockDialogService.Object);

            //Act

            var addedEmployee = updateEmployeeViewModel.UpdateEmployee().Result;

            //Assert
            _mockEmployeeServiceGateway.Verify(x => x.UpdateEmployee(inputEmployee), Times.Exactly(0));
            _mockDialogService.Verify(x => x.ShowSuccessMessageBox(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mockDialogService.Verify(x => x.ShowErrorMessageBox(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
