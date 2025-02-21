using Figure;
using Microsoft.AspNetCore.SignalR;

namespace SignalRApp
{
    public class ModelHub : Hub
    {
        #region Private Fields
        private ModelService _modelService;
        private ModelHubService _hubService;

        private CancellationTokenSource _cts;
        private CancellationToken _token;
        #endregion

        public event EventHandler OnStopModel;

        #region Constructors
        public ModelHub(ModelService modelService, ModelHubService hubService)
        {
            _modelService = modelService;
            _hubService = hubService;
        }
        #endregion

        #region Public Methods
        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;

            Console.WriteLine($"(ModelHub) Client connected: {connectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            CancelTheToken();

            var connectionId = Context.ConnectionId;
            Console.WriteLine($"(ModelHub) Client disconnected: {connectionId}");

            await base.OnDisconnectedAsync(exception);
        }

        public async Task StartModelAsync()
        {
            Console.WriteLine($"(ModelHub) ID {Context.ConnectionId}: Start");
            _hubService.ModelStartedHubs.Add(this);
            InitToken();
            while (!_token.IsCancellationRequested)
            {
                List<double> amounts = _modelService.GetAmounts();
                await Clients.Caller.SendAsync("ReceiveAmounts", amounts);

                List<IFigure> figures = _modelService.FiguresInit();
                await DoMissions(figures);
            }
        }

        public void StopModel()
        {
            Console.WriteLine($"(ModelHub) ID {Context.ConnectionId}: Stop");
            _hubService.StopActions(this);
        }

        public void CancelTheToken()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
        #endregion

        #region Private Methods
        private void InitToken()
        {
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
        }

        private async Task DoMissions(List<IFigure> figures)
        {
            string message = string.Empty;
            foreach (var figure in figures)
            {
                figure.StartTheMission();
                message += figure.Message;
                await Clients.Caller.SendAsync("ReceiveMessage", message);
                try { await Task.Delay(_modelService.GetSleep(), _token); }
                catch (OperationCanceledException) { return; }
                figure.StopTheMission();
                message += figure.Message;
                await Clients.Caller.SendAsync("ReceiveMessage", message);
            }
        }
        #endregion
    }
}
