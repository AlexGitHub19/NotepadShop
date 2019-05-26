using System.Web;
using System.Web.Mvc;

namespace NotepadShop.Filters
{
    public class UserNameAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Controller.ViewBag.Email = filterContext.HttpContext.User.Identity.Name;
            }
        }
    }
}