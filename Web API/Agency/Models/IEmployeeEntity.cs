using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Models
{
    interface IEmployeeEntity
    {
        IEnumerable<Employee> GetAll();
        Employee GetEmployee(int id);
        Employee Add(Employee e);
        bool Update(Employee e);
        void Remove(Employee e);
    }
}
