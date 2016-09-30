using System;
using SQLite;

namespace Agency
{
	[Table("Employee")]
	public class Employee
	{
		[Column("EmployeeID"), PrimaryKey]
		public int EmployeeID { get; set; }
		[Column("LastName"), NotNull]
		public string LastName { get; set; }
		[Column("FirstName"), NotNull]
		public string FirstName { get; set; }
		[Column("Birth")]
		public DateTime BirthDate { get; set; }
		[Column("City")]
		public string City { get; set; }
		[Column("Region")]
		public string Region { get; set; }
		[Column("PostalCode")]
		public string PostalCode { get; set; }

		[Ignore]
		public string FirstLastName { get { return FirstName + " " + LastName; } }
	}
}