using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Win32;
using PersonnelSystem.Classes;
using PersonnelSystem.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PersonnelSystem
{
    /// <summary>
    /// Кадровая система
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region ПОЛЯ И СВОЙСТВА

        public const string NameFileCsv = "ListDepartment.csv";
        public string Path = Directory.GetCurrentDirectory() + "\\Файлы CSV\\" + NameFileCsv;

        public const string NameFileLogs = "Logs.txt";
        public string PathForLogs = Directory.GetCurrentDirectory() + "\\Файлы CSV\\" + NameFileLogs;

        public Department? MainDepartment { get; set; }

        /// <summary>
        /// Список всех отделов
        /// </summary>
        public List<Department> ListAllDepartments { get; set; } = new List<Department>();

        /// <summary>
        /// Главный отдел
        /// </summary>
        public ObservableCollection<Department> RootDepartment { get; set; } = new ObservableCollection<Department>();

        /// <summary>
        /// Список отделов в CSV
        /// </summary>
        public List<DepartmentAsCsv> ListAllDepartmentsAsCsv { get; set; } = new List<DepartmentAsCsv>();

        /// <summary>
        /// Список всех сотрудников
        /// </summary>
        public List<Employee> ListAllEmployees { get; set; } = new List<Employee>();

        /// <summary>
        /// Список сотрудников
        /// </summary>
        public ObservableCollection<Employee> ListEmployeesSelectedDepartment { get; set; } = new ObservableCollection<Employee>();

        /// <summary>
        /// Список сотрудников в CSV
        /// </summary>
        public List<EmployeeAsCsv> ListAllEmployeesAsCsv { get; set; } = new List<EmployeeAsCsv>();

        /// <summary>
        /// Фамилия сотрудника
        /// </summary>
        public string SurnameEmployee
        {
            get => _SurnameEmployee;
            set
            {
                _SurnameEmployee = value;
                OnPropertyChanged(nameof(SurnameEmployee));
            }
        }
        private string _SurnameEmployee = string.Empty;

        /// <summary>
        /// Имя сотрудника
        /// </summary>
        public string NameEmployee
        {
            get => _NameEmployee;
            set
            {
                _NameEmployee = value;
                OnPropertyChanged(nameof(NameEmployee));
            }
        }
        private string _NameEmployee = string.Empty;

        /// <summary>
        /// Отчество сотрудника
        /// </summary>
        public string PatronymicEmployee
        {
            get => _PatronymicEmployee;
            set
            {
                _PatronymicEmployee = value;
                OnPropertyChanged(nameof(PatronymicEmployee));
            }
        }
        private string _PatronymicEmployee = string.Empty;


        /// <summary>
        /// Отдел в котором находится сотрудник
        /// </summary>
        public Department? DepartmentEmployee
        {
            get => _DepartmentEmployee;
            set
            {
                _DepartmentEmployee = value;
                OnPropertyChanged(nameof(DepartmentEmployee));
            }
        }
        private Department? _DepartmentEmployee;

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

        /// <summary>
        /// Выбранный отдел
        /// </summary>
        public Department? CurrentDepartment
        {
            get => _CurrentDepartment;
            set
            {
                _CurrentDepartment = value;
                OnPropertyChanged(nameof(CurrentDepartment));
            }
        }
        private Department? _CurrentDepartment;

        /// <summary>
        /// Значение чекбокса отвечающее, за поиск сотрудников по дате
        /// </summary>
        public bool CheckBoxIsSearch
        {
            get => _CheckBoxIsSearch;
            set
            {
                _CheckBoxIsSearch = value;
                OnPropertyChanged(nameof(CheckBoxIsSearch));
                SwitchForCheckBoxIsSearch();
            }
        }
        private bool _CheckBoxIsSearch;

        /// <summary>
        /// Видимость контролов
        /// </summary>
        public Visibility VisibilityControls
        {
            get => _VisibilityControls;
            set
            {
                _VisibilityControls = value;
                OnPropertyChanged(nameof(VisibilityControls));
            }
        }
        private Visibility _VisibilityControls = Visibility.Visible;

        /// <summary>
        /// Текст меняющийся после изменения значения CheckBoxIsSearch
        /// </summary>
        public string TextForFirstDate
        {
            get => _TextForFirstDate;
            set
            {
                _TextForFirstDate = value;
                OnPropertyChanged(nameof(TextForFirstDate));
            }
        }
        private string _TextForFirstDate = "Дата трудоустройства";

        /// <summary>
        /// Текст для второй даты, 
        /// меняющийся после изменения значения CheckBoxIsSearch
        /// </summary>
        public string TextForSecondDate
        {
            get => _TextForSecondDate;
            set
            {
                _TextForSecondDate = value;
                OnPropertyChanged(nameof(TextForSecondDate));
            }
        }
        private string _TextForSecondDate = "Дата увольнения";

        /// <summary>
        /// Текст для поиска по ФИО
        /// </summary>
        public string TextSearch
        {
            get => _TextSearch;
            set
            {
                _TextSearch = value;
                OnPropertyChanged(nameof(TextSearch));
            }
        }
        private string _TextSearch = string.Empty;

        /// <summary>
        /// Типы сохранения в лог
        /// </summary>
        public enum SaveLogAction
        {
            AddEmployee = 11,
            DeleteEmployee = 12,
            ChangeEmployee = 13,
            DismissEmployee = 14,
            AddDepartment = 21,
            DeleteDepartment = 22
        }

        #region КОМАНДЫ

        public RaiseCommand? ReadCsvFileCommand { get; set; }
        public RaiseCommand? SaveCsvFileCommand { get; set; }

        public RaiseCommand? AddEmployeeCommand { get; set; }
        public RaiseCommand? NewEmployeeCommand { get; set; }
        public RaiseCommand? DeleteEmployeeCommand { get; set; }
        public RaiseCommand? DismissEmployeeCommand { get; set; }
        public RaiseCommand? ChangeEmployeeCommand { get; set; }
        public RaiseCommand? ShowAllEmployeesCommand { get; set; }

        public RaiseCommand? AddDepartmentCommand { get; set; }
        public RaiseCommand? RenameDepartmentCommand { get; set; }
        public RaiseCommand? DeleteDepartmentCommand { get; set; }

        #endregion

        #endregion

        #region КОНСТРУКТОР

        public MainWindow()
        {
            InitializeComponent();

            LoadCommands();
            CreateFirstNode();

            if (File.Exists(Path))
            {
                ClearAllList();
                ReadCsvFile(Path);
                ConvertListsCsvInLists();
            }

            DataContext = this;
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
            SaveCsvFileCommand = new RaiseCommand(SaveCsvFileCommand_Execute);

            AddEmployeeCommand = new RaiseCommand(AddEmployeeCommand_Execute, AddEmployeeCommand_CanExecute);
            NewEmployeeCommand = new RaiseCommand(NewEmployeeCommand_Execute, NewEmployeeCommand_CanExecute);
            DismissEmployeeCommand = new RaiseCommand(DismissEmployeeCommand_Execute, DismissEmployeeCommand_CanExecute);
            DeleteEmployeeCommand = new RaiseCommand(DeleteEmployeeCommand_Execute, DeleteEmployeeCommand_CanExecute);
            ChangeEmployeeCommand = new RaiseCommand(ChangeEmployeeCommand_Execute, ChangeEmployeeCommand_CanExecute);
            ShowAllEmployeesCommand = new RaiseCommand(ShowAllEmployeesCommand_Execute, ShowAllEmployeesCommand_CanExecute);

            AddDepartmentCommand = new RaiseCommand(AddDepartmentCommand_Execute, AddDepartmentCommand_CanExecute);
            RenameDepartmentCommand = new RaiseCommand(RenameDepartmentCommand_Execute, RenameDepartmentCommand_CanExecute);
            DeleteDepartmentCommand = new RaiseCommand(DeleteDepartmentCommand_Execute, DeleteDepartmentCommand_CanExecute);
        }

        /// <summary>
        /// Создать первичный узел для дерева
        /// </summary>
        public void CreateFirstNode()
        {
            MainDepartment = new Department(TagClass: DepartmentAsCsv.Tag,
                                            Id_department: "0",
                                            NameDepartment: "Отделы",
                                            ListDepartments: null,
                                            DepartmentsString: string.Empty,
                                            ParentDepartment: null,
                                            ParentDepartmentString: string.Empty,
                                            TypeDepartment: "Main",
                                            ListEmployees: null );
        }

        /// <summary>
        /// Проверка команды, Показать всех сотрудников
        /// </summary>
        private bool ShowAllEmployeesCommand_CanExecute(object parameter)
        {
            if (ListAllEmployees != null && ListAllEmployees.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Выполнить команду, Показать всех сотрудников
        /// </summary>
        private void ShowAllEmployeesCommand_Execute(object parameter)
        {
            ListEmployeesSelectedDepartment.Clear();

            foreach (var employees in ListAllEmployees)
            {
                ListEmployeesSelectedDepartment.Add(employees);
            }
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
            Employee employee = new Employee(TagClass: EmployeeAsCsv.Tag,
                                            ID_employee: ListAllEmployees.Count,
                                            SurnameEmployee: SurnameEmployee,
                                            NameEmployee: NameEmployee,
                                            PatronymicEmployee: PatronymicEmployee,
                                            DepartmentEmployee: DepartmentEmployee,
                                            DateAdmissionEmployee: DateAdmissionEmployee ?? DateTime.Now.Date,
                                            DateDismissalEmployee: DateDismissalEmployee);

            if (ListAllEmployees.Any(x => Employee.CheckingFieldEquality(x, employee)))
            {
                MessageBox.Show("Данный сотрудник уже есть в списке", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            employee.ID_employee++;

            ListAllEmployees.Add(employee);
            ListAllDepartments.First(x => x == employee.DepartmentEmployee).ListEmployees.Add(employee);

            if (employee.DepartmentEmployee != null)
                SelectEmployeesForDepartment(employee.DepartmentEmployee);

            AddLogs(SaveLogAction.AddEmployee, employee, employee.DepartmentEmployee);
        }

        /// <summary>
        /// Проверка полей на заполнение
        /// </summary>
        public bool CheckField()
        {
            if (string.IsNullOrEmpty(TextBoxSurnameEmployee.Text))
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

        /// <summary>
        /// Проверка, есть ли что-то в полях для ввода информации о сотруднике
        /// </summary>
        public bool IsTextInField()
        {
            if (!string.IsNullOrEmpty(TextBoxSurnameEmployee.Text))
                return true;

            if (!string.IsNullOrEmpty(TextBoxNameEmployee.Text))
                return true;

            if (!string.IsNullOrEmpty(TextBoxPatronymicEmployee.Text))
                return true;

            if (!string.IsNullOrEmpty(ComboBoxListDepartment.Text))
                return true;

            if (!string.IsNullOrEmpty(DatePickerAdmissionEmployee.Text))
                return true;

            if (!string.IsNullOrEmpty(DatePickerAdmissionEmployee.Text))
                return true;

            return false;
        }

        /// <summary>
        /// Выполнить команду, Очистить поля для заполнения нового сотрудника 
        /// </summary>
        public void NewEmployeeCommand_Execute(object? parameter)
        {
            ClearTextInControls();
            CurrentEmployee = null;
            DataGridEmployee.SelectedItem = null;
        }

        /// <summary>
        /// Проверка команды, перед очищением полей
        /// </summary>
        private bool NewEmployeeCommand_CanExecute(object? parameter)
        {
            return IsTextInField();
        }

        /// <summary>
        /// Уволить сотрудника
        /// </summary>
        private void DismissEmployeeCommand_Execute(object? parameter)
        {
            var currentEmployee = ListAllEmployees.First(x => x == CurrentEmployee);
            DismissEmployee(currentEmployee);
        }

        /// <summary>
        /// Уволить сотрудника
        /// </summary>
        public void DismissEmployee(Employee employee)
        {
            employee.DateDismissalEmployee = DateDismissalEmployee;

            string dateAdmissionEmployee = DateAdmissionEmployee?.ToShortDateString() ?? string.Empty;
            string dateDismissalEmployee = DateDismissalEmployee?.ToShortDateString() ?? string.Empty;

            var text = string.Format($"Сотрудник: {SurnameEmployee} {NameEmployee} {PatronymicEmployee} \r\n" +
                                    $"Принят: {dateAdmissionEmployee}\r\n" +
                                    $"Уволен: {dateDismissalEmployee}");

            MessageBox.Show($"{text}", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);

            SelectEmployeesForDepartment(employee.DepartmentEmployee);

            AddLogs(SaveLogAction.DismissEmployee, employee, employee.DepartmentEmployee);
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
        private void DeleteEmployeeCommand_Execute(object parameter)
        {
            AddLogs(SaveLogAction.DeleteEmployee, CurrentEmployee, CurrentEmployee!.DepartmentEmployee);

            ListEmployeesSelectedDepartment.Remove(CurrentEmployee!);

            ListAllEmployees.Remove(CurrentEmployee!);
            ListAllDepartments.First(x => x.ListEmployees.Remove(CurrentEmployee!));

            CurrentEmployee = null;
        }

        /// <summary>
        /// Проверка для команды, удалить сотрудника
        /// </summary>
        private bool DeleteEmployeeCommand_CanExecute(object parameter)
        {
            return CurrentEmployee != null;
        }

        /// <summary>
        /// Проверка для команды, Изменить данные сотрудника
        /// </summary>
        private bool ChangeEmployeeCommand_CanExecute(object parameter)
        {
            if (CurrentEmployee == null)
                return false;

            if (CurrentEmployee.SurnameEmployee == SurnameEmployee &&
                CurrentEmployee.NameEmployee == NameEmployee &&
                CurrentEmployee.PatronymicEmployee == PatronymicEmployee &&
                CurrentEmployee.DepartmentEmployee == DepartmentEmployee &&
                CurrentEmployee.DateAdmissionEmployee == DateAdmissionEmployee &&
                CurrentEmployee.DateDismissalEmployee == DateDismissalEmployee)
                return false;

            return true;
        }

        /// <summary>
        /// Выполнить команду, Изменить данные сотрудника
        /// </summary>
        private void ChangeEmployeeCommand_Execute(object parameter)
        {
            TextInsertCurrentEmployee(CurrentEmployee);
        }

        /// <summary>
        /// Изменение информации о сотруднике
        /// </summary>
        public void TextInsertCurrentEmployee(Employee? employee)
        {
            if (employee == null)
                return;

            employee.SurnameEmployee = SurnameEmployee;
            employee.NameEmployee = NameEmployee;
            employee.PatronymicEmployee = PatronymicEmployee;

            ListAllDepartments.FirstOrDefault(dep => dep == employee.DepartmentEmployee)?.ListEmployees.Remove(employee);
            employee.DepartmentEmployee = DepartmentEmployee;
            ListAllDepartments.FirstOrDefault(dep => dep == employee.DepartmentEmployee)?.ListEmployees.Add(employee);

            employee.DateAdmissionEmployee = DateAdmissionEmployee ?? employee.DateAdmissionEmployee;
            employee.DateDismissalEmployee = DateDismissalEmployee;

            SelectEmployeesForDepartment(employee.DepartmentEmployee);

            AddLogs(SaveLogAction.ChangeEmployee, employee, employee.DepartmentEmployee);
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

            ClearAllList();

            ReadCsvFile(openFileDialog.FileName);

            ConvertListsCsvInLists();
        }

        /// <summary>
        /// Чтение CSV файла
        /// </summary>
        public void ReadCsvFile(string pathFile)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            StreamReader streamReader;
            try
            {
                streamReader = new StreamReader(pathFile, Encoding.GetEncoding(1251));
            }
            catch
            {
                streamReader = new StreamReader(pathFile, Encoding.UTF8);
            }

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = ";"
            };


            using (CsvReader? csvReader = new CsvReader(streamReader, csvConfig))
            {
                if (csvReader == null)
                    return;

                var countPropertiesInDepartment = DepartmentAsCsv.GetCountProperties();
                var countPropertiesInEmployee = EmployeeAsCsv.GetCountProperties();

                while (csvReader.Read())
                {
                    string stringReader = csvReader.GetField(0) ?? string.Empty;

                    switch (stringReader)
                    {
                        case "": continue;
                        case "TagClass": continue;

                        case DepartmentAsCsv.Tag:

                            if (csvReader.ColumnCount < countPropertiesInDepartment)
                                continue;

                            var departmentAsCsv = csvReader.GetRecord<DepartmentAsCsv>();

                            ListAllDepartmentsAsCsv.Add(new DepartmentAsCsv(
                                TagClass: departmentAsCsv.TagClass,
                                Id_department: departmentAsCsv.Id_department,
                                NameDepartment: departmentAsCsv.NameDepartment,
                                DepartmentsString: departmentAsCsv.DepartmentsString,
                                ParentDepartmentString: departmentAsCsv.ParentDepartmentString,
                                TypeDepartment: departmentAsCsv.TypeDepartment));
                            break;

                        case EmployeeAsCsv.Tag:

                            if (csvReader.ColumnCount < countPropertiesInEmployee)
                                continue;

                            var employeesAsCsv = csvReader!.GetRecord<EmployeeAsCsv>();

                            ListAllEmployeesAsCsv.Add(new EmployeeAsCsv(
                                TagClass: employeesAsCsv.TagClass,
                                ID_employee: employeesAsCsv.ID_employee,
                                SurnameEmployee: employeesAsCsv.SurnameEmployee,
                                NameEmployee: employeesAsCsv.NameEmployee,
                                PatronymicEmployee: employeesAsCsv.PatronymicEmployee,
                                DepartmentEmployee: employeesAsCsv.DepartmentEmployee,
                                DateAdmissionEmployee: employeesAsCsv.DateAdmissionEmployee,
                                DateDismissalEmployee: employeesAsCsv.DateDismissalEmployee));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Конвертируем Листы CSV в Листы для отображения
        /// </summary>
        public void ConvertListsCsvInLists()
        {
            RootDepartment.Add(MainDepartment);

            foreach (var departmentsAsCsv in ListAllDepartmentsAsCsv)
            {
                ListAllDepartments.Add(new Department(departmentsAsCsv));
            }

            foreach (var employeesAsCsv in ListAllEmployeesAsCsv)
            {
                ListAllEmployees.Add(new Employee(employeesAsCsv));
            }

            // Добавление в отделы сотрудников
            foreach (var department in ListAllDepartments)
            {
                // Список с сотрудниками для конкретного отдела 
                var listEmployees = from employee in ListAllEmployees
                                    where int.TryParse(employee.DepartmentEmployeeString, out int departmentEmployeeString) && departmentEmployeeString == department.Id_department
                                    select employee;

                foreach (var employee in listEmployees)
                {
                    department.ListEmployees?.Add(employee);
                    employee.DepartmentEmployee = department;
                }

                //Запись Родительского отдела в Отдел
                department.ParentDepartment = (from dep in ListAllDepartments
                                               where int.TryParse(department.ParentDepartmentString, out int depOut) && depOut == dep.Id_department
                                               select dep).FirstOrDefault(MainDepartment);

                if (department.ParentDepartment == MainDepartment)
                    MainDepartment.ListDepartments?.Add(department);

                #region Добавление дочерних отделов в Родительский отдел (Не актуально)
                //      Добавление через строку подчиненные отделы

                //var numbersDepartments = from dep in department.DepartmentsString.Split(",")
                //                        where int.TryParse(dep, out int depInt) 
                //                        select int.Parse(dep);

                //var listSubsidiaryDepartment1 = ListAllDepartments.Join(numbersDepartments,
                //                                                      dep => dep.Id_department,
                //                                                      number => number, 
                //                                                      (dep, id) => new Department(TagClass: dep.TagClass,
                //                                                                                  Id_department: dep.Id_department,
                //                                                                                  NameDepartment: dep.NameDepartment,
                //                                                                                  ListDepartments: dep.ListDepartments,
                //                                                                                  ParentDepartment: dep.ParentDepartment,
                //                                                                                  TypeDepartment: dep.TypeDepartment,
                //                                                                                  ListEmployees: dep.ListEmployees)).ToList();

                //foreach (var dep in listSubsidiaryDepartment1)
                //{
                //    department.ListDepartments.Add(dep);
                //}

                #endregion
            }

            //  Добавление дочерних отделов в Родительский отдел, через строку Родительский отдел
            foreach (var dep1 in ListAllDepartments)
            {
                var listSubsidiaryDepartment = ListAllDepartments.Where(dep2 => dep2.ParentDepartment?.Id_department == dep1.Id_department).ToList();
                foreach (var subsidiaryDepartment in listSubsidiaryDepartment)
                {
                    if (listSubsidiaryDepartment != null)
                        dep1.ListDepartments.Add(subsidiaryDepartment);
                }
            }
        }

        /// <summary>
        /// Выполнить команду, Сохранить данные в CSV файл
        /// </summary>
        private void SaveCsvFileCommand_Execute(object? parameter)
        {
            SaveCsvFile("Последовательное");
        }

        /// <summary>
        /// Сохранение CSV файла
        /// </summary>
        public void SaveCsvFile(string? typeSaveCsv = "Последовательное")
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using var writer = new StreamWriter(Path, false, Encoding.GetEncoding(1251));

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";"
            };

            using (var csv = new CsvWriter(writer, csvConfig))
            {
                csv.WriteHeader<DepartmentAsCsv>();
                csv.NextRecord();

                foreach (var department in ListAllDepartments)
                {
                    csv.WriteRecord(new DepartmentAsCsv(department));
                    csv.NextRecord();

                    if (typeSaveCsv == "Чередующееся")
                        foreach (var employee in department.ListEmployees)
                        {
                            csv.WriteRecord(new EmployeeAsCsv(employee));
                            csv.NextRecord();
                        }
                }

                if (typeSaveCsv == "Последовательное")
                {
                    csv.NextRecord(); // Отделить отделы от сотрудников
                    csv.WriteHeader<EmployeeAsCsv>();
                    csv.NextRecord();

                    foreach (var employee in ListAllEmployees)
                    {
                        csv.WriteRecord(new EmployeeAsCsv(employee));
                        csv.NextRecord();
                    }
                }
            }

            MessageBox.Show("Сохранение CSV завершено.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);

            OpenFolder();
        }

        /// <summary>
        /// Выбор отдела для отображения сотрудников
        /// </summary>
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeViewSelectedItem = (sender as TreeView)?.SelectedItem;
            var department = treeViewSelectedItem as Department;

            if (department is null)
                return;

            CurrentDepartment = department;

            SelectEmployeesForDepartment(department);
        }

        /// <summary>
        /// Отображение списка сотрудников для конкретного отдела
        /// </summary>
        public void SelectEmployeesForDepartment(Department? department)
        {
            if (department == null)
                return;

            CurrentEmployee = null;
            ListEmployeesSelectedDepartment.Clear();
            ClearTextInControls();

            foreach (var item in department.ListEmployees)
            {
                ListEmployeesSelectedDepartment.Add(item);
            }
        }

        /// <summary>
        /// Открытие папки с выделенным файлом после сохранения
        /// </summary>
        public void OpenFolder()
        {
            string exp = "C:\\Windows\\explorer.exe";
            if (File.Exists(exp))
            {
                Process process = new Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.Arguments = $"/select, \"{@Path}\"";
                process.StartInfo.FileName = exp;
                process.Start();
            }
        }

        /// <summary>
        /// Чистка всех списков
        /// </summary>
        public void ClearAllList()
        {
            ListAllDepartments.Clear();
            ListAllDepartmentsAsCsv.Clear();
            ListAllEmployees.Clear();
            ListAllEmployeesAsCsv.Clear();
            ListEmployeesSelectedDepartment.Clear();

            RootDepartment.Clear();
            MainDepartment?.ListDepartments.Clear();

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

        /// <summary>
        /// Получение списка всех сотрудников 
        /// Не актуально
        /// </summary>
        public dynamic GetAllEmployees()
        {
            var employees = from department in ListAllDepartments
                            from employee in department.ListEmployees
                            select employee;
            return employees;
        }

        /// <summary>
        /// Получить список сотрудников за определенный промежуток времени
        /// </summary>
        public void GetListEmployeesSpecificDate(List<Employee>? employees)
        {
            if (!CheckBoxIsSearch)
                return;

            if (DateAdmissionEmployee == null || DateDismissalEmployee == null)
                return;

            if (DateAdmissionEmployee > DateDismissalEmployee)
                return;

            List<Employee> listEmployeeDate = [];

            if (employees?.Count > 0)
            {
                listEmployeeDate = (from employee in employees
                                    where employee.DateAdmissionEmployee >= DateAdmissionEmployee && employee.DateAdmissionEmployee <= DateDismissalEmployee
                                    select employee).ToList();
            }
            else
            {
                listEmployeeDate = (from employee in ListAllEmployees
                                    where employee.DateAdmissionEmployee >= DateAdmissionEmployee && employee.DateAdmissionEmployee <= DateDismissalEmployee
                                    select employee).ToList();
            }

            ListEmployeesSelectedDepartment.Clear();
            foreach (var employee in listEmployeeDate)
            {
                ListEmployeesSelectedDepartment.Add(employee);
            }
        }

        /// <summary>
        /// Изменение свойств при изменении значения CheckBoxIsSearch
        /// </summary>
        public void SwitchForCheckBoxIsSearch()
        {
            if (CheckBoxIsSearch)
            {
                VisibilityControls = Visibility.Collapsed;
                TextForFirstDate = "Первая дата";
                TextForSecondDate = "Вторая дата";
            }
            else
            {
                VisibilityControls = Visibility.Visible;
                TextForFirstDate = "Дата трудоустройства";
                TextForSecondDate = "Дата увольнения";
            }

            DateAdmissionEmployee = null;
            DateDismissalEmployee = null;
        }

        /// <summary>
        /// Выбор конечной даты (Отображение сотрудников за выбранный промежуток времени)
        /// </summary>
        private void DatePickerDismissalEmployee_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            GetListEmployeesSpecificDate(null);
        }

        /// <summary>
        /// Выбор первичной даты (Отображение сотрудников за выбранный промежуток времени)
        /// </summary>
        private void DatePickerAdmissionEmployee_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            GetListEmployeesSpecificDate(null);
        }

        /// <summary>
        /// Выполнить команду, Добавить новый отдел
        /// </summary>
        private void AddDepartmentCommand_Execute(object parameter)
        {
            AddNewDepartment();
        }

        /// <summary>
        /// Проверка команды, Добавить новый отдел
        /// </summary>
        private bool AddDepartmentCommand_CanExecute(object parameter)
        {
            if (ListAllDepartments.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Добавить новый отдел
        /// </summary>
        public void AddNewDepartment()
        {
            var tempList = new List<Department>( ListAllDepartments );
            if(MainDepartment != null)
                tempList.Add(MainDepartment);

            WindowAddNewDepartment windowAddNewDepartment = new WindowAddNewDepartment(tempList, CurrentDepartment);
            windowAddNewDepartment.ShowDialog();

            var numberNewDepartment = ListAllDepartments.Count + 1;
            if (string.IsNullOrEmpty(windowAddNewDepartment.NameDepartment))
            {
                MessageBox.Show("Не задано имя", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Department newDepartment = new Department(TagClass: DepartmentAsCsv.Tag,
                                                      Id_department: numberNewDepartment,
                                                      NameDepartment: windowAddNewDepartment.NameDepartment,
                                                      ParentDepartment: windowAddNewDepartment.SelectedDepartment,
                                                      TypeDepartment: Department.TypeDepartments.Subordinate);

            ListAllDepartments.Add(newDepartment);
            windowAddNewDepartment.SelectedDepartment.ListDepartments.Add(newDepartment);

            AddLogs(SaveLogAction.AddDepartment, null, newDepartment);
        }

        /// <summary>
        /// Проверка команды, Удалить отдел
        /// </summary>
        private bool DeleteDepartmentCommand_CanExecute(Object parameter)
        {
            return CurrentDepartment != null;
            //return (parameter as TreeView)?.SelectedItem != null;
        }

        /// <summary>
        /// Выполнить команду, Удалить отдел
        /// </summary>
        private void DeleteDepartmentCommand_Execute(Object parameter)
        {
            var selectedItem = (parameter as TreeView)?.SelectedItem;
            if (selectedItem == null)
            {
                selectedItem = CurrentDepartment;
            }

            if (selectedItem is Department selectedDepartment)
            {
                ListAllDepartments.Remove(selectedDepartment);

                DeleteDepartment(selectedDepartment, MainDepartment.ListDepartments);
            }
        }

        /// <summary>
        /// Удалить отдел
        /// </summary>
        public void DeleteDepartment(Department department, ObservableCollection<Department> ObserCollDepartments)
        {
            if (!ObserCollDepartments.Remove(department))
            {
                foreach (var item in ObserCollDepartments.Select(x => x.ListDepartments))
                {
                    DeleteDepartment(department, item);
                }
            }
            else
                AddLogs(SaveLogAction.DeleteDepartment, null, department);
        }

        /// <summary>
        /// Фильтр списка сотрудников
        /// </summary>
        public void FilterEmployees()
        {
            if (ListEmployeesSelectedDepartment.Count < 1)
            {
                foreach (var item in ListAllEmployees)
                    ListEmployeesSelectedDepartment.Add(item);
            }

            var list = (from employee in ListAllEmployees
                        where employee.SurnameEmployee.Contains($"{TextSearch}", StringComparison.CurrentCultureIgnoreCase) ||
                              employee.NameEmployee.Contains($"{TextSearch}", StringComparison.CurrentCultureIgnoreCase) ||
                              employee.PatronymicEmployee.Contains($"{TextSearch}", StringComparison.CurrentCultureIgnoreCase)
                        select employee).ToList();

            ListEmployeesSelectedDepartment.Clear();

            if (list?.Count > 0)
            {
                foreach (var employee in list)
                {
                    ListEmployeesSelectedDepartment.Add(employee);
                }
            }
            else if (TextSearch == string.Empty)
            {
                foreach (var employee in ListAllEmployees)
                {
                    ListEmployeesSelectedDepartment.Add(employee);
                }
            }

            if (CheckBoxIsSearch)
                GetListEmployeesSpecificDate(ListEmployeesSelectedDepartment.ToList());
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                var text = (sender as TextBox)?.Text ?? string.Empty;
                TextSearch = text;
                FilterEmployees();
            }
        }

        /// <summary>
        /// Добавление логов 
        /// </summary>
        public async void AddLogs(SaveLogAction saveLogAction, Employee? employee, Department? department)
        {
            if (employee == null &&
                (SaveLogAction.AddEmployee == saveLogAction ||
                SaveLogAction.DeleteEmployee == saveLogAction ||
                SaveLogAction.ChangeEmployee == saveLogAction ||
                SaveLogAction.DismissEmployee == saveLogAction))
                return;

            if (department == null)
                return;

            string text = string.Empty;
            string dateNow = DateTime.Now.ToString();

            string idEmp = employee?.ID_employee.ToString() ?? string.Empty;
            string surnameEmp = employee?.SurnameEmployee ?? string.Empty;
            string nameEmp = employee?.NameEmployee ?? string.Empty;
            string patEmp = employee?.PatronymicEmployee ?? string.Empty;
            string dateAdmissionEmp = employee?.DateAdmissionEmployee.ToString("d") ?? string.Empty;
            string dateDismissalEmp = employee?.DateDismissalEmployee?.ToString("d") ?? "нет";

            string nameDep = department.NameDepartment;
            string idDep = department.Id_department.ToString();
            string parentDep = department.ParentDepartment?.NameDepartment ?? "нет";
            string idParentDep = department.ParentDepartment?.Id_department.ToString() ?? "нет";

            #region ПОЛУЧЕНИЕ СПИСКА УДАЛЕННЫХ ПОДЧИНЕННЫХ ОТДЕЛОВ

            List<Department> listSubsidiaryDepartments = new List<Department>();
            GetListSubsidiaryDepartments(department.ListDepartments.ToList(), listSubsidiaryDepartments);

            string textSubDeps = string.Empty;

            if (listSubsidiaryDepartments.Count < 1)
                textSubDeps = "нет";
            else
            {
                foreach (var dep in listSubsidiaryDepartments)
                {
                    textSubDeps += "id: \"" + dep.Id_department + "\" название: \"" + dep.NameDepartment + "\", ";
                }
                textSubDeps = textSubDeps.Substring(0, textSubDeps.Length - 2);
            }

            #endregion

            switch (saveLogAction)
            {
                case SaveLogAction.AddEmployee:
                    text = string.Format($"Добавлен сотрудник - id: \"{idEmp}\", ФИО: \"{surnameEmp} {nameEmp} {patEmp}\", " +
                                         $"дата трудоустройства: \"{dateAdmissionEmp}\", дата увольнения: \"{dateDismissalEmp}\", " +
                                         $"в отдел - id: \"{idDep}\" название: \"{nameDep}\", дата: \"{dateNow}\"");
                    break;

                case SaveLogAction.DeleteEmployee:
                    text = string.Format($"Удален сотрудник - id:\"{idEmp}\", ФИО: \"{surnameEmp} {nameEmp} {patEmp}\", " +
                                         $"дата трудоустройства: \"{dateAdmissionEmp}\", дата увольнения: \"{dateDismissalEmp}\", " +
                                         $"отдел - id: \"{idDep}\" название: \"{nameDep}\", дата: \"{dateNow}\"");
                    break;

                case SaveLogAction.ChangeEmployee:
                    text = string.Format($"Изменен сотрудник - id:\"{idEmp}\", ФИО: \"{surnameEmp} {nameEmp} {patEmp}\", " +
                                         $"дата трудоустройства: \"{dateAdmissionEmp}\", дата увольнения: \"{dateDismissalEmp}\", " +
                                         $"отдел - id: \"{idDep}\" название: \"{nameDep}\", дата: \"{dateNow}\"");
                    break;

                case SaveLogAction.DismissEmployee:
                    text = string.Format($"Уволен сотрудник - id:\"{idEmp}\", ФИО: \"{surnameEmp} {nameEmp} {patEmp}\", " +
                                         $"дата трудоустройства: \"{dateAdmissionEmp}\", дата увольнения: \"{dateDismissalEmp}\", " +
                                         $"отдел - id: \"{idDep}\" название: \"{nameDep}\", дата: \"{dateNow}\"");
                    break;

                case SaveLogAction.AddDepartment:
                    text = string.Format($"Добавлен отдел - id: \"{idDep}\" название: \"{nameDep}\", родительский отдел - id: \"{idParentDep}\" название: \"{parentDep}\", дата: \"{dateNow}\"");
                    break;

                case SaveLogAction.DeleteDepartment:
                    text = string.Format($"Удален отдел - id: \"{idDep}\" название: \"{nameDep}\", родительский отдел - id: \"{idParentDep}\" название: \"{parentDep}\", " +
                                         $"дочерние отделы - ({textSubDeps}), дата: \"{dateNow}\"");
                    break;

                default:
                    break;
            }

            using (StreamWriter writer = new StreamWriter(PathForLogs, ExistFile()))
            {
                await writer.WriteLineAsync(text);
            }
        }

        /// <summary>
        /// Проверка, существует ли файл
        /// </summary>
        public bool ExistFile()
        {
            return File.Exists(PathForLogs);
        }

        /// <summary>
        /// Получение списка дочерних (подчиненных) отделов
        /// </summary>
        public void GetListSubsidiaryDepartments(List<Department> listSubsidiaryDepartments, List<Department> plusDep)
        {
            foreach (var department in listSubsidiaryDepartments)
            {
                plusDep.Add(department);

                if (department.ListDepartments.Count > 0)
                    GetListSubsidiaryDepartments(department.ListDepartments.ToList(), plusDep);
            }
        }

        /// <summary>
        /// Проверка команды, переименовать отдел
        /// </summary>
        private bool RenameDepartmentCommand_CanExecute(object parameter)
        {
            return CurrentDepartment != null;
        }

        /// <summary>
        /// Выполнить команду, переименовать отдел
        /// </summary>
        private void RenameDepartmentCommand_Execute(object parameter)
        {
            RenameDepartment(CurrentDepartment);
        }

        /// <summary>
        /// Переименовать отдел
        /// </summary>
        private void RenameDepartment(Department? department)
        {
            if (department == null)
                return;
            
            var list = ListAllDepartments;
            if(MainDepartment != null)
                list.Add(MainDepartment);

            WindowInput windowInput = new WindowInput(list, department);
            windowInput.ShowDialog();

            if (windowInput.CanExecute == true)
                department.NameDepartment = windowInput.NameDepartment;

            System.Windows.Data.CollectionViewSource.GetDefaultView(RootDepartment).Refresh();
        }

        /// <summary>
        /// Получить все отделы
        /// </summary>
        public static List<Department> GetAllDepartment(Department department)
        {
            var result = new List<Department> { department };
            foreach (var listDepartments in department.ListDepartments)
            {
                result.AddRange(GetAllDepartment(listDepartments));
            }
            return result;
        }

        #endregion

    } //END
}