using System;

namespace PersonnelSystem.Classes
{
    public class DepartmentAsCsv
    {
        public const string Tag = "Department";

        /// <summary>
        /// Тег класса
        /// </summary>
        public string TagClass { get; set; } = string.Empty;

        /// <summary>
        /// ID отдела
        /// </summary>
        public string Id_department { get; set; } = string.Empty;

        /// <summary>
        /// Название отдела
        /// </summary>
        public string NameDepartment { get; set; } = string.Empty;

        /// <summary>
        /// Подчиненные отделы
        /// </summary>
        public string DepartmentsString { get; set; } = string.Empty;

        /// <summary>
        /// Тип отдела
        /// </summary>
        public string TypeDepartment { get; set; } = string.Empty;

        /// <summary>
        /// Список сотрудников
        /// </summary>
        public string ListEmployees { get; set; } = string.Empty;

        public DepartmentAsCsv(string TagClass, string Id_department, string NameDepartment, string DepartmentsString, string TypeDepartment, string ListEmployees)
        {
            this.TagClass = TagClass;
            this.Id_department = Id_department;
            this.NameDepartment = NameDepartment;
            this.DepartmentsString = DepartmentsString;
            this.TypeDepartment = TypeDepartment;
            this.ListEmployees = ListEmployees;
        }

        public DepartmentAsCsv() 
        {
            
        }
    }
}
