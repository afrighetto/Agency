using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Agency.Models;
namespace Agency.Controllers
{
    //[Authorize]
    public class EmployeeController : ApiController
    {
        private static readonly IEmployeeEntity entity = new EmployeeEntity();
        //GET Requests
        public IEnumerable<Employee> GetAll()
        {
            return entity.GetAll();
        }
        public IHttpActionResult GetEmployee(int id)
        {
            Employee e = entity.GetEmployee(id);
            if (e == null)
                return NotFound();
            return Ok(e);
        }
        public IEnumerable<Employee> GetEmployeeByCity(string city)
        {
            return entity.GetAll().Where(item => string.Equals(item.City, city, StringComparison.InvariantCultureIgnoreCase));
        }

        //Creating a new Employee resource (POST).
        public IHttpActionResult PostEmployee(Employee e)
        {
            Employee newEmployee = entity.Add(e);
            if (newEmployee == null)
                return Conflict();
            return Created<Employee>(Request.RequestUri + newEmployee.EmployeeID.ToString(), newEmployee);
        }

        //Updating an Employee resource (PUT), employee is read from body.
        public HttpResponseMessage PutEmployee(int id, [FromBody] Employee e)
        {
            if (e == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read employee from body.");
            e.EmployeeID = id;
            if (!entity.Update(e))
                return Request.CreateResponse(HttpStatusCode.NotModified);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //Deleting an Employee resource (DELETE), returning 204 No Content.
        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee e = entity.GetEmployee(id);
            if (e != null)
            {
                entity.Remove(e);
                return Ok();
            }
            return NotFound();
        }
    }
}
