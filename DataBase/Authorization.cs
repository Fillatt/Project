using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DataBase;

public class Authorization
{
    #region Structs
    /// <summary>
    /// Стуктура для получения результата авторизации
    /// </summary>
    public struct AuthorizationResult
    {       
        public string Message { get; set; }
      
        public string MessageColor { get; set; }
      
        public bool IsSuccess { get; set; }
    }
    #endregion

    #region Private Fields
    private Context _dbContext;
    #endregion

    #region Constructors
    public Authorization(Context dbContext)
    {
        _dbContext = dbContext;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Регистрация
    /// </summary>
    /// <param name="account"></param>
    /// <param name="db"></param>
    /// <returns></returns>
    public async Task<AuthorizationResult> RegisterAsync(Account account)
    {
        Log.Debug("Authorization.RegisterAsync: Start");

        var result = new AuthorizationResult();
        if (!await IsAccountExistAsync(_dbContext, account.Login))
        {
            byte[] passwordHash, passwordSalt;
            PasswordHasher.CreatePasswordHash(account.Password, out passwordHash, out passwordSalt);
            AccountDB user = new AccountDB
            {
                Login = account.Login,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            await _dbContext.Accounts.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            result = GetResult("Register is success", true);
        }
        else result = GetResult("Account is exist", false);

        Log.Debug("Authorization.RegisterAsync: Done; Result: {@Result}", result);

        return result;
    }

    /// <summary>
    /// Вход в систему
    /// </summary>
    /// <param name="account"></param>
    /// <param name="db"></param>
    /// <returns></returns>
    public async Task<AuthorizationResult> LoginAsync(Account account)
    {
        Log.Debug("Authorization.LoginAsync: Start");

        var result = new AuthorizationResult();
        var accountDb = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Login == account.Login);

        if (accountDb == null || !PasswordHasher.VerifyPasswordHash(account.Password, accountDb.PasswordHash, accountDb.PasswordSalt))
            result = GetResult("Invalid username or password", false);
        else result = GetResult($"Login is success", true);

        Log.Debug("Authorization.LoginAsync: Done; Result: {@Result}", result);

        return result;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Проверка наличия аккаунта в базе данных
    /// </summary>
    /// <param name="db"></param>
    /// <param name="login"></param>
    /// <returns></returns>
    private async Task<bool> IsAccountExistAsync(Context db, string login) =>
        await db.Accounts.FirstOrDefaultAsync(x => x.Login == login) != null;

    /// <summary>
    /// Получение AuthorizationResult c указанным сообщением и флагом успеха авторизации
    /// </summary>
    /// <param name="message"></param>
    /// <param name="isSuccess"></param>
    /// <returns></returns>
    private AuthorizationResult GetResult(string message, bool isSuccess)
    {
        Log.Debug("Authorization.GetResult: Start");

        var result = new AuthorizationResult
        {
            Message = message,
            MessageColor = isSuccess ? "Green" : "Red",
            IsSuccess = isSuccess
        };

        Log.Debug("Authorization.GetResult: Done; Result: {@Result}", result);

        return result;
    }
    #endregion
}
