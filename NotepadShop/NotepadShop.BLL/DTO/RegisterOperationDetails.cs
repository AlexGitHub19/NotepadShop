namespace NotepadShop.BLL.DTO
{
    public class RegisterOperationDetails
    {
        public RegisterOperationDetails(bool succedeed, string errorMessage)
        {
            Succedeed = succedeed;
            ErrorMessage = errorMessage;
        }
        public bool Succedeed { get; private set; }
        public string ErrorMessage { get; private set; }
    }
}
