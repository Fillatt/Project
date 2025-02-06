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
    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await ApiService.HealthRequestAsync();
            if (response?.ReasonPhrase == "OK") IsConnected.OnNext(true);
            else IsConnected.OnNext(false);

            await Task.Delay(5000, stoppingToken);
        }
    }
    #endregion
}
