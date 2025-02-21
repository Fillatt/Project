namespace DataBase
{
    public interface IValidationService
    {
        ValidationService.ValidationResult GetValidation(Account account);
    }
}