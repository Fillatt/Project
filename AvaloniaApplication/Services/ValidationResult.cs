namespace AvaloniaApplication.Services
{
    public class ValidationResult
    {
        public string LoginError { get; set; }

        public string PasswordError { get; set; }

        public bool IsSuccess { get; set; }
    }
}
