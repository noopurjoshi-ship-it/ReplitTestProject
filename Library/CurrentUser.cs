using ReplitTestProject.Models;
using System;

namespace ReplitTestProject.Library
{
    public class CurrentUser : UserInfo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpContext _httpContext;

        public bool IsLoggedIn { get { return !string.IsNullOrEmpty(GUID); } }

        public CurrentUser()
        {
            // Get HttpContext without passing it as parameter
            var accessor = new HttpContextAccessor();
            _httpContext = accessor.HttpContext;

            var cookie = _httpContext?.Request.Cookies["UserGUID"];
            if (!string.IsNullOrEmpty(cookie))
            {
                try
                {
                    Load(cookie);
                }
                catch
                {
                    Logout();
                }
            }
        }

        public new bool Login(string EmployeeID, string Password)
        {
            try
            {
                base.Login(EmployeeID, Password);

                // Set cookie - expires in 7 days (adjust as needed)
                _httpContext.Response.Cookies.Append("UserGUID", GUID, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true,   // prevents JS access - more secure
                    Secure = true,     // HTTPS only
                    SameSite = SameSiteMode.Lax
                });
            }
            catch (Exception ex)
            {
                // Expire the cookie on failed login
                _httpContext.Response.Cookies.Append("UserGUID", "", new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(-1)
                });
                throw ex;
            }
            return true;
        }

        public bool Logout()
        {
            try
            {
                _httpContext.Response.Cookies.Append("UserGUID", "", new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(-1)
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}
