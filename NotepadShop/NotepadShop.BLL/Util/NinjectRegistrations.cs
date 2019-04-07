using Ninject.Modules;
using NotepadShop.BLL.Interfaces;
using NotepadShop.BLL.Services;

namespace NotepadShop.BLL.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserService>().To<UserService>();
        }
    }
}
