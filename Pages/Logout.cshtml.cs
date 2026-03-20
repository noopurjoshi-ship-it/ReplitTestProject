using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReplitTestProject.Library;

namespace ReplitTestProject.Pages
{
    public class LogoutModel : BasePage
    {
        public IActionResult OnGet()
        {
            CurrentUser.Logout();
            return RedirectToPage("/Login");
        }
    }
}
