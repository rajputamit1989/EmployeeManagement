using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.BusinessModel
{
    public interface IDialogService
    {
        void ShowErrorMessageBox(string text, string caption);
        void ShowSuccessMessageBox(string text, string caption);

    }
}
