using PersonnelSystem.Classes;
using System.ComponentModel;
using System.Windows;

namespace PersonnelSystem.Windows
{
    /// <summary>
    /// Окно добавления нового отдела
    /// </summary>
    public partial class WindowAddNewDepartment : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Название отдела
        /// </summary>
        public string NameDepartment 
        {
            get => _NameDepartment; 
            set
            {
                _NameDepartment = value;
                OnPropertyChanged(nameof(NameDepartment));
            }
        } 
        private string _NameDepartment = string.Empty;

        /// <summary>
        /// Родительский отдел
        /// </summary>
        public List<Department> ListParentsDepartments 
        {
            get => _ListParentsDepartments;
            set
            {
                _ListParentsDepartments = value;
                OnPropertyChanged(nameof(ListParentsDepartments));
            }
        }
        private List<Department> _ListParentsDepartments = [];

        public Department SelectedDepartment { get; set; }

        public RaiseCommand AddNewDepartmentCommand { get; set; }

        #region ОБНОВЛЕНИЕ UI

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public WindowAddNewDepartment(List<Department> ListParentsDepartments)
        {
            InitializeComponent();

            this.ListParentsDepartments = ListParentsDepartments;
            this.SelectedDepartment = ListParentsDepartments[0];

            AddNewDepartmentCommand = new RaiseCommand(AddNewDepartmentCommand_Execute, AddNewDepartmentCommand_CanExecute);
        }

        /// <summary>
        /// Добавить новый отдел
        /// </summary>
        private void AddNewDepartmentCommand_Execute(object parameter)
        {
            this.Close();
        }
        
        /// <summary>
        /// Проверка команды, Добавить новый отдел
        /// </summary>
        private bool AddNewDepartmentCommand_CanExecute(object parameter)
        {
            if(string.IsNullOrEmpty(NameDepartment) || ListParentsDepartments == null) 
                return false;
            else 
                return true;
        }
    }
}
