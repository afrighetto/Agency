using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Agency.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<HttpStatusCodeResult> Register(string email, string password, string passwordConfirm)
        {
            bool login = string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || !string.Equals(password, passwordConfirm);
            if (login)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid registration form.");
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44396/");
                client.DefaultRequestHeaders.Accept.Clear();
                Dictionary<string, string> registerUser = new Dictionary<string, string> {
                    {"Email", email},
                    {"Password", password},
                    {"ConfirmPassword", password}
                };
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Account/Register", registerUser);

                return new HttpStatusCodeResult(response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}
