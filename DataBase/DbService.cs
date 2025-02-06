using Microsoft.EntityFrameworkCore;

namespace DataBase;

public class DbService
{
    #region Private Fields
    private Context _dbContext;
    #endregion

    #region Constructors
    public DbService(Context dbContext)
    {
        _dbContext = dbContext;
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
        await _dbContext.Accounts.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Добавление 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task Add(ApiRequestResult requestResult)
    {
        await _dbContext.ApiRequests.AddAsync(requestResult);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Верификация аккаунта
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public async Task<bool> VerifyAccount(Account account)
    {
        var accountDb = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Login == account.Login);
        return accountDb != null && PasswordHasher.VerifyPasswordHash(account.Password, accountDb.PasswordHash, accountDb.PasswordSalt);
    }

    /// <summary>
    /// Проверка наличия аккаунта в базе данных
    /// </summary>
    /// <param name="db"></param>
    /// <param name="login"></param>
    /// <returns></returns>
    public async Task<bool> IsAccountExistAsync(Account account) =>
        await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Login == account.Login) != null;
    #endregion
}
