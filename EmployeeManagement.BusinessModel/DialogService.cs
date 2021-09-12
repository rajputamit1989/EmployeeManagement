using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EmployeeManagement.BusinessModel
{
    public class DialogService :IDialogService
    {
        public void ShowSuccessMessageBox(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK,MessageBoxImage.Information);
        }

        public void ShowErrorMessageBox(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK,MessageBoxImage.Error);
        }
    }
}
