using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System.Reactive;

namespace AvaloniaApplication.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen
{
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
    #endregion

    #region Constructors
    public MainWindowViewModel()
    {
        AuthenticationCommand = ReactiveCommand
            .CreateFromObservable(Services.Provider.GetRequiredService<NavigateService>().NavigateAuthentication);

        MainCommand = ReactiveCommand
            .CreateFromObservable(Services.Provider.GetRequiredService<NavigateService>().NavigateMain, LoginViewModel.IsAuthenticated);
    }
    #endregion
}
