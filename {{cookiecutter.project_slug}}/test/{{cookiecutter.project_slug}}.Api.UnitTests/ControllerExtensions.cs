using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace {{cookiecutter.project_slug}}.Api.Tests
{
    public static class ControllerExtensions
    {
        public static T WithUser<T>(this T c) where
            T: ControllerBase
        {
            var user = new ClaimsPrincipal();
            user.AddIdentity(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test")
            }));

            c.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            return c;
        }
    }
}
