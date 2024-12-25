using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DataBase;

/// <summary>
/// Статический класс, осуществляющий авторизацию
/// </summary>
public static class Authorization
{
    #region Structs
    /// <summary>
    /// Стуктура для получения результата авторизации
    /// </summary>
    public struct AuthorizationResult
    {

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Цвет сообщения
        /// </summary>
        public string MessageColor { get; set; }

        /// <summary>
        /// Флаг успеха авторизации
        /// </summary>
        public bool IsSuccess { get; set; }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Регистрация
    /// </summary>
    /// <param name="account"></param>
    /// <param name="db"></param>
    /// <returns></returns>
    public static async Task<AuthorizationResult> RegisterAsync(Account account, Context db)
    {
        Log.Debug("Authorization.RegisterAsync: Start");

        var result = new AuthorizationResult();
        if (!await IsAccountExistAsync(db, account.Login))
        {
            byte[] passwordHash, passwordSalt;
            PasswordHasher.CreatePasswordHash(account.Password, out passwordHash, out passwordSalt);
            AccountDB user = new AccountDB
            {
                Login = account.Login,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            await db.Accounts.AddAsync(user);
            await db.SaveChangesAsync();

            result = GetResult("Register is success, please login!", true);
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
    public static async Task<AuthorizationResult> LoginAsync(Account account, Context db)
    {
        Log.Debug("Authorization.LoginAsync: Start");

        var result = new AuthorizationResult();
        var accountDb = await db.Accounts.FirstOrDefaultAsync(x => x.Login == account.Login);

        if (accountDb == null || !PasswordHasher.VerifyPasswordHash(account.Password, accountDb.PasswordHash, accountDb.PasswordSalt))
            result = GetResult("Invalid username or password", false);
        else result = GetResult($"Welcome, {account.Login}!", true);

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
    private static async Task<bool> IsAccountExistAsync(Context db, string login) =>
        await db.Accounts.FirstOrDefaultAsync(x => x.Login == login) != null;

    /// <summary>
    /// Получение AuthorizationResult c указанным сообщением и флагом успеха авторизации
    /// </summary>
    /// <param name="message"></param>
    /// <param name="isSuccess"></param>
    /// <returns></returns>
    private static AuthorizationResult GetResult(string message, bool isSuccess)
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
