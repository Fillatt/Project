using Serilog;
using System.Reactive.Subjects;
namespace APIClient;

public class HealthCheckService
{
    #region Properties
    public NeuralAPIService ApiService { get; set; }

    public BehaviorSubject<bool> IsConnected { get; set; } = new BehaviorSubject<bool>(false);
    #endregion

    #region Constructors
    public HealthCheckService(NeuralAPIService apiService)
    {
        ApiService = apiService;
    }
    #endregion

    #region Public Methods
    public async Task ExecuteAsync(CancellationToken stopToken)
    {
        Log.Debug("HealthCheckService.ExecuteAsync: Start");
        while (!stopToken.IsCancellationRequested)
        {
            var response = await ApiService.HealthRequestAsync();
            if (response?.ReasonPhrase == "OK") IsConnected.OnNext(true);
            else
            {
                IsConnected.OnNext(false);
                Log.Warning("HealthCheckService.ExecuteAsync: Сonnection lost");
            }

            Log.Debug("HealthCheckService.ExecuteAsync: Response: {Response}", response);

            await Task.Delay(5000, stopToken);
        }
        Log.Debug("HealthCheckService.ExecuteAsync: Done");
    }
    #endregion
}
