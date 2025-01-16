using Serilog;

namespace DataBase;

public static class Validation
{
    #region Structs
    /// <summary>
    /// Структура для получения результата валидации
    /// </summary>
    public struct ValidationResult
    {      
        public string LoginError { get; set; }
        
        public string PasswordError { get; set; }
       
        public bool IsSuccess { get; set; }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Получение результата валидации
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static ValidationResult GetValidation(Account account)
    {
        Log.Debug("Validation.GetValidation: Start");

        var loginValidation = LoginValidation(account.Login);
        var passwordValidation = PasswordValidation(account.Password);

        var result = new ValidationResult
        {
            PasswordError = passwordValidation.PasswordError,
            LoginError = loginValidation.LoginError,
            IsSuccess = loginValidation.IsSuccess && passwordValidation.IsSuccess
        };

        Log.Debug("Validation.GetValidation: Done; Result: {@Result}", result);

        return result;
    }
    #endregion

    #region Private Methods   

    /// <summary>
    /// Валидация логина
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    private static ValidationResult LoginValidation(string login)
    {
        Log.Debug("Validation.LoginValidation: Start");

        ValidationResult result = new();
        if (IsEmpty(login)) result = LoginIsEmpty();
        else if (IsDigit(login)) result = LoginStartWithNumber();        
        else result.IsSuccess = true;

        Log.Debug("Validation.LoginValidation: Done; Result: {@Result}", result);

        return result;
    }

    /// <summary>
    /// Валидация пароля
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    private static ValidationResult PasswordValidation(string password)
    {
        Log.Debug("Validation.PasswordValidation: Start");

        ValidationResult result = new();
        if (IsEmpty(password)) result = PasswordIsEmpty();
        else if (IsEnoughLength(password)) result = PasswordIsNotEnoughLength();
        else result.IsSuccess = true;

        Log.Debug("Validation.PasswordValidation: Done; Result: {@Result}", result);

        return result;
    }

    private static ValidationResult PasswordIsNotEnoughLength() => GetPasswordError("The password must be longer than 8 characters");

    private static ValidationResult PasswordIsEmpty() => GetPasswordError("Password can not be empty");

    private static ValidationResult LoginIsEmpty() => GetLoginError("Login can not be empty");

    private static ValidationResult LoginStartWithNumber() => GetLoginError("Login can not start with a number");

    private static ValidationResult GetLoginError(string message)
    {
        return new ValidationResult
        {
            LoginError = message,
            IsSuccess = false
        };
    }

    private static ValidationResult GetPasswordError(string message)
    {
        return new ValidationResult
        {
            PasswordError = message,
            IsSuccess = false
        };
    }
    
    private static bool IsEnoughLength(string password) => password.Length < 8;
   
    private static bool IsEmpty(string element) => string.IsNullOrEmpty(element);

    private static bool IsDigit(string element) => char.IsDigit(element[0]);
    #endregion
}
