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

        public EmployeeViewModel(IEmployeeServiceGateway employeeServiceGateway)
        {
            _employeeServiceGateway = employeeServiceGateway;
            GetAndSetEmployees().ConfigureAwait(false);
            SetEmployeeIds();
        }

        private void SetEmployeeIds()
        {
            var employeeIds = Employees.Select(employee => employee.Id).ToList();
            EmployeeIds = employeeIds.Any() ? new ObservableCollection<int>(Employees.Select(employee => employee.Id).ToList()) : new ObservableCollection<int>();
        }

        private ObservableCollection<Employee> _employees;
        private ObservableCollection<int> _employeeIds;

        private Employee _selectedEmployee;

        private ICommand _getEmployeesCommand;
        private ICommand _deleteEmployeeCommand;
        private ICommand _resetCommand;
        private string _searchCriteriaName;
        private int? _employeeSearchCriteriaId;

        public ICommand GetEmployeesCommand
        {
            get
            {
                return _getEmployeesCommand ?? (_getEmployeesCommand = new RelayCommand(
                    async param => await GetAndSetEmployees((int?) param),
                    null));
            }
        }

        public ICommand DeleteEmployeeCommand
        {
            get
            {
                return _deleteEmployeeCommand ?? (_deleteEmployeeCommand = new RelayCommand(
                    async param => await DeleteEmployee((Employee) param),
                    null));
            }
        }

        public ICommand ResetCommand
        {
            get
            {
                return _resetCommand ?? (_resetCommand = new RelayCommand(async param => await Reset(),
                    null));
            }
        }

        private async Task Reset()
        {
            EmployeeSearchCriteriaName = string.Empty;
            EmployeeSearchCriteriaId = null;
            //EmployeeSearchCriteria.Id = null;
            //EmployeeSearchCriteria.Name = string.Empty;
            //EmployeeSearchCriteria = null; 
            SelectedEmployee = null;
            await GetAndSetEmployees();
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

        public ObservableCollection<int> EmployeeIds
        {
            get => _employeeIds;
            set
            {
                _employeeIds = value;
                NotifyPropertyChanged("EmployeeIds");
            }
        }

        //public EmployeeSearchCriteria EmployeeSearchCriteria
        //{
        //    get => _searchCriteria;
        //    set
        //    {
        //        _searchCriteria = value;
        //        NotifyPropertyChanged("EmployeeSearchCriteria");
        //    }
        //}

        public string EmployeeSearchCriteriaName
        {
            get => _searchCriteriaName;
            set
            {
                _searchCriteriaName = value;
                NotifyPropertyChanged("EmployeeSearchCriteriaName");
            }
        }

        public int? EmployeeSearchCriteriaId
        {
            get => _employeeSearchCriteriaId;
            set
            {
                _employeeSearchCriteriaId = value;
                NotifyPropertyChanged("EmployeeSearchCriteriaId");
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
            if (employee.Id> 0)
            {
                await _employeeServiceGateway.DeleteEmployee(employee.Id);
                Employees.Remove(employee);
                SetEmployeeIds();
            }
        }

        public async Task GetAndSetEmployees(int? page = null)
        {
            //Search by ID if ID is entered by user
            if (_employeeSearchCriteriaId.HasValue)
            {
                bool isValidInputId = Int32.TryParse(Convert.ToString(_employeeSearchCriteriaId), out int employeeSearchCriteriaId);
                if (!isValidInputId)
                    return;
                Employees = new ObservableCollection<Employee>()
                {
                    await _employeeServiceGateway.GetEmployeeById(employeeSearchCriteriaId)
                };
                return;
            }

            //Search by Name if Name is entered by user
            string name = _searchCriteriaName;
            if (!string.IsNullOrEmpty(name))
            {
                Employees = new ObservableCollection<Employee>(await _employeeServiceGateway.GetEmployeesByName(name));
                return;
            }
            //Return all records if there is no search criteria
            Employees = new ObservableCollection<Employee>(await _employeeServiceGateway.GetEmployees(page));

        }
    }
}
