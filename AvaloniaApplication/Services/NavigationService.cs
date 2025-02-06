using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;
using System;

namespace AvaloniaApplication.Services;

public class NavigationService
{
    #region Public Methods
    /// <summary>
    /// Навигация к окну аутентификации
    /// </summary>
    /// <returns></returns>
    public IObservable<IRoutableViewModel> NavigateAuthentication() =>
        Locator.Current.GetService<IScreen>().Router.Navigate.Execute(Locator.Current.GetService<IRoutableViewModel>("Login"));

    /// <summary>
    /// Навгация к окну основной программы
    /// </summary>
    /// <returns></returns>
    public IObservable<IRoutableViewModel> NavigateMain() =>
        Locator.Current.GetService<IScreen>().Router.Navigate.Execute(Locator.Current.GetService<IRoutableViewModel>("Main"));

    /// <summary>
    /// Навигация к окну регистрации
    /// </summary>
    /// <returns></returns>
    public IObservable<IRoutableViewModel> NavigateRegister() =>
        Locator.Current.GetService<IScreen>().Router.Navigate.Execute(Locator.Current.GetService<IRoutableViewModel>("Register"));

    /// <summary>
    /// Навигация к окну с интерфейсом joke API 
    /// </summary>
    /// <returns></returns>
    public IObservable<IRoutableViewModel> NavigateJokeApi() =>
        Locator.Current.GetService<IScreen>().Router.Navigate.Execute(Locator.Current.GetService<IRoutableViewModel>("JokeAPI"));

    /// <summary>
    /// Навигация к окну с интерфейсом neural API
    /// </summary>
    /// <returns></returns>
    public IObservable<IRoutableViewModel> NavigateNeuralApi() =>
        Locator.Current.GetService<IScreen>().Router.Navigate.Execute(Locator.Current.GetService<IRoutableViewModel>("NeuralAPI"));
    #endregion
}
