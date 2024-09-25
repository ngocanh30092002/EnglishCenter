namespace EnglishCenter.Presentation.Helpers
{
    public static class CookieHelper
    {
        public static void AddCookie(HttpContext httpContext, string name, string value, CookieOptions options)
        {
            if (httpContext.Request.Cookies.ContainsKey(name))
            {
                httpContext.Response.Cookies.Delete(name);
            }

            httpContext.Response.Cookies.Append(name, value, options);
        }
    }
}
