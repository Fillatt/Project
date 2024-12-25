using ReactiveUI;
using System;
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
        AuthenticationCommand = ReactiveCommand.CreateFromObservable(NavigateAuthentication);
        MainCommand = ReactiveCommand.CreateFromObservable(NavigateMain, LoginViewModel.IsAuthenticated);
        LoginViewModel.RegisterSelected += (sender, args) => NavigateRegister();
        AuthenticationCommand.Execute();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Навигация к окну аутентификации
    /// </summary>
    /// <returns></returns>
    private IObservable<IRoutableViewModel> NavigateAuthentication() => Router.Navigate.Execute(new LoginViewModel(this));

    /// <summary>
    /// Навгация к окну основной программы
    /// </summary>
    /// <returns></returns>
    private IObservable<IRoutableViewModel> NavigateMain() => Router.Navigate.Execute(new MainViewModel(this));

    /// <summary>
    /// Навигация к окну регистрации
    /// </summary>
    /// <returns></returns>
    private IObservable<IRoutableViewModel> NavigateRegister() => Router.Navigate.Execute(new RegisterViewModel(this));
    #endregion
}
