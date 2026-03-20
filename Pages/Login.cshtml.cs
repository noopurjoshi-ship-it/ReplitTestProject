using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReplitTestProject.Library;

namespace ReplitTestProject.Pages
{
    public class LoginModel : BasePage
    {
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
        }
        [BindProperty]
        public string EmployeeID { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string LoginError { get; set; }
        public string PasswordError { get; set; }

        public IActionResult OnGet()
        {
            if (CurrentUser.IsLoggedIn &&
                !(Convert.ToBoolean(CurrentUser.IsCustomer) && Convert.ToBoolean(CurrentUser.IsDistributor)))
            {
                return RedirectToPage("/Dashboard");
            }

            if (CurrentUser.IsLoggedIn &&
                (Convert.ToBoolean(CurrentUser.IsCustomer) || Convert.ToBoolean(CurrentUser.IsDistributor) ||
                 Convert.ToBoolean(CurrentUser.IsDesigner) || Convert.ToBoolean(CurrentUser.IsAgent)))
            {
                return RedirectToPage("/Dashboard");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(EmployeeID) || string.IsNullOrEmpty(Password))
            {
                LoginError = "Please enter your username and password.";
                return Page();
            }

            try
            {
                CurrentUser.Login(EmployeeID.Trim(), Password.Trim());
            }
            catch (Exception ex)
            {
                PasswordError = ex.Message;
                return Page();
            }

            string target = Request.Query["target"];

            if (string.IsNullOrEmpty(target))
            {
                if (CurrentUser.IsCustomer || CurrentUser.IsDistributor ||
                    CurrentUser.IsAgent || CurrentUser.IsDesigner)
                    return RedirectToPage("/Dashboard");
                else
                    return RedirectToPage("/Dashboard");
            }

            return Redirect(target);
        }
    }
}
