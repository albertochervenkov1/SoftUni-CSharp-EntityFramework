using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext db = new SoftUniContext();
            string result = GetEmployeesByFirstNameStartingWithSa(db);
            Console.WriteLine(result);
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context) { 
            
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees               
                .Where(e=>e.Salary>50000)
                .OrderBy(e=>e.FirstName)
                .Select(e=>new
                {
                    e.FirstName,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development" )
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Department.Name,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Name} - ${e.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
                
        }

        public static string AddNewAddressToEmployee(SoftUniContext context) 
        {StringBuilder sb=new StringBuilder();

            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            
            context.Addresses.Add(newAddress);
            Employee nakovEmployee=context.Employees
                .First(e => e.LastName=="Nakov");

            nakovEmployee.Address = newAddress;

            context.SaveChanges();

            string[] allEmployees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e=>e.Address.AddressText)
                .Take(10)
                .ToArray();

            return String.Join(Environment.NewLine, allEmployees);
        }

        public static string GetEmployeesInPeriod(SoftUniContext context) 
        {
            var employees = context.Employees.Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects.Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        ProjectStartDate = ep.Project.StartDate,
                        ProjectEndDate = ep.Project.EndDate
                    })
                }).Take(10);
            StringBuilder employeeManagerResult = new StringBuilder();

            foreach (var employee in employees)
            {
                employeeManagerResult.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var project in employee.Projects)
                {
                    var startDate = project.ProjectStartDate.ToString("M/d/yyyy h:mm:ss tt");
                    var endDate = project.ProjectEndDate.HasValue ? project.ProjectEndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished";

                    employeeManagerResult.AppendLine($"--{project.ProjectName} - {startDate} - {endDate}");
                }
            }
            return employeeManagerResult.ToString().TrimEnd();


        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb=new StringBuilder();
            var addresses=context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .Take(10)
                .Select(a => new
                {
                    Text = a.AddressText,
                    Town = a.Town.Name,
                    EmployeesCount = a.Employees.Count
                })
                .ToList();
            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.Text}, {a.Town} - {a.EmployeesCount} employees");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context) 
        {
            var employee = context.Employees.Where(e => e.EmployeeId == 147).FirstOrDefault();

            var projects=context.EmployeesProjects
                .Where(p=>p.EmployeeId==147)
                .Select(p => new 
                {
                    ProjectName=p.Project.Name
                })
                .OrderBy(p=>p.ProjectName)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            foreach (var p in projects)
            {
                sb.AppendLine($"{p.ProjectName}");
            }
            return sb.ToString().TrimEnd();
        }
        
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var deparments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .Select(d => new
                {
                    d.Name,
                    managerFirstName = d.Manager.FirstName,
                    employees = d.Employees
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle
                        }).OrderBy(e=>e.FirstName).ThenBy(e=>e.LastName).ToList()
                })
                .ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var depart in deparments)
            {
                sb.AppendLine($"{depart.Name} {depart.managerFirstName}");
                foreach (var emp in depart.employees)
                {
                    sb.AppendLine($"{emp.FirstName} {emp.LastName} {emp.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .OrderBy(p=>p.Name)
                .ToList();
            StringBuilder sb=new StringBuilder();
            
            foreach (var project in projects)
            {
                var startDate = project.StartDate.ToString("M/d/yyyy h:mm:ss tt");
                sb.AppendLine($"{project.Name}");
                sb.AppendLine($"{project.Description}");
                sb.AppendLine($"{startDate}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" || e.Department.Name == "Marketing"||e.Department.Name== "Information Services")
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Salary = e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();
            StringBuilder sb=new StringBuilder();

            foreach (var employee in employees)
            {
                var sal=employee.Salary*(decimal)1.12;
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${sal:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            StringBuilder sb= new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine(
                    $"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var townSeattle =  context
                .Towns
                .First(t => t.Name == "Seattle");

            var addressesToDelete =
                context
                    .Addresses
                    .Where(a => a.TownId == townSeattle.TownId);

            int addressesCount = addressesToDelete.Count();

            var employeesOnDeletedAddresses =
                context
                    .Employees
                    .Where(e => addressesToDelete.Any(a => a.AddressId == e.AddressId));

            foreach (var employee in employeesOnDeletedAddresses)
            {
                employee.AddressId = null;
            }

            foreach (var address in addressesToDelete)
            {
                context.Addresses.Remove(address);
            }

            context.Remove(townSeattle);

            context.SaveChanges();

            return $"{addressesCount} addresses in Seattle were deleted";
        }


    }
}
