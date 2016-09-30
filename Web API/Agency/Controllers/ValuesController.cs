using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Agency.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            string username = this.RequestContext.Principal.Identity.Name.Split('@')[0];
            return string.Concat(username.First().ToString().ToUpper(), string.Join("", username.Skip(1)).ToLower());
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
