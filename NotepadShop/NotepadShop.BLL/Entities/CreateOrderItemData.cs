using NotepadShop.BLL.Interfaces;

namespace NotepadShop.BLL.Entities
{
    public class CreateOrderItemData: ICreateOrderItemData
    {
        public string Code { get; }
        public int Count { get; }

        public CreateOrderItemData(string code, int count)
        {
            Code = code;
            Count = count;
        }
    }
}
