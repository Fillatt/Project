using Serilog;
using System.Text;

namespace DataBase;

/// <summary>
/// Статический класс для хеширования пароля
/// </summary>
public static class PasswordHasher
{
    #region Public Methods
    /// <summary>
    /// Хеширование пароля
    /// </summary>
    /// <param name="password"></param>
    /// <param name="passwordHash"></param>
    /// <param name="passwordSalt"></param>
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        Log.Debug("PasswordHasher.CreatePasswordHash: Start");

        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        Log.Debug("PasswordHasher.CreatePasswordHash: Done");
    }

    /// <summary>
    /// Проверка пароля
    /// </summary>
    /// <param name="password"></param>
    /// <param name="storedHash"></param>
    /// <param name="storedSalt"></param>
    /// <returns></returns>
    public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        Log.Debug("PasswordHasher.VerifyPasswordHash: Start");

        using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var result = computedHash.SequenceEqual(storedHash);

            Log.Debug("PasswordHasher.VerifyPasswordHash: Done; Result: {Result}", result);

            return result;
        }
    }
    #endregion
}
