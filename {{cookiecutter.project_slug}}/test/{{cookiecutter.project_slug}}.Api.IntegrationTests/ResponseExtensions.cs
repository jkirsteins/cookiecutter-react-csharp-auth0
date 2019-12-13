namespace {{cookiecutter.project_slug}}.Api.IntegrationTests
{
    using System;
    using System.Net.Http;
    using Microsoft.AspNetCore.Http;

    public static class ResponseExtensions
    {
        public static string GetCookieSettings(this HttpResponseMessage response, string cookieName)
        {
            if (response.Headers.Contains("Set-Cookie"))
            {
                foreach (var header in response.Headers.GetValues("Set-Cookie"))
                {
                    if (header.StartsWith($"{cookieName}=", StringComparison.Ordinal))
                    {
                        var p2 = header.IndexOf(';');
                        return header.Substring(p2 + 1);
                    }
                }
            }
            return null;
        }
    }
}
