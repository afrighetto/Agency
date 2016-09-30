using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;

namespace Agency.Models
{
    public class EmployeeEntity : DataContext, IEmployeeEntity
    {
        public EmployeeEntity() : base(Properties.Settings.Default.localDBConnection) { }
        public IEnumerable<Employee> GetAll()
        {
            return this.GetTable<Employee>();
        }
        public Employee GetEmployee(int id)
        {
            return this.GetTable<Employee>().SingleOrDefault(item => item.EmployeeID == id);
        }
        public Employee Add(Employee e)
        {
            if (this.GetEmployee(e.EmployeeID) == null)
            {
                this.GetTable<Employee>().InsertOnSubmit(e);
                this.SubmitChanges();
                return e;
            }
            return null;
        }
        public bool Update(Employee e)
        {
            Employee item = this.GetEmployee(e.EmployeeID);
            if (e.EmployeeID == item.EmployeeID &&
                e.LastName == item.LastName &&
                e.FirstName == item.FirstName &&
                e.BirthDate == item.BirthDate &&
                e.Region == item.Region && e.City == item.City && e.PostalCode == item.PostalCode) return false;

            item.EmployeeID = e.EmployeeID;
            item.LastName = e.LastName;
            item.FirstName = e.FirstName;
            item.BirthDate = e.BirthDate;
            item.City = e.City;
            item.Region = e.Region;
            item.PostalCode = e.PostalCode;
            this.SubmitChanges();
            return true;
        }
        public void Remove(Employee e)
        {
            try
            {
                this.GetTable<Employee>().DeleteOnSubmit(e);
                this.SubmitChanges();
            }
            catch (Exception) { }
            return;
        }
    }
}