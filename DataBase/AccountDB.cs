using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase;

/// <summary>
/// Класс, представляющий аккаунт, хранящийся в базе данных
/// </summary>
public class AccountDB
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Логин
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Хешированный пароль
    /// </summary>
    public byte[] PasswordHash { get; set; }

    /// <summary>
    /// Соль, используемая в процессе хеширования
    /// </summary>
    public byte[] PasswordSalt { get; set; }
}
