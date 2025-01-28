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
        public string TagClass { get; set; }

        /// <summary>
        /// ID Сотрудника
        /// </summary>
        public string ID_employee;

        /// <summary>
        /// Фамилия сотрудника
        /// </summary>
        public string SurnameEmployee { get; set; }

        /// <summary>
        /// Имя сотрудника
        /// </summary>
        public string NameEmployee { get; set; }

        /// <summary>
        /// Отчество сотрудника
        /// </summary>
        public string PatronymicEmployee { get; set; }

        /// <summary>
        /// Отдел в котором числится сотрудник
        /// </summary>
        public string DepartmentEmployee { get; set; }

        /// <summary>
        /// Дата приема на работу нового сотрудника
        /// </summary>
        public string DateAdmissionEmployee { get; set; } 

        /// <summary>
        /// Дата увольнения сотрудника
        /// </summary>
        public string DateDismissalEmployee { get; set; }

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
    }
}
