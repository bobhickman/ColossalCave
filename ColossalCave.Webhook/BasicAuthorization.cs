using System;
using System.Text;
using System.Threading.Tasks;
using ColossalCave.Webhook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ColossalCave.Webhook
{
    public class BasicAuthorization
    {
        private readonly RequestDelegate _next;
        private readonly BasicAuthorizationModel _authModel;

        public BasicAuthorization(RequestDelegate next,
            IOptions<BasicAuthorizationModel> basicAuthorizationAccessor)
        {
            _next = next;
            _authModel = basicAuthorizationAccessor.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!_authModel.IsEnabled)
            {
                await _next.Invoke(context);
                return;
            }

            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                //Extract credentials
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                int seperatorIndex = usernamePassword.IndexOf(':');

                var username = usernamePassword.Substring(0, seperatorIndex);
                var password = usernamePassword.Substring(seperatorIndex + 1);

                if (username == _authModel.Username && password == _authModel.Password)
                {
                    await _next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 401; //Unauthorized
                }
            }
            else
            {
                // no authorization header
                context.Response.StatusCode = 401; //Unauthorized
            }
        }
    }
}
