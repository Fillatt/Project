
namespace DataBase
{
    public interface IAuthorizationService
    {
        Task<AuthorizationService.AuthorizationResult> LoginAsync(Account account);
        Task<AuthorizationService.AuthorizationResult> RegisterAsync(Account account);
    }
}