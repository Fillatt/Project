using DataBase;
using Microsoft.AspNetCore.SignalR;

namespace SignalRApp;

public class JokesDataBaseHub : Hub
{
    #region Private Fields
    private DbService _dbService;
    #endregion

    #region Constructors
    public JokesDataBaseHub(DbService dbService)
    {
        _dbService = dbService;
    }
    #endregion

    #region Public Methods
    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;

        Console.WriteLine($"(JokesDataBaseHub) Client connected: {connectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        Console.WriteLine($"(JokesDataBaseHub) Client disconnected: {connectionId}");

        await base.OnDisconnectedAsync(exception);
    }

    public async Task AddInDataBaseAsync(Joke joke)
    {
        await _dbService.AddAsync(new ApiRequestResult
        {
            JokeId = joke.Id,
            Type = joke.Type,
            Punchline = joke.Punchline,
            Setup = joke.Setup,
        });
    }

    public async Task AddListInDataBaseAsync(List<Joke> jokes)
    {
        foreach (var joke in jokes) await _dbService.AddAsync(new ApiRequestResult
        {
            JokeId = joke.Id,
            Type = joke.Type,
            Punchline = joke.Punchline,
            Setup = joke.Setup,
        });
    }
    #endregion
}
