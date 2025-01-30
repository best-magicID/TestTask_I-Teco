﻿using System;
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
        public List<Department> ListDepartments { get; set; } = new List<Department>();

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
            Main = 1,
            Subordinate = 2
        };

        /// <summary>
        /// Список сотрудников
        /// </summary>
        public List<Employee> ListEmployees { get; set; } = new List<Employee>();


        public Department(string TagClass,
                          string Id_department,
                          string NameDepartment,
                          string DepartmentsString,
                          string ParentDepartmentString,
                          string TypeDepartment,
                          List<Employee> ListEmployees)
        {
            this.TagClass = TagClass;

            if(int.TryParse(Id_department, out int id_department))
                this.Id_department = id_department;
            else
                this.Id_department = 0;

            this.NameDepartment = NameDepartment;
            this.DepartmentsString = DepartmentsString;
            this.ParentDepartmentString = ParentDepartmentString;

            if (int.TryParse(TypeDepartment, out int typeDepartment))
                this.TypeDepartment = (TypeDepartments)typeDepartment;
            else
                this.TypeDepartment = TypeDepartments.Subordinate;

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

            if (int.TryParse(departmentAsCsv.TypeDepartment, out int typeDepartment))
                this.TypeDepartment = (TypeDepartments)typeDepartment;
            else
                this.TypeDepartment = TypeDepartments.Subordinate;
        }
    }
}
