using System.ComponentModel;

namespace EmployeeManagement.BusinessModel
{
    /// <summary>
    /// Employee business entity with relevant fields
    /// </summary>
    public class Employee : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _email;
        private string _gender;
        private string _status;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public string Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;
                NotifyPropertyChanged("Gender");
            }
        }
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                NotifyPropertyChanged("Email");
            }
        }
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
