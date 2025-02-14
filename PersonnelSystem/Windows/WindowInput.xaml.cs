using PersonnelSystem.Classes;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace PersonnelSystem.Windows
{
    /// <summary>
    /// Окно ввода
    /// </summary>
    public partial class WindowInput : Window, INotifyPropertyChanged
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
        /// Выбранный отдел для переименования
        /// </summary>
        public Department SelectedDepartment
        {
            get => _SelectedDepartment;
            set
            {
                _SelectedDepartment = value;
                OnPropertyChanged(nameof(SelectedDepartment));
            }
        }
        private Department _SelectedDepartment;

        List<Department> departments = [];

        public RaiseCommand RenameDepartmentCommand { get; set; }

        public bool CanExecute = false;


        public WindowInput(List<Department> departments, Department SelectedDepartment)
        {
            InitializeComponent();
            this.departments = departments;
            this.SelectedDepartment = SelectedDepartment;

            RenameDepartmentCommand = new RaiseCommand(RenameDepartmentCommand_Execute, RenameDepartmentCommand_CanExecute);

            DataContext = this;
        }

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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = (sender as TextBox)?.Text;

            if (!string.IsNullOrEmpty(text)) 
                NameDepartment = text;
        }

        /// <summary>
        /// Проверка имени
        /// </summary>
        public bool IsComparisonName()
        {
            if(departments.Any(dep => dep.NameDepartment.ToLower() == NameDepartment.ToLower()) || NameDepartment == string.Empty)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Выполнить команду, переименовать отдел
        /// </summary>
        private void RenameDepartmentCommand_Execute(object parameter)
        {
            CanExecute = true;
            this.Close();
        }

        /// <summary>
        /// Проверка команды, переименовать отдел
        /// </summary>
        private bool RenameDepartmentCommand_CanExecute(object parameter)
        {
            return IsComparisonName();
        }
    }
}
