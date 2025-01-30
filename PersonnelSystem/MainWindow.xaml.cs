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
using System.Windows.Data;
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
        public RaiseCommand DismissEmployeeCommand {  get; set; }
        public RaiseCommand ChangeEmployeeCommand { get; set; }


        private string _SurnameEmployee = string.Empty;
        public string SurnameEmployee 
        { 
            get => _SurnameEmployee;
            set
            {
                _SurnameEmployee = value;
                OnPropertyChanged(nameof(SurnameEmployee));
            }
        }

        private string _NameEmployee = string.Empty;
        public string NameEmployee
        {
            get => _NameEmployee;
            set
            {
                _NameEmployee = value;
                OnPropertyChanged(nameof(NameEmployee));
            }
        }

        private string _PatronymicEmployee = string.Empty;
        public string PatronymicEmployee
        {
            get => _PatronymicEmployee;
            set
            {
                _PatronymicEmployee = value;
                OnPropertyChanged(nameof(PatronymicEmployee));
            }
        }

        private Department? _DepartmentEmployee;
        public Department? DepartmentEmployee
        {
            get => _DepartmentEmployee;
            set
            {
                _DepartmentEmployee = value;
                OnPropertyChanged(nameof(DepartmentEmployee));
            }
        }

        /// <summary>
        /// Дата приема на работу нового сотрудника
        /// </summary>
        public DateTime? DateAdmissionEmployee
        {
            get => _DateAdmissionEmployee;
            set
            {
                _DateAdmissionEmployee = value;
                OnPropertyChanged(nameof(DateAdmissionEmployee));
            }
        }
        public DateTime? _DateAdmissionEmployee;

        /// <summary>
        /// Дата увольнения сотрудника
        /// </summary>
        public DateTime? DateDismissalEmployee 
        {
            get => _DateDismissalEmployee;
            set
            {
                _DateDismissalEmployee = value;
                OnPropertyChanged(nameof(DateDismissalEmployee));
            }
        }
        private DateTime? _DateDismissalEmployee;


        /// <summary>
        /// Выбранный сотрудник
        /// </summary>
        public Employee? CurrentEmployee
        {
            get => _CurrentEmployee;
            set
            {
                _CurrentEmployee = value;
                OnPropertyChanged(nameof(CurrentEmployee));
            }
        }
        private Employee? _CurrentEmployee;

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
            NewEmployeeCommand = new RaiseCommand(NewEmployeeCommand_Execute, NewEmployeeCommand_CanExecute);
            DismissEmployeeCommand = new RaiseCommand(DismissEmployeeCommand_Execute, DismissEmployeeCommand_CanExecute);
            DeleteEmployeeCommand = new RaiseCommand(DeleteEmployeeCommand_Execute, DeleteEmployeeCommand_CanExecute);
            ChangeEmployeeCommand = new RaiseCommand(ChangeEmployeeCommand_Execute, ChangeEmployeeCommand_CanExecute);
        }

        /// <summary>
        /// Проверка, перед Добавлением нового сотрудника
        /// </summary>
        private bool AddEmployeeCommand_CanExecute(object? parameter)
        {
            return CurrentEmployee == null && CheckField();
        }

        /// <summary>
        /// Добавление нового сотрудника
        /// </summary>
        private void AddEmployeeCommand_Execute(object? parameter)
        {
            var countEmployee = ListEmployees.Count;

            Employee employee = new Employee(TagClass: EmployeeAsCsv.Tag,
                                            ID_employee: countEmployee,
                                            SurnameEmployee: SurnameEmployee,
                                            NameEmployee: NameEmployee,
                                            PatronymicEmployee: PatronymicEmployee,
                                            DepartmentEmployee: DepartmentEmployee,
                                            DateAdmissionEmployee: DateAdmissionEmployee ?? DateTime.Now.Date,
                                            DateDismissalEmployee: DateDismissalEmployee);

            if (ListEmployees.Any(x => Employee.CheckingFieldEquality(x, employee)))
            {
                MessageBox.Show("Данный сотрудник уже есть в списке", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            employee.ID_employee ++;

            ListEmployees.Add(employee);
            ListDepartments.First(x => x == employee.DepartmentEmployee).ListEmployees.Add(employee);
        }

        /// <summary>
        /// Проверка полей на заполнение
        /// </summary>
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
            ClearTextInControls();
            CurrentEmployee = null;
            DataGridEmployee.SelectedItem = null;
        }

        private bool NewEmployeeCommand_CanExecute(object? parameter)
        {
            return CheckField();
        }

        /// <summary>
        /// Уволить сотрудника
        /// </summary>
        private void DismissEmployeeCommand_Execute(object? parameter)
        {
            ListEmployees.First(x => x == CurrentEmployee).DateDismissalEmployee = DateDismissalEmployee;

            var text = string.Format($"Сотрудник: {SurnameEmployee} {NameEmployee} {PatronymicEmployee} \r\n" +
                                    $"Принят: {DateAdmissionEmployee.Value.ToShortDateString()}\r\n" +
                                    $"Уволен: {DateDismissalEmployee.Value.ToShortDateString()}");
            
            MessageBox.Show($"{text}", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);

            //DataGridEmployee.UpdateLayout();
            CollectionViewSource.GetDefaultView(ListEmployees.First(x => x == CurrentEmployee));
            //CollectionViewSource.GetDefaultView(ListDepartments);
        }

        /// <summary>
        /// Проверка для команды, уволить сотрудника
        /// </summary>
        private bool DismissEmployeeCommand_CanExecute(object? parameter)
        {
            if (CurrentEmployee != null && CurrentEmployee.DateDismissalEmployee == null && DateDismissalEmployee != null)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Выполнить команду, Удалить сотрудника
        /// </summary>
        private void DeleteEmployeeCommand_Execute(object? parameter)
        {
            ListEmployees.Remove(CurrentEmployee!);
            ListDepartments.First(x => x.ListEmployees.Remove(CurrentEmployee!));
            CurrentEmployee = null;
        }

        /// <summary>
        /// Проверка для команды, удалить сотрудника
        /// </summary>
        private bool DeleteEmployeeCommand_CanExecute(object? parameter)
        {
            return CurrentEmployee != null;
        }

        private bool ChangeEmployeeCommand_CanExecute(object? parameter)
        {
            return CurrentEmployee != null;
        }

        private void ChangeEmployeeCommand_Execute(object? parameter)
        {
            TextInsertCurentEmployee(CurrentEmployee);
        }

        /// <summary>
        /// Заполнение данными экземпляр класса сотрудники
        /// </summary>
        public void TextInsertCurentEmployee(Employee? employee)
        {
            if (employee == null)
                return;

            employee.SurnameEmployee = SurnameEmployee;
            employee.NameEmployee = NameEmployee;
            employee.PatronymicEmployee = PatronymicEmployee;

            employee.DepartmentEmployee = DepartmentEmployee;

            employee.DateAdmissionEmployee = DateAdmissionEmployee ?? employee.DateAdmissionEmployee;
            employee.DateDismissalEmployee = DateDismissalEmployee;
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

                // переделать!!!!!!!!! Подумать

                foreach (var employee in department.ListEmployees)
                {
                    employee.DepartmentEmployee = department;
                }
            }
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

            CurrentEmployee = null;
            ListEmployees.Clear();
            ClearTextInControls();

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

            CurrentEmployee = null;
        }

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

            CurrentEmployee = employee;
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

            this.DateAdmissionEmployee = employee.DateAdmissionEmployee;
            this.DateDismissalEmployee = employee.DateDismissalEmployee;
        }

        /// <summary>
        /// Очистка контролов
        /// </summary>
        public void ClearTextInControls()
        {
            TextBoxSurnameEmployee.Text = string.Empty;
            TextBoxNameEmployee.Text = string.Empty;
            TextBoxPatronymicEmployee.Text = string.Empty;
            ComboBoxListDepartment.SelectedItem = null;
            DatePickerAdmissionEmployee.Text = string.Empty;
            DatePickerDismissalEmployee.Text = string.Empty;
        }

        #endregion

    } //END
}