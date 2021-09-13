using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EmployeeManagement.BusinessModel;
using Button = System.Windows.Controls.Button;
using Window = System.Windows.Window;

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

        private void BtnExport_OnClick(object sender, RoutedEventArgs e)
        {
            DgEmployees.SelectionMode = DataGridSelectionMode.Extended;
            DgEmployees.SelectAllCells();
            DgEmployees.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, DgEmployees);
            String result = (string)Clipboard.GetData(DataFormats.Text);
            DgEmployees.UnselectAllCells();
            System.IO.StreamWriter file1 = new System.IO.StreamWriter(@"Employees.xls");
            file1.WriteLine(result.Replace(',', ' '));
            
            file1.Close();
            DgEmployees.SelectionMode = DataGridSelectionMode.Single;
            MessageBox.Show(" Employees information exported to Employees.xls in your application directory", "Export Success",MessageBoxButton.OK,MessageBoxImage.Information);
        }
    }
}
