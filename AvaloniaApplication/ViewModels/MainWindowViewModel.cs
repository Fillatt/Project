using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System.Reactive;

namespace AvaloniaApplication.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen
{
    #region Private Fields
    NavigationService _navigationService;
    #endregion

    #region Properties
    /// <summary>
    /// Маршрутизатор
    /// </summary>
    public RoutingState Router { get; } = new RoutingState();

    /// <summary>
    /// Навигация к окну аутентификации
    /// </summary>
    public ReactiveCommand<Unit, IRoutableViewModel> AuthenticationCommand { get; }

    /// <summary>
    /// Навигация к окну основной программы
    /// </summary>
    public ReactiveCommand<Unit, IRoutableViewModel> MainCommand { get; }

    /// <summary>
    /// Навигация к окну API
    /// </summary>
    public ReactiveCommand<Unit, IRoutableViewModel> ApiCommand { get; }
    #endregion

    #region Constructors
    public MainWindowViewModel()
    {
        _navigationService = Services.Provider.GetRequiredService<NavigationService>();

        AuthenticationCommand = ReactiveCommand.CreateFromObservable(_navigationService.NavigateAuthentication);
        MainCommand = ReactiveCommand.CreateFromObservable(_navigationService.NavigateMain, LoginViewModel.IsAuthenticated);
        ApiCommand = ReactiveCommand.CreateFromObservable(_navigationService.NavigateApi, LoginViewModel.IsAuthenticated);
    }
    #endregion
}
