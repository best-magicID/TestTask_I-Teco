﻿using System;
using System.Diagnostics;
using System.Reflection;

namespace PersonnelSystem.Classes
{
    /// <summary>
    /// Отдел прочитанный из CSV файла
    /// </summary>
    public class DepartmentAsCsv
    {
        public const string Tag = "Department";

        /// <summary>
        /// Тег класса
        /// </summary>
        public string TagClass { get; set; } = Tag;

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
        /// Родительский отдел
        /// </summary>
        public string ParentDepartmentString { get; set; } = string.Empty;

        /// <summary>
        /// Тип отдела
        /// </summary>
        public string TypeDepartment { get; set; } = string.Empty;

        /// <summary>
        /// Список сотрудников
        /// </summary>
        //public string EmployeesString { get; set; } = string.Empty;


        public DepartmentAsCsv()
        {

        }

        public DepartmentAsCsv(string TagClass,
                               string Id_department,
                               string NameDepartment,
                               string DepartmentsString,
                               string ParentDepartmentString,
                               string TypeDepartment)
        {
            this.TagClass = TagClass;
            this.Id_department = Id_department;
            this.NameDepartment = NameDepartment;
            this.DepartmentsString = DepartmentsString;
            this.ParentDepartmentString = ParentDepartmentString;
            this.TypeDepartment = TypeDepartment;
        }

        public DepartmentAsCsv(Department department)
        {
            this.TagClass= department.TagClass;
            this.Id_department = department.Id_department.ToString();
            this.NameDepartment= department.NameDepartment.ToString();
            this.DepartmentsString = string.Join(",", department.ListDepartments.Select(x => x.Id_department).ToArray());
            this.ParentDepartmentString = department.ParentDepartment?.Id_department.ToString() ?? string.Empty;
            this.TypeDepartment = department.TypeDepartment.ToString();
        }

        static public int GetCountProperties()
        {
            DepartmentAsCsv departmentAsCsv = new DepartmentAsCsv();
            var countField = departmentAsCsv.GetType().GetProperties().Length;
            return countField;
        }
    }
}
