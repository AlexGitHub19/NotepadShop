using System;

namespace NotepadShop.BLL.Interfaces
{
    public interface IItem: IBriefItem
    {
        string Code { get;}
        DateTime AddingTime { get;}
    }
}
