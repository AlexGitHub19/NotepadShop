using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NotepadShop.BLL.DTO;
using NotepadShop.BLL.Interfaces;
using NotepadShop.Models.AccountModels;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NotepadShop.Controllers
{
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
                    result.ErrorMessage = "Неверный логин или пароль.";
                }
                else
                {
                    result.Email = model.Email;
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
        public async Task<JsonResult> Register(RegisterModel model)
        {
            //SetInitialData();
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
                    result.ErrorMessage = operationDetails.ErrorMessage;
                }
            }

            return Json(result);
        }

        private void SetInitialData()
        {
            UserService.SetInitialData();
        }
    }
}