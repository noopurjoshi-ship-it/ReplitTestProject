using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ReplitTestProject.Library
{
    public class BasePage : PageModel
    {
        private CurrentUser mCurrentUser = null;
        public CurrentUser CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                {
                    mCurrentUser = new CurrentUser();
                }
                return mCurrentUser;
            }
        }
    }
}
