using System;

namespace PersonnelSystem.Classes
{
    /// <summary>
    /// Информация о сотруднике в CSV
    /// </summary>
    public class EmployeeAsCsv
    {
        public const string Tag = "Employee";

        /// <summary>
        /// Тег класса
        /// </summary>
        public string TagClass { get; set; } = Tag;

        /// <summary>
        /// ID Сотрудника
        /// </summary>
        public string ID_employee { get; set; } = string.Empty;

        /// <summary>
        /// Фамилия сотрудника
        /// </summary>
        public string SurnameEmployee { get; set; } = string.Empty;

        /// <summary>
        /// Имя сотрудника
        /// </summary>
        public string NameEmployee { get; set; } = string.Empty;

        /// <summary>
        /// Отчество сотрудника
        /// </summary>
        public string PatronymicEmployee { get; set; } = string.Empty;

        /// <summary>
        /// Отдел в котором числится сотрудник
        /// </summary>
        public string DepartmentEmployee { get; set; } = string.Empty;

        /// <summary>
        /// Дата приема на работу нового сотрудника
        /// </summary>
        public string DateAdmissionEmployee { get; set; } = string.Empty;

        /// <summary>
        /// Дата увольнения сотрудника
        /// </summary>
        public string DateDismissalEmployee { get; set; } = string.Empty;


        public EmployeeAsCsv() 
        { 

        }

        public EmployeeAsCsv(string TagClass,
                             string ID_employee,
                             string SurnameEmployee,
                             string NameEmployee,
                             string PatronymicEmployee,
                             string DepartmentEmployee,
                             string DateAdmissionEmployee,
                             string DateDismissalEmployee)
        {
            this.TagClass = TagClass;
            this.ID_employee = ID_employee;

            this.SurnameEmployee = SurnameEmployee; 
            this.NameEmployee = NameEmployee;
            this.PatronymicEmployee = PatronymicEmployee;

            this.DepartmentEmployee = DepartmentEmployee;

            this.DateAdmissionEmployee = DateAdmissionEmployee;
            this.DateDismissalEmployee = DateDismissalEmployee;
        }

        public EmployeeAsCsv(Employee employee)
        {
            this.TagClass = employee.TagClass;
            this.ID_employee = employee.ID_employee.ToString();
            
            this.SurnameEmployee = employee.SurnameEmployee;
            this.NameEmployee = employee.NameEmployee;
            this.PatronymicEmployee = employee.PatronymicEmployee;

            this.DepartmentEmployee = employee.DepartmentEmployee?.Id_department.ToString() ?? string.Empty;

            this.DateAdmissionEmployee = employee.DateAdmissionEmployee.ToShortDateString();
            this.DateDismissalEmployee = employee.DateDismissalEmployee?.ToShortDateString() ?? string.Empty;
        }

        static public int GetCountProperties()
        {
            EmployeeAsCsv employeeAsCsv = new EmployeeAsCsv();
            var countField = employeeAsCsv.GetType().GetProperties().Length;
            return countField;
        }
    }
}
