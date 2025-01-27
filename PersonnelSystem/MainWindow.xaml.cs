using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using PersonnelSystem.Classes;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonnelSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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

        #endregion

        #region КОНСТРУКТОР

        public MainWindow()
        {
            InitializeComponent();

            LoadCommands();

            ReadCsvFileCommand.Execute(null);
        }

        #endregion

        #region МЕТОДЫ И КОМАНДЫ

        public void LoadCommands()
        {
            ReadCsvFileCommand = new RaiseCommand(ReadCsvFileCommand_Execute);
        }

        private void ReadCsvFileCommand_Execute(object? parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            using var streamReader = new StreamReader(openFileDialog.FileName, Encoding.UTF8);
           
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = ";"
            };

            ListDepartments.Clear();
            ListDepartmentsAsCsv.Clear();
            ListEmployees.Clear();
            ListEmployeesAsCsv.Clear();

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
                                NameEmployee: employeesAsCsv.NameEmployee,
                                SurnameEmployee: employeesAsCsv.SurnameEmployee,
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


            //var records = csvReader.GetRecords<DepartmentAsCsv>();

            //foreach (var record in records)
            //{
            //    ListDepartmentsAsCsv.Add(new DepartmentAsCsv(
            //        TagClass: record.TagClass,
            //        Id_department: record.Id_department,
            //        NameDepartment: record.NameDepartment,
            //        DepartmentsString: record.DepartmentsString,
            //        TypeDepartment: record.TypeDepartment));
            //}
            

            foreach (var departmentsAsCsv in ListDepartmentsAsCsv)
            {
                ListDepartments.Add(new Department(departmentsAsCsv));
            }
            TreeViewDepartments.ItemsSource = ListDepartments;

            foreach (var item in ListEmployeesAsCsv)
            {
                ListEmployees.Add(new Employee(item));
            }
            ListViewEmployees.ItemsSource = ListEmployees;
        }

        #endregion

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ListEmployees.Clear();

            //for (int i = 0; i < 6; i++)
            //{
            //    ListEmployees.Add(new Employee(
            //        ID_employee: i, 
            //        NameEmployee: i + "Name", 
            //        SurnameEmployee: i + "Surname", 
            //        PatronymicEmployee: i + "Patronymic", 
            //        DepartmentEmployee: ListDepartments[0], 
            //        DateAdmission: DateTime.Now));
            //}

            //ListViewEmployees.ItemsSource = ListEmployees;
        }

    } //END
}