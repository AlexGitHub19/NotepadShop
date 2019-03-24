using System.Web;
using System.Web.Mvc;

namespace NotepadShop.Filters
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string localizationLanguage = null;

            HttpCookie languageCookie = filterContext.HttpContext.Request.Cookies["ns-language"];
            if (languageCookie != null)
            {
                string languageFromCookie = languageCookie.Value;
                localizationLanguage = languageFromCookie;
            }
            else if (filterContext.HttpContext.Request.UserLanguages.Length > 0)
            {
                string[] userLanguages = filterContext.HttpContext.Request.UserLanguages;
                foreach (string language in userLanguages)
                {
                    string firstTwoLeters = language.Substring(0, 2);
                    switch (firstTwoLeters)
                    {
                        case "ru":
                            localizationLanguage = "ru";
                            break;
                        case "uk":
                            localizationLanguage = "uk";
                            break;
                        case "en":
                            localizationLanguage = "en";
                            break;
                        default:
                            localizationLanguage = null;
                            break;
                    }

                    if (localizationLanguage != null)
                    {
                        break;
                    }
                }
            }

            filterContext.Controller.ViewBag.ResourceManager = Localization.Localization.GetResourceManager(localizationLanguage);
            filterContext.Controller.ViewBag.Language = localizationLanguage;
        }
    }
}