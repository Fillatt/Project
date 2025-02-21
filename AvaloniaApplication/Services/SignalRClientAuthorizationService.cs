using ConsoleApp;
using Microsoft.AspNetCore.SignalR.Client;
using System.Reactive.Subjects;
using System.Threading.Tasks;
namespace AvaloniaApplication.Services;

public class SignalRClientAuthorizationService
{
    #region Private Fields
    private HubConnection _authorizationHubConnection;

    private AuthorizationResult _authorizationResult;

    private ValidationResult _validationResult;
    #endregion

    public BehaviorSubject<bool> IsConnectedSubject { get; set; } = new BehaviorSubject<bool>(false);

    #region Constructors
    public SignalRClientAuthorizationService(ConfigurationService configurationService)
    {
        string url = $"{configurationService.GetSignalRAppUrl()}/authorization";
        _authorizationHubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .Build();

        _authorizationHubConnection.On<string, bool>("ReceiveAuthorizationResult", ReceiveAuthorizationResult);
        _authorizationHubConnection.On<string, string, bool>("ReceiveValidationResult", ReceiveValidationResult);
        _authorizationHubConnection.Closed += (ex) => OnConnectionIsLostAsync();

        StartConnectionAsync();
    }
    #endregion

    #region Public Methods
    public void ReceiveAuthorizationResult(string message, bool isSuccess)
    {
        _authorizationResult = new AuthorizationResult()
        {
            Message = message,
            IsSuccess = isSuccess
        };
    }

    public void ReceiveValidationResult(string loginError, string passwordError, bool isSuccess)
    {
        _validationResult = new ValidationResult()
        {
            LoginError = loginError,
            PasswordError = passwordError,
            IsSuccess = isSuccess
        };
    }

    public async Task StartLoginAsync(string login, string password)
    {
        await _authorizationHubConnection.InvokeAsync("LoginAsync", login, password);
    }

    public AuthorizationResult GetAuthorizationResult()
    {
        return _authorizationResult;
    }

    public async Task StartRegisterAsync(string login, string password)
    {
        await _authorizationHubConnection.InvokeAsync("RegisterAsync", login, password);
    }

    public async Task StartValidationAsync(string login, string password)
    {
        await _authorizationHubConnection.InvokeAsync("GetValidationAsync", login, password);
    }

    public ValidationResult GetValidationResult()
    {
        return _validationResult;
    }
    #endregion

    private async Task StartConnectionAsync()
    {
        try
        {
            await _authorizationHubConnection.StartAsync();
            IsConnectedSubject.OnNext(true);
        }
        catch
        {
            IsConnectedSubject.OnNext(false);
        }
    }

    private Task OnConnectionIsLostAsync()
    {
        try
        {
            IsConnectedSubject.OnNext(false);
            return Task.CompletedTask;
        }
        catch
        {
            IsConnectedSubject.OnNext(false);
            return Task.CompletedTask;
        }
    }
}
