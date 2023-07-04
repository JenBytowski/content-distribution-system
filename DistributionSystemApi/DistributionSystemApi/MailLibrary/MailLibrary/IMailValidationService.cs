

namespace MailLibrary
{
    public interface IMailValidationService 
    {
        void ValidateMailAndThrowError(MailModel mail);
    }
}
