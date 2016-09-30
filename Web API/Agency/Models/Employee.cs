using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;

namespace Agency.Models
{
    [Table(Name = "dbo.Employees")]
    public class Employee
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int EmployeeID { get; set; }
        [Column]
        public string LastName { get; set; }
        [Column]
        public string FirstName { get; set; }
        [Column]
        public DateTime BirthDate { get; set; }
        [Column]
        public string City { get; set; }
        [Column]
        public string Region { get; set; }
        [Column]
        public string PostalCode { get; set; }
    }
}