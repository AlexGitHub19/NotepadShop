namespace NotepadShop.BLL.DTO
{
    public class RegisterOperationDetails
    {
        public RegisterOperationDetails(bool succedeed, ErrorType errorType)
        {
            Succedeed = succedeed;
            ErrorType = errorType;
        }
        public bool Succedeed { get; private set; }
        public ErrorType ErrorType { get; private set; }
    }
}
