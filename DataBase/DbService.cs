using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase;

public class DbService
{
    #region Private Fields
    private AccountContext _accountContext;

    private ApiRequestResultContext _apiRequestResultContext;
    #endregion

    #region Constructors
    public DbService(AccountContext accountContext, ApiRequestResultContext apiRequestResultContext)
    {
        _accountContext = accountContext;
        _apiRequestResultContext = apiRequestResultContext;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Добавление аккаунта в БД
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public async Task Add(Account account)
    {
        byte[] passwordHash, passwordSalt;
        PasswordHasher.CreatePasswordHash(account.Password, out passwordHash, out passwordSalt);
        AccountDB user = new AccountDB
        {
            Login = account.Login,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        await _accountContext.Accounts.AddAsync(user);
        await _accountContext.SaveChangesAsync();
    }

    /// <summary>
    /// Добавление 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task Add(ApiRequestResult requestResult)
    {
        await _apiRequestResultContext.ApiRequestsResults.AddAsync(requestResult);
        await _apiRequestResultContext.SaveChangesAsync();
    }

    /// <summary>
    /// Верификация аккаунта
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public async Task<bool> VerifyAccount(Account account)
    {
        var accountDb = await _accountContext.Accounts.FirstOrDefaultAsync(x => x.Login == account.Login);
        return accountDb != null && PasswordHasher.VerifyPasswordHash(account.Password, accountDb.PasswordHash, accountDb.PasswordSalt);
    }

    /// <summary>
    /// Проверка наличия аккаунта в базе данных
    /// </summary>
    /// <param name="db"></param>
    /// <param name="login"></param>
    /// <returns></returns>
    public async Task<bool> IsAccountExistAsync(Account account) =>
        await _accountContext.Accounts.FirstOrDefaultAsync(x => x.Login == account.Login) != null;
    #endregion
}
