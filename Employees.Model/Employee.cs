using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using IDKEY = System.Int64;

namespace Employees.Model
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public IDKEY Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Employee Chief { get; set; }
        public IDKEY? ChiefId { get; set; }
        public List<Employee> Subordinates { get; set; } = new List<Employee>();
        public virtual string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
