using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using EmployeeManagement.BusinessModel;
using EmployeeManagement.ServiceGateway;

namespace EmployeeManagement.Application
{
    public class UpdateEmployeeViewModel : ViewModelBase, IUpdateEmployeeViewModel, ICloseable
    {
        private readonly IEmployeeViewModel _employeeViewModel;
        private readonly IEmployeeServiceGateway _employeeServiceGateway;
        private Employee _selectedEmployee;
        private readonly IDialogService _dialogService;

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                NotifyPropertyChanged("SelectedEmployee");
            }
        }

        private ICommand _updateEmployeeCommand;
        public ICommand UpdateEmployeeCommand
        {
            get
            {
                return _updateEmployeeCommand ?? (_updateEmployeeCommand = new RelayCommand(
                    async param => await UpdateEmployee(),
                    null));
            }
        }

        public UpdateEmployeeViewModel(IEmployeeViewModel employeeViewModel, IEmployeeServiceGateway employeeServiceGateway,IDialogService dialogService)
        {
            _employeeViewModel = employeeViewModel;
            _employeeServiceGateway = employeeServiceGateway;
            _dialogService = dialogService;
        }
        public async Task<Employee> UpdateEmployee()
        {
            if (!IsValidEmployee())
            {
                _dialogService.ShowErrorMessageBox("All fields are mandatory", "Missing data");
                return null;
            }
            var employeeUpdated = await _employeeServiceGateway.UpdateEmployee(SelectedEmployee);

            //Raise event when employee is added so that the corresponding view can be closed.
            RequestClose?.Invoke(this, EventArgs.Empty);
            _dialogService.ShowSuccessMessageBox("Employee updated successfully", "Success");
            return employeeUpdated;
        }
        private bool IsValidEmployee()
        {
            if (SelectedEmployee == null || SelectedEmployee.Id<1 || string.IsNullOrWhiteSpace(SelectedEmployee.Email) ||
                string.IsNullOrWhiteSpace(SelectedEmployee.Gender) || string.IsNullOrWhiteSpace(SelectedEmployee.Status))
                return false;
            return true;
        }
        public event EventHandler<EventArgs> RequestClose;
    }
}
