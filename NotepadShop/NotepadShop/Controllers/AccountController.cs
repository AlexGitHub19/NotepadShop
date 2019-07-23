using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NotepadShop.Assemblers;
using NotepadShop.BLL.DTO;
using NotepadShop.BLL.Interfaces;
using NotepadShop.Models.AccountModels;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NotepadShop.Controllers
{
    [RoutePrefix("account/api")]
    public class AccountController : Controller
    {
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        //public ActionResult Login()
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("login")]
        public async Task<JsonResult> Login(LoginModel model)
        {
            LoginResult result = new LoginResult();
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                result.ErrorMessage = "Not full input data";
            }
            else
            {
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = UserService.Authenticate(userDto);
                if (claim == null)
                {
                    result.Email = null;
                    result.ErrorMessage = ViewBag.ResourceManager.GetString("incorrectLoginOrPassword");
                }
                else
                {
                    result.Email = model.Email;
                    LanguageType? userLanguage = UserService.GetUserLanguage(model.Email);
                    if (userLanguage != null) {
                        result.Language = WebAssembler.AssembleLanguage(userLanguage.Value);
                        ViewBag.Language = result.Language;
                    }

                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                }
            }
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Route("logout")]
        public void Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                AuthenticationManager.SignOut();
            }
        }

        //public ActionResult Register()
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("register")]
        public async Task<JsonResult> Register(RegisterModel model)
        {
            RegisterResult result = new RegisterResult();
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                result.ErrorMessage = "Not full input data";
            }
            else
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Role = "user"
                };
                RegisterOperationDetails operationDetails = UserService.Create(userDto);
                if (operationDetails.Succedeed)
                {
                    result.Email = model.Email;
                    ClaimsIdentity claim = UserService.Authenticate(userDto);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                }
                else
                {
                    result.ErrorMessage = getRegistrationErrorMessage(operationDetails.ErrorType);
                }
            }

            return Json(result);
        }

        private string getRegistrationErrorMessage(ErrorType errorType)
        {
            string result = "error";
            switch (errorType)
            {
                case ErrorType.UserWithSuchEmailAlreadyExists:
                    result = ViewBag.ResourceManager.GetString("userWithSuchEmailAlreadyExists");
                    break;
                case ErrorType.IdentityErrorWhileCreating:
                    result = ViewBag.Localization.ResourceManager.GetString("registrationIdentityError");
                    break;
                case ErrorType.None:
                default:
                    result = null;
                    break;
            }

            return result;
        }

        [HttpGet]
        [Route("set-initial-data")]
        public void SetInitialData()
        {
            if (!UserService.IsInitialDataSet()) {
                UserService.SetInitialData();
            }
            
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("change-password")]
        public JsonResult ChangePassword(string oldPassword, string newPassword)
        {
            bool isSuceeded = UserService.ChangePassword(User.Identity.GetUserId(), oldPassword, newPassword);
            return Json(isSuceeded);
        }

    }
}