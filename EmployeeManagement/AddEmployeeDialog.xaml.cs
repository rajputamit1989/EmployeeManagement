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
using System.Windows.Shapes;
using EmployeeManagement.Application;
using EmployeeManagement.Infrastructure;
using Unity;

namespace EmployeeManagement.Presentation
{
    /// <summary>
    /// Interaction logic for AddEmployeeDialog.xaml
    /// </summary>
    public partial class AddEmployeeDialog : Window
    {
        public AddEmployeeDialog()
        {
            InitializeComponent();
            DataContext = UnityContainerBootstrapper.Container.Resolve<IAddEmployeeViewModel>() as AddEmployeeViewModel;

            Loaded += (s, e) =>
            {
                if (DataContext is ICloseable)
                {
                    (DataContext as ICloseable).RequestClose += (_, _) => this.Close();
                }
            };
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtEmail_LostFocus(object sender, KeyboardFocusChangedEventArgs keyboardFocusChangedEventArgs)
        {
            if (IsValidEmail(TxtEmail.Text?.Trim()) == false)
            {
                keyboardFocusChangedEventArgs.Handled = true;
                MessageBox.Show("E-Mail expected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                TxtEmail.Focus();
            }
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void TxtName_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
