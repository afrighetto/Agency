using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Agency.Filters
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                if (actionContext.Request.Method == HttpMethod.Get || actionContext.Request.Method == HttpMethod.Head)
                {
                    actionContext.Response.StatusCode = HttpStatusCode.Found;
                }
                else
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                    {
                        ReasonPhrase = "HTTPS is required."
                    };
                }
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }

    }
}