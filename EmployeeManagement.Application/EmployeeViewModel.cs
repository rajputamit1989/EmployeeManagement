using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeManagement.BusinessModel;
using EmployeeManagement.ServiceGateway;

namespace EmployeeManagement.Application
{
    public class EmployeeViewModel : ViewModelBase, IEmployeeViewModel
    {
        private readonly IEmployeeServiceGateway _employeeServiceGateway;
        private readonly IDialogService _dialogService;
        public EmployeeViewModel(IEmployeeServiceGateway employeeServiceGateway, IDialogService dialogService)
        {
            _employeeServiceGateway = employeeServiceGateway;
            _dialogService = dialogService;
            _currentPage = 1;
            GetAndSetEmployees().GetAwaiter().GetResult();
        }
        private ObservableCollection<Employee> _employees;
        private Employee _selectedEmployee;
        private int _currentPage;

        private ICommand _getEmployeesCommand;
        private ICommand _deleteEmployeeCommand;
        private ICommand _resetCommand;
        private ICommand _nextCommand;
        private ICommand _previousCommand;

        private string _searchCriteria;

        public ICommand NextCommand
        {
            get
            {
                return _nextCommand ??= new RelayCommand(
                    async param => await NextCommandHandler(),
                    null);
            }
        }

        private async Task NextCommandHandler()
        {
            CurrentPage = CurrentPage + 1;
            EmployeeSearchCriteria = null;
            await GetAndSetEmployees();
        }

        public ICommand PreviousCommand
        {
            get
            {
                return _previousCommand ??= new RelayCommand(
                    async param => await PreviousCommandHandler(),
                    null);
            }
        }
        private async Task PreviousCommandHandler()
        {
            if (CurrentPage != 1)
                CurrentPage = CurrentPage - 1;
            EmployeeSearchCriteria = null;
            await GetAndSetEmployees();
        }
        public ICommand GetEmployeesCommand
        {
            get
            {
                return _getEmployeesCommand ??= new RelayCommand(
                    async param => await GetAndSetEmployees(),
                    null);
            }
        }

        public ICommand DeleteEmployeeCommand
        {
            get
            {
                return _deleteEmployeeCommand ??= new RelayCommand(
                    async param => await DeleteEmployee((Employee)param),
                    null);
            }
        }

        public ICommand ResetCommand
        {
            get
            {
                return _resetCommand ??= new RelayCommand(async param => await ResetEmployees(),
                    null);
            }
        }

        public async Task ResetEmployees()
        {
            EmployeeSearchCriteria = string.Empty;
            CurrentPage = 1;
            SelectedEmployee = null;
            await GetAndSetEmployees();
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                NotifyPropertyChanged("CurrentPage");
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                NotifyPropertyChanged("Employees");
            }
        }
        public string EmployeeSearchCriteria
        {
            get => _searchCriteria;
            set
            {
                _searchCriteria = value;
                NotifyPropertyChanged("EmployeeSearchCriteria");
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                NotifyPropertyChanged("SelectedEmployee");
            }
        }

        public async Task DeleteEmployee(Employee employee)
        {
            if (employee.Id > 0)
            {
                await _employeeServiceGateway.DeleteEmployee(employee.Id);
                Employees.Remove(employee);
            }
        }

        /// <summary>
        /// This fetches (and sets Employees property) from the system under different conditions like getting all employees, or by ID or by Name.
        /// </summary>
        /// <returns></returns>
        public async Task GetAndSetEmployees()
        {
            try
            {
                if (!string.IsNullOrEmpty(_searchCriteria))
                {
                    //First check if user is searching by employee id

                    bool isValidInputId = Int32.TryParse(_searchCriteria, out int employeeSearchCriteriaId);
                    if (isValidInputId)
                    {
                        var employeeFound = await _employeeServiceGateway.GetEmployeeById(employeeSearchCriteriaId);
                        Employees = employeeFound.Id != 0 ? new ObservableCollection<Employee>() { employeeFound } : null;
                        CurrentPage = 1;
                        return;
                    }

                    //Next case :It is a search by employee name
                    string name = _searchCriteria;
                    var employeesFound = await _employeeServiceGateway.GetEmployeesByName(name);
                    Employees = employeesFound.Any() ? new ObservableCollection<Employee>(employeesFound) : null;
                    CurrentPage = 1;
                    return;

                }
                //Return all records if there is no search criteria
                Employees = new ObservableCollection<Employee>(await _employeeServiceGateway.GetEmployees(_currentPage).ConfigureAwait(false));

            }
            catch (Exception e)
            {
                _dialogService.ShowErrorMessageBox("Error encountered while fetching data from service, Error Message : "+e.Message,"Error");
            }
        }
    }
}
