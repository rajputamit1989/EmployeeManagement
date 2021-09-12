using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EmployeeManagement.BusinessModel;

namespace EmployeeManagement.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_AddEmployee(object sender, RoutedEventArgs e)
        {
            var employeeDialog = new AddEmployeeDialog();
           employeeDialog.ShowDialog();
        }

        private void TextValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = (sender as Button)?.DataContext as Employee;
            var updateEmployeeDialog = new UpdateEmployeeDialog(selectedEmployee);
            updateEmployeeDialog.ShowDialog();
        }
    }
}
