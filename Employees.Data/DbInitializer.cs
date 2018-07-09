using Employees.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employees.Data
{
    public class DbInitializer
    {
        private EmployeesContext _context;

        public DbInitializer(EmployeesContext context)
        {
            _context = context;
        }

        public void CreateSuperChief()
        {
            Employee e = new Employee();
            e.FirstName = "Супер";
            e.LastName = "Шеф";
            _context.Add(e);
            _context.SaveChanges();
        }

    }
}
