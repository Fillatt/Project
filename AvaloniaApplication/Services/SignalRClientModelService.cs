using ConsoleApp;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace AvaloniaApplication.Services;

public class SignalRClientModelService
{
    #region Private Fields
    private HubConnection _modelHubConnection;
    #endregion

    #region Properties
    public BehaviorSubject<string> MessageSubject { get; set; } = new BehaviorSubject<string>(string.Empty);

    public BehaviorSubject<List<double>> AmountsSubject { get; set; } = new BehaviorSubject<List<double>>(new List<double>());

    public BehaviorSubject<bool> IsConnectedSubject { get; set; } = new BehaviorSubject<bool>(false);
    #endregion

    #region Constructors
    public SignalRClientModelService(ConfigurationService configurationService)
    {
        string url = $"{configurationService.GetSignalRAppUrl()}/model";
        _modelHubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .Build();

        _modelHubConnection.On<List<double>>("ReceiveAmounts", ReceiveAmounts);
        _modelHubConnection.On<string>("ReceiveMessage", ReceiveMessage);
        _modelHubConnection.Closed += (ex) => OnConnectionIsLostAsync();

        StartConnectionAsync();
    }
    #endregion

    #region Public Methods
    public void ReceiveAmounts(List<double> amounts)
    {
        AmountsSubject.OnNext(amounts);
    }

    public void ReceiveMessage(string message)
    {
        MessageSubject.OnNext(message);
    }

    public async Task StartModelAsync()
    {
        await _modelHubConnection.InvokeAsync("StartModelAsync");
    }

    public async Task StopModelAsync()
    {
        await _modelHubConnection.InvokeAsync("StopModel");
    }
    #endregion

    private async Task StartConnectionAsync()
    {
        try
        {
            await _modelHubConnection.StartAsync();
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
