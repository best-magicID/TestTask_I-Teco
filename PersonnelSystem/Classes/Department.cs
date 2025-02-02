using System.Collections.ObjectModel;

namespace PersonnelSystem.Classes
{
    /// <summary>
    /// Отдел
    /// </summary>
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
        public ObservableCollection<Department> ListDepartments { get; set; } = new ObservableCollection<Department>();

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
        /// Список сотрудников
        /// </summary>
        public ObservableCollection<Employee> ListEmployees { get; set; } = new ObservableCollection<Employee>();


        /// <summary>
        /// Список типов отделов
        /// </summary>
        public enum TypeDepartments
        {
            Main = 0,
            Subordinate = 1
        };

        public Department(string TagClass,
                          string Id_department,
                          string NameDepartment,
                          ObservableCollection<Department>? ListDepartments,
                          string DepartmentsString,
                          Department? ParentDepartment,
                          string ParentDepartmentString,
                          string TypeDepartment,
                          ObservableCollection<Employee>? ListEmployees)
        {
            this.TagClass = TagClass;

            if(int.TryParse(Id_department, out int id_department))
                this.Id_department = id_department;
            else
                this.Id_department = 0;

            this.NameDepartment = NameDepartment;

            this.ListDepartments = ListDepartments != null ? ListDepartments : this.ListDepartments;
            this.DepartmentsString = DepartmentsString;

            this.ParentDepartment = ParentDepartment;
            this.ParentDepartmentString = ParentDepartmentString;

            if (TypeDepartments.TryParse(TypeDepartment, out TypeDepartments typeDepartment))
                this.TypeDepartment = typeDepartment;

            this.ListEmployees = ListEmployees != null ? ListEmployees : this.ListEmployees;
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

        public Department(string TagClass,
                          int Id_department,
                          string NameDepartment,
                          ObservableCollection<Department>? ListDepartments,
                          Department? ParentDepartment,
                          TypeDepartments TypeDepartment,
                          ObservableCollection<Employee>? ListEmployees)
        {
            this.TagClass = TagClass;
            this.Id_department = Id_department;
            this.NameDepartment = NameDepartment;

            this.ListDepartments = ListDepartments != null ? ListDepartments : this.ListDepartments;

            if(ListDepartments != null)
                this.DepartmentsString = string.Join(",", ListDepartments.Select(x => x.Id_department));

            this.ParentDepartment = ParentDepartment;
            if (ParentDepartment != null)
                this.ParentDepartmentString = ParentDepartment.Id_department.ToString();

            this.TypeDepartment = TypeDepartment;

            this.ListEmployees = ListEmployees != null ? ListEmployees : this.ListEmployees;
        }
    }
}
