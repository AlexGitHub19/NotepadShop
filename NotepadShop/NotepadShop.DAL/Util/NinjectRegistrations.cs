using Ninject.Modules;
using NotepadShop.DAL.Interfaces;
using NotepadShop.DAL.Repositories;

namespace NotepadShop.DAL.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<IdentityUnitOfWork>();
        }
    }
}
