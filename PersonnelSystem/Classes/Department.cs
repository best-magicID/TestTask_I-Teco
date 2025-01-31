using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PersonnelSystem.Classes
{
    public class Department
    {
        /// <summary>
        /// Тег класса
        /// </summary>
        public string TagClass { get; set; }

        /// <summary>
        /// ID отдела
        /// </summary>
        public int Id_department { get; set; }

        /// <summary>
        /// Название отдела
        /// </summary>
        public string NameDepartment { get; set; }

        /// <summary>
        /// Подчиненные отделы
        /// </summary>
        public List<Department>? ListDepartments { get; set; } 

        /// <summary>
        /// Список отделов для чтения из CSV
        /// </summary>
        public string DepartmentsString { get; set; } = string.Empty;

        /// <summary>
        /// Родительский отдел
        /// </summary>
        public Department? ParentDepartment { get; set; }

        /// <summary>
        /// Родительский отдел для чтения из CSV
        /// </summary>
        public string ParentDepartmentString { get; set; } = string.Empty;

        /// <summary>
        /// Тип отдела
        /// </summary>
        public TypeDepartments TypeDepartment { get; set; } = TypeDepartments.Subordinate;

        /// <summary>
        /// Список типов отделов
        /// </summary>
        public enum TypeDepartments
        {
            Main = 0,
            Subordinate = 1
        };

        /// <summary>
        /// Список сотрудников
        /// </summary>
        public List<Employee>? ListEmployees { get; set; }


        public Department(string TagClass,
                          string Id_department,
                          string NameDepartment,
                          ObservableCollection<Department>? ListDepartments,
                          string DepartmentsString,
                          Department? ParentDepartment,
                          string ParentDepartmentString,
                          string TypeDepartment,
                          List<Employee>? ListEmployees)
        {
            this.TagClass = TagClass;

            if(int.TryParse(Id_department, out int id_department))
                this.Id_department = id_department;
            else
                this.Id_department = 0;

            this.NameDepartment = NameDepartment;

            this.ListDepartments = ListDepartments?.ToList();
            this.DepartmentsString = DepartmentsString;

            this.ParentDepartment = ParentDepartment;
            this.ParentDepartmentString = ParentDepartmentString;

            if (TypeDepartments.TryParse(TypeDepartment, out TypeDepartments typeDepartment))
                this.TypeDepartment = typeDepartment;

            this.ListEmployees = ListEmployees;
        }

        public Department(DepartmentAsCsv departmentAsCsv)
        {
            this.TagClass = departmentAsCsv.TagClass;

            if (int.TryParse(departmentAsCsv.Id_department, out int id_department))
                this.Id_department = id_department;
            else
                this.Id_department = 0;

            this.NameDepartment = departmentAsCsv.NameDepartment;
            this.DepartmentsString = departmentAsCsv.DepartmentsString;
            this.ParentDepartmentString = departmentAsCsv.ParentDepartmentString;

            if (TypeDepartments.TryParse(departmentAsCsv.TypeDepartment, out TypeDepartments typeDepartment))
                this.TypeDepartment = typeDepartment;
        }
    }
}
