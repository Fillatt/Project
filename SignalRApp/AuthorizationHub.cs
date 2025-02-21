using DataBase;
using Microsoft.AspNetCore.SignalR;
namespace SignalRApp;

public class AuthorizationHub : Hub
{
    #region Private Fields
    private IAuthorizationService _authorizationService;
    private IValidationService _validationService;
    #endregion

    #region Constructors
    public AuthorizationHub(IAuthorizationService authorizationService, IValidationService validationService)
    {
        _authorizationService = authorizationService;
        _validationService = validationService;
    }
    #endregion

    #region Public Methods
    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;

        Console.WriteLine($"(AuthorizationHub) Client connected: {connectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        Console.WriteLine($"(AuthorizationHub) Client disconnected: {connectionId}");

        await base.OnDisconnectedAsync(exception);
    }

    public async Task LoginAsync(string login, string password)
    {
        var result = await _authorizationService.LoginAsync(new Account { Login = login, Password = password });
        await Clients.Caller.SendAsync("ReceiveAuthorizationResult", result.Message, result.IsSuccess);
    }

    public async Task RegisterAsync(string login, string password)
    {
        var result = await _authorizationService.RegisterAsync(new Account { Login = login, Password = password });
        await Clients.Caller.SendAsync("ReceiveAuthorizationResult", result.Message, result.IsSuccess);
    }

    public async Task GetValidationAsync(string login, string password)
    {
        var result = _validationService.GetValidation(new Account { Login = login, Password = password });
        await Clients.Caller.SendAsync("ReceiveValidationResult", result.LoginError, result.PasswordError, result.IsSuccess);
    }
    #endregion
}
