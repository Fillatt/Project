using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DataBase;

public class Authorization
{
    #region Records
    /// <summary>
    /// Запись для получения результата авторизации
    /// </summary>
    public record AuthorizationResult
    {       
        public string Message { get; set; }              
      
        public bool IsSuccess { get; set; }
    }
    #endregion

    #region Private Fields
    private DbService _dbService;
    #endregion

    #region Constructors
    public Authorization(DbService dbService)
    {
        _dbService = dbService;
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
        if (!await _dbService.IsAccountExistAsync(account))
        {
            await _dbService.Add(account);
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

        if (!(await _dbService.VerifyAccount(account))) result = GetResult("Invalid username or password", false);
        else result = GetResult($"Login is success", true);

        Log.Debug("Authorization.LoginAsync: Done; Result: {@Result}", result);

        return result;
    }
    #endregion

    #region Private Methods
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
            IsSuccess = isSuccess
        };

        Log.Debug("Authorization.GetResult: Done; Result: {@Result}", result);

        return result;
    }
    #endregion
}
