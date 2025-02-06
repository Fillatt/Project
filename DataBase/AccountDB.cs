namespace DataBase;

/// <summary>
/// Класс, представляющий аккаунт, хранящийся в базе данных
/// </summary>
public class AccountDB
{
    public int Id { get; set; }

    public string Login { get; set; }

    public byte[] PasswordHash { get; set; }

    public byte[] PasswordSalt { get; set; }
}
