using System;

namespace PersonnelSystem.Classes
{
    /// <summary>
    /// Информация о сотруднике
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Тег класса
        /// </summary>
        public string TagClass { get; set; } = string.Empty;

        /// <summary>
        /// ID Сотрудника
        /// </summary>
        public int ID_employee;

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
        public Department? DepartmentEmployee { get; set; }

        /// <summary>
        /// Отдел в котором числится сотрудник
        /// </summary>
        public string DepartmentEmployeeString { get; set; } = string.Empty;

        /// <summary>
        /// Дата приема на работу нового сотрудника
        /// </summary>
        public DateTime DateAdmissionEmployee { get; set; } = DateTime.Now.Date;

        /// <summary>
        /// Дата увольнения сотрудника
        /// </summary>
        public DateTime? DateDismissalEmployee { get; set; } = null;


        public Employee()
        {

        }

        public Employee(string TagClass, int ID_employee, string SurnameEmployee, string NameEmployee,  string PatronymicEmployee, Department? DepartmentEmployee, DateTime DateAdmissionEmployee, DateTime? DateDismissalEmployee = null)
        {
            this.TagClass = TagClass;

            this.ID_employee = ID_employee;

            this.SurnameEmployee = SurnameEmployee;
            this.NameEmployee = NameEmployee;
            this.PatronymicEmployee = PatronymicEmployee;

            this.DepartmentEmployee = DepartmentEmployee;
            if(DepartmentEmployee!= null) 
                this.DepartmentEmployeeString = DepartmentEmployee.Id_department.ToString();

            this.DateAdmissionEmployee = DateAdmissionEmployee;
            this.DateDismissalEmployee = DateDismissalEmployee;
        }

        public Employee(EmployeeAsCsv employeesAsCsv) 
        {
            this.TagClass = employeesAsCsv.TagClass;

            if (int.TryParse(employeesAsCsv.ID_employee, out int id_employee))
                this.ID_employee = id_employee;
            else
                this.ID_employee = 0;

            this.SurnameEmployee = employeesAsCsv.SurnameEmployee;
            this.NameEmployee = employeesAsCsv.NameEmployee;
            this.PatronymicEmployee = employeesAsCsv.PatronymicEmployee;

            this.DepartmentEmployeeString = employeesAsCsv.DepartmentEmployee;

            if (DateTime.TryParse(employeesAsCsv.DateAdmissionEmployee, out DateTime dateAdmissionEmployee))
                this.DateAdmissionEmployee = dateAdmissionEmployee;
            else
                this.DateAdmissionEmployee = DateTime.Now.Date;

            if (DateTime.TryParse(employeesAsCsv.DateDismissalEmployee, out DateTime dateDismissalEmployee))
                this.DateDismissalEmployee = dateDismissalEmployee;
            else
                this.DateDismissalEmployee = null;
        }

        /// <summary>
        /// Проверка, есть ли одинаковые сотрудники
        /// </summary>
        public static bool CheckingFieldEquality(Employee employee1, Employee employee2)
        {
            if(employee1 == employee2) 
                return true;

            else if(employee1.TagClass == employee2.TagClass &&
                    employee1.SurnameEmployee == employee2.SurnameEmployee &&
                    employee1.NameEmployee == employee2.NameEmployee &&
                    employee1.PatronymicEmployee == employee2.PatronymicEmployee &&
                    employee1.DepartmentEmployee == employee2.DepartmentEmployee &&
                    employee1.DateAdmissionEmployee.Date == employee2.DateAdmissionEmployee.Date)
            {
                return true;
            }
            else
                return false;
        }

    }
}
