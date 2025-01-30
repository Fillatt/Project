using ReactiveUI;
using Splat;
using System.Collections.Generic;
using APIClient;
using System.Reactive;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaApplication.ViewModels
{
    public class APIViewModel : ReactiveObject, IRoutableViewModel
    {
        #region
        APIService _apiService;
        List<Joke> _jokes;
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
        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> RandomJokeCommand { get; }

        public ReactiveCommand<Unit, Unit> TenRandomJokesCommand { get; }
        #endregion

        #region Constructors
        public APIViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            _apiService = Services.Provider.GetRequiredService<APIService>();
            RandomJokeCommand = ReactiveCommand.CreateFromTask(GetRandomJoke);
            TenRandomJokesCommand = ReactiveCommand.CreateFromTask(Get10RandomJokes);
        }
        #endregion

        #region
        private async Task GetRandomJoke()
        {
            var joke = await _apiService.GetRandomJoke();
            Jokes = [joke];
        }

        private async Task Get10RandomJokes() => Jokes = await _apiService.GetRandom10Jokes();
        
        #endregion
    }
}
