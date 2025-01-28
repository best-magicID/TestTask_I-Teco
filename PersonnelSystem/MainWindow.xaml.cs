using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Win32;
using PersonnelSystem.Classes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PersonnelSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region ПОЛЯ И СВОЙСТВА

        /// <summary>
        /// Список отделов
        /// </summary>
        public ObservableCollection<Department> ListDepartments { get; set; } = new ObservableCollection<Department>();

        /// <summary>
        /// Список отделов в CSV
        /// </summary>
        public List<DepartmentAsCsv> ListDepartmentsAsCsv { get; set; } = new List<DepartmentAsCsv>();

        /// <summary>
        /// Список сотрудников
        /// </summary>
        public ObservableCollection<Employee> ListEmployees { get; set; } = new ObservableCollection<Employee>();

        /// <summary>
        /// Список сотрудников в CSV
        /// </summary>
        public List<EmployeeAsCsv> ListEmployeesAsCsv { get; set; } = new List<EmployeeAsCsv>();

        public RaiseCommand ReadCsvFileCommand { get; set; }
        public RaiseCommand AddEmployeeCommand {  get; set; }
        public RaiseCommand NewEmployeeCommand {  get; set; }
        public RaiseCommand DeleteEmployeeCommand { get; set; }


        private string _SurnameEmployee = string.Empty;
        public string SurnameEmployee 
        { 
            get => _SurnameEmployee;
            set
            {
                if (_SurnameEmployee != value)
                {
                    _SurnameEmployee = value;
                    OnPropertyChanged(nameof(SurnameEmployee));
                }
            }
        }

        private string _NameEmployee = string.Empty;
        public string NameEmployee
        {
            get => _NameEmployee;
            set
            {
                if (_NameEmployee != value)
                {
                    _NameEmployee = value;
                    OnPropertyChanged(nameof(NameEmployee));
                }
            }
        }

        private string _PatronymicEmployee = string.Empty;
        public string PatronymicEmployee
        {
            get => _PatronymicEmployee;
            set
            {
                if (_PatronymicEmployee != value)
                {
                    _PatronymicEmployee = value;
                    OnPropertyChanged(nameof(PatronymicEmployee));
                }
            }
        }

        private Department _DepartmentEmployee;
        public Department DepartmentEmployee
        {
            get => _DepartmentEmployee;
            set
            {
                if (_DepartmentEmployee != value)
                {
                    _DepartmentEmployee = value;
                    OnPropertyChanged(nameof(DepartmentEmployee));
                }
            }
        }

        /// <summary>
        /// Выбранный сотрудник
        /// </summary>
        public Employee CurrentEmployee
        {
            get => _CurrentEmployee;
            set
            {
                _CurrentEmployee = value;
                OnPropertyChanged(nameof(CurrentEmployee));
            }
        }
        private Employee _CurrentEmployee;

        #endregion

        #region КОНСТРУКТОР

        public MainWindow()
        {
            InitializeComponent();

            LoadCommands();

            ReadCsvFileCommand_Execute(null);
        }

        #endregion

        #region МЕТОДЫ И КОМАНДЫ

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

        /// <summary>
        /// Загрузка команд
        /// </summary>
        public void LoadCommands()
        {
            ReadCsvFileCommand = new RaiseCommand(ReadCsvFileCommand_Execute);
            AddEmployeeCommand = new RaiseCommand(AddEmployeeCommand_Execute, AddEmployeeCommand_CanExecute);
            NewEmployeeCommand = new RaiseCommand(NewEmployeeCommand_Execute);
        }

        private bool AddEmployeeCommand_CanExecute(object? parameter)
        {
            return CheckField();
        }

        private void AddEmployeeCommand_Execute(object? parameter)
        {
            var countEmployee = ListEmployees.Count + 1;

            Employee employee = new Employee(TagClass: EmployeeAsCsv.Tag,
                                            ID_employee: countEmployee,
                                            SurnameEmployee: SurnameEmployee,
                                            NameEmployee: NameEmployee,
                                            PatronymicEmployee: PatronymicEmployee,
                                            DepartmentEmployee: DepartmentEmployee,
                                            DateAdmissionEmployee: DateTime.Now.Date,
                                            DateDismissalEmployee: null);

            ListEmployees.Add(employee);
            ListDepartments.First(x => x == employee.DepartmentEmployee).ListEmployees.Add(employee);
        }

        public bool CheckField()
        {
            if(string.IsNullOrEmpty(TextBoxSurnameEmployee.Text))
                return false;

            if (string.IsNullOrEmpty(TextBoxNameEmployee.Text))
                return false;

            if (string.IsNullOrEmpty(TextBoxPatronymicEmployee.Text))
                return false;

            if (string.IsNullOrEmpty(ComboBoxListDepartment.Text))
                return false;

            if (string.IsNullOrEmpty(DatePickerAdmissionEmployee.Text))
                return false;

            return true;
        }

        public void NewEmployeeCommand_Execute(object? parameter)
        {
            ClearTextInControl();
        }

        /// <summary>
        /// Чтение CSV файла
        /// </summary>
        private void ReadCsvFileCommand_Execute(object? parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName == string.Empty)
                return;

            using var streamReader = new StreamReader(openFileDialog.FileName, Encoding.UTF8);
           
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = ";"
            };

            ClearAllList();

            using (CsvReader? csvReader = new CsvReader(streamReader, csvConfig))
            {
                if (csvReader == null) 
                    return;

                while (csvReader!.Read())
                {
                    string? stringReader = csvReader?.GetField(0);

                    switch (stringReader)
                    {
                        case DepartmentAsCsv.Tag:
                            var departmentAsCsv = csvReader!.GetRecord<DepartmentAsCsv>();

                            ListDepartmentsAsCsv.Add(new DepartmentAsCsv(
                                TagClass: departmentAsCsv.TagClass,
                                Id_department: departmentAsCsv.Id_department,
                                NameDepartment: departmentAsCsv.NameDepartment,
                                DepartmentsString: departmentAsCsv.DepartmentsString,
                                TypeDepartment: departmentAsCsv.TypeDepartment,
                                ListEmployees: departmentAsCsv.ListEmployees));
                            break;

                        case EmployeeAsCsv.Tag:
                            var employeesAsCsv = csvReader!.GetRecord<EmployeeAsCsv>();

                            ListEmployeesAsCsv.Add(new EmployeeAsCsv(
                                TagClass: employeesAsCsv.TagClass,
                                ID_employee: employeesAsCsv.ID_employee,
                                SurnameEmployee: employeesAsCsv.SurnameEmployee, 
                                NameEmployee: employeesAsCsv.NameEmployee,
                                PatronymicEmployee: employeesAsCsv.PatronymicEmployee,
                                DepartmentEmployee: employeesAsCsv.DepartmentEmployee,
                                DateAdmissionEmployee: employeesAsCsv.DateAdmissionEmployee,
                                DateDismissalEmployee: employeesAsCsv.DateDismissalEmployee));
                            break;

                        case null:
                            break;
                    }
                }
            }

            ConvertListsCsvInLists();
        }

        /// <summary>
        /// Конвертируем Листы CSV в Листы для отображения
        /// </summary>
        public void ConvertListsCsvInLists()
        {
            foreach (var departmentsAsCsv in ListDepartmentsAsCsv)
            {
                ListDepartments.Add(new Department(departmentsAsCsv));
            }

            foreach (var employeesAsCsv in ListEmployeesAsCsv)
            {
                ListEmployees.Add(new Employee(employeesAsCsv));
            }

            // Добавление в отделы сотрудников
            foreach (var department in ListDepartments)
            {
                // Лист сотрудниками для конкретного отдела 
                department.ListEmployees = ListEmployees.Where(x => int.Parse(x.DepartmentEmployeeString) == department.Id_department).ToList<Employee>();

                foreach (var employee in department.ListEmployees)
                {
                    employee.DepartmentEmployee = department;
                }
            }

            TreeViewDepartments.ItemsSource = ListDepartments;
            ComboBoxListDepartment.ItemsSource = ListDepartments;
            //ListViewEmployees.ItemsSource = ListEmployees;
            DataGridEmployee.ItemsSource = ListEmployees;
        }

        /// <summary>
        /// Отображение списка сотрудников для конкретного отдела
        /// </summary>
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeViewSelectedItem = (sender as TreeView)?.SelectedItem;
            var department = treeViewSelectedItem as Department;

            if (department is null)
                return;

            ListEmployees.Clear();
            ClearTextInControl();

            foreach (var item in department.ListEmployees)
            {
                ListEmployees.Add(item);
            }
        }

        /// <summary>
        /// Чистка всех списков
        /// </summary>
        public void ClearAllList()
        {
            ListDepartments.Clear();
            ListDepartmentsAsCsv.Clear();
            ListEmployees.Clear();
            ListEmployeesAsCsv.Clear();
        }

        #endregion

        /// <summary>
        /// Выбор сотрудника из списка
        /// </summary>
        private void DataGridEmployee_CurrentCellChanged(object sender, EventArgs e)
        {
            var dataGrid = sender as DataGrid;
            var employee = dataGrid?.CurrentItem as Employee;

            if (employee == null || employee == CurrentEmployee)
                return;

            FillFields(employee);

            //CurrentEmployee = employee;
        }

        /// <summary>
        /// Заполнение полей
        /// </summary>
        public void FillFields(Employee employee)
        {
            this.SurnameEmployee = employee.SurnameEmployee;
            this.NameEmployee = employee.NameEmployee;
            this.PatronymicEmployee = employee.PatronymicEmployee;

            this.DepartmentEmployee = employee.DepartmentEmployee;
        }

        /// <summary>
        /// Очистка контролов
        /// </summary>
        public void ClearTextInControl()
        {
            TextBoxSurnameEmployee.Text = string.Empty;
            TextBoxNameEmployee.Text = string.Empty;
            TextBoxPatronymicEmployee.Text = string.Empty;
            ComboBoxListDepartment.SelectedItem = null;
            DatePickerAdmissionEmployee.Text = string.Empty;
            DatePickerDismissalEmployee.Text = string.Empty;
        }

    } //END
}