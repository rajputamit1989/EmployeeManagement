using EmployeeManagement.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeManagement.ServiceGateway;

namespace EmployeeManagement.Application
{
    public class AddEmployeeViewModel : ViewModelBase,IAddEmployeeViewModel, ICloseable
    {
        private readonly IEmployeeViewModel _employeeViewModel;
        private readonly IEmployeeServiceGateway _employeeServiceGateway;
        private Employee _newEmployee;
        public event EventHandler<EventArgs> RequestClose;
        private readonly IDialogService _dialogService;
        public Employee NewEmployee
        {
            get => _newEmployee;
            set
            {
                _newEmployee = value;
                NotifyPropertyChanged("NewEmployee");
            }
        }

        private ICommand _addEmployeeCommand;
        public ICommand AddEmployeeCommand
        {
            get
            {
                return _addEmployeeCommand ?? (_addEmployeeCommand = new RelayCommand(
                    async param => await AddNewEmployee(),
                    null));
            }
        }

        public AddEmployeeViewModel(IEmployeeViewModel employeeViewModel, IEmployeeServiceGateway employeeServiceGateway,IDialogService dialogService)
        {
            _employeeViewModel = employeeViewModel;
            _employeeServiceGateway = employeeServiceGateway;
            _dialogService = dialogService;
            NewEmployee = new Employee();
        }

        /// <summary>
        /// Adds a new employee in the system
        /// </summary>
        /// <returns></returns>
        public async Task<Employee> AddNewEmployee()
        {
            try
            {
                if (!IsValidEmployee())
                {
                    _dialogService.ShowErrorMessageBox("All fields are mandatory", "Missing data");
                    return null;
                }
                var employeeAdded = await _employeeServiceGateway.AddEmployee(NewEmployee);
                await _employeeViewModel.ResetEmployees();

                //Raise event when employee is added so that the corresponding view can be closed.
                RequestClose?.Invoke(this, EventArgs.Empty);
                _dialogService.ShowSuccessMessageBox("New employee added with Employee ID : " + employeeAdded.Id, "Success");
                return employeeAdded;
            }
            catch (Exception e)
            {
                    _dialogService.ShowErrorMessageBox("Error encountered while adding employee, Error Message : " + e.Message, "Error");
            }

            return null;
        }

        private bool IsValidEmployee()
        {
            if (NewEmployee == null || string.IsNullOrWhiteSpace(NewEmployee.Email) ||
                string.IsNullOrWhiteSpace(NewEmployee.Gender) || string.IsNullOrWhiteSpace(NewEmployee.Status))
                return false;
            return true;
        }
    }
}
