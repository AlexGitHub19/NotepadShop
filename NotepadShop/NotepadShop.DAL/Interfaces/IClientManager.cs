using System;

namespace NotepadShop.DAL.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create();
    }
}
