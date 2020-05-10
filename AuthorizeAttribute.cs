using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InventoryService
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute() : base(typeof(UnauthorizedFilter))
        {
            //Empty constructor
        }

    }
    public class UnauthorizedFilter : IAuthorizationFilter
    {
        //
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //
            Microsoft.Extensions.Primitives.StringValues authTokens;
            bool IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
           // context.HttpContext.Request.Headers.TryGetValue("authToken", out1 authTokens);
            var _token = context.HttpContext.Request.Headers["Authorization"].Count() !=0 ? context.HttpContext.Request.Headers["Authorization"][0]:"";
            if (!IsAuthenticated)
            {
                //if (context.HttpContext.Request.IsAjaxRequest())
                //{
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized; //Set HTTP 403 - JRozario
                context.HttpContext.Response.Headers.Add("authToken", _token);
                context.HttpContext.Response.Headers.Add("AuthStatus", "NotAuthorized");

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";
                context.Result = new JsonResult("NotAuthorized")
                {
                    Value = new
                    {
                        Status = "Error",
                        Message = "Invalid Token"
                    },
                };
            }
            else
            {
                context.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");
                context.HttpContext.Response.Headers.Add("authToken", _token);
                context.HttpContext.Response.Headers.Add("storeAccessiblity", "Authorized");

                return;
            }
        }

        //public void OnAuthorization(AuthorizationFilterContext filterContext)
        //{
        //    if (filterContext != null)
        //    {
        //        Microsoft.Extensions.Primitives.StringValues authTokens;
        //        filterContext.HttpContext.Request.Headers.TryGetValue("authToken", out authTokens);

        //        var _token = authTokens.FirstOrDefault();

        //        if (_token != null)
        //        {
        //            string authToken = _token;
        //            if (authToken != null)
        //            {
        //                if (IsValidToken(authToken))
        //                {
        //                    filterContext.HttpContext.Response.Headers.Add("authToken", authToken);
        //                    filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");

        //                    filterContext.HttpContext.Response.Headers.Add("storeAccessiblity", "Authorized");

        //                    return;
        //                }
        //                else
        //                {
        //                    filterContext.HttpContext.Response.Headers.Add("authToken", authToken);
        //                    filterContext.HttpContext.Response.Headers.Add("AuthStatus", "NotAuthorized");

        //                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        //                    filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";
        //                    filterContext.Result = new JsonResult("NotAuthorized")
        //                    {
        //                        Value = new
        //                        {
        //                            Status = "Error",
        //                            Message = "Invalid Token"
        //                        },
        //                    };
        //                }

        //            }

        //        }
        //        else
        //        {
        //            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
        //            filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide authToken";
        //            filterContext.Result = new JsonResult("Please Provide authToken")
        //            {
        //                Value = new
        //                {
        //                    Status = "Error",
        //                    Message = "Please Provide authToken"
        //                },
        //            };
        //        }
        //    }
        //}

        public bool IsValidToken(string authToken)
        {
            //validate Token here  
            return true;
        }


    }

   
}
