using APIClient;
using ConsoleApp;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace AvaloniaApplication.Services;

public class SignalRClientJokesDbService
{
    #region Private Fields
    private HubConnection _jokesDbHubConnection;
    #endregion

    #region Properties
    public BehaviorSubject<bool> IsConnectedSubject { get; set; } = new BehaviorSubject<bool>(false);
    #endregion

    public SignalRClientJokesDbService(ConfigurationService configurationService)
    {
        string url = $"{configurationService.GetSignalRAppUrl()}/jokes_data_base";
        _jokesDbHubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .Build();

        _jokesDbHubConnection.Closed += (ex) => OnConnectionIsLostAsync();

        StartConnectionAsync();
    }

    public async Task AddInDataBaseAsync(Joke joke)
    {
        await _jokesDbHubConnection.SendAsync("AddInDataBaseAsync", joke);
    }

    public async Task AddInDataBaseAsync(List<Joke> jokes)
    {
        await _jokesDbHubConnection.SendAsync("AddListInDataBaseAsync", jokes);
    }

    #region Private Methods
    private async Task StartConnectionAsync()
    {
        try
        {
            await _jokesDbHubConnection.StartAsync();
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
    #endregion
}
