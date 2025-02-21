using APIClient;
using AvaloniaApplication.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Serilog;
using Splat;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

namespace AvaloniaApplication.ViewModels
{
    public class JokeAPIViewModel : ReactiveObject, IRoutableViewModel
    {
        #region Private Fields
        private JokeAPIService _apiService;

        private List<Joke> _jokes;

        private SignalRClientJokesDbService _signalRClientService;

        private string _titleConnectionError;
        #endregion

        #region Properties
        /// <summary>
        /// Ссылка на IScreen, которому принадлежит данная модель представления
        /// </summary>
        public IScreen HostScreen { get; }

        /// <summary>
        /// Идентификатор для IRotableViewModel
        /// </summary>
        public string UrlPathSegment { get; } = "LoginViewModel";

        public List<Joke> Jokes
        {
            get => _jokes;
            set { this.RaiseAndSetIfChanged(ref _jokes, value); }
        }

        public string TitleConnectionError
        {
            get => _titleConnectionError;
            set { this.RaiseAndSetIfChanged(ref _titleConnectionError, value); }
        }
        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> RandomJokeCommand { get; }

        public ReactiveCommand<Unit, Unit> TenRandomJokesCommand { get; }
        #endregion

        #region Constructors
        public JokeAPIViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            _apiService = App.Current.Services.GetRequiredService<JokeAPIService>();
            _signalRClientService = App.Current.Services.GetRequiredService<SignalRClientJokesDbService>();

            _signalRClientService.IsConnectedSubject.Subscribe(OnConnectionChange);
            RandomJokeCommand = ReactiveCommand.CreateFromTask(GetRandomJoke, _signalRClientService.IsConnectedSubject);
            TenRandomJokesCommand = ReactiveCommand.CreateFromTask(Get10RandomJokes, _signalRClientService.IsConnectedSubject);
        }
        #endregion

        #region Private Methods
        private async Task GetRandomJoke()
        {
            Log.Debug("JokeAPIViewModel.GetRandomJoke: Start");

            var joke = await _apiService.GetRandomJoke();
            Jokes = [joke];

            await _signalRClientService.AddInDataBaseAsync(joke);

            Log.Debug("JokeAPIViewModel.GetRandomJoke: Done; Jokes: {Jokes}", Jokes);
            Log.Information("JokeAPI: Received a random joke");
        }

        private async Task Get10RandomJokes()
        {
            Log.Debug("JokeAPIViewModel.Get10RandomJokes: Start");

            Jokes = await _apiService.GetRandom10Jokes();

            await _signalRClientService.AddInDataBaseAsync(Jokes);

            Log.Debug("JokeAPIViewModel.Get10RandomJokes: Done; Jokes: {Jokes}", Jokes);
            Log.Information("JokeAPI: Received 10 random jokes");
        }

        private void OnConnectionChange(bool isConnected)
        {
            if (isConnected) TitleConnectionError = string.Empty;
            else TitleConnectionError = "Connection is lost";
        }
        #endregion
    }
}
