using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;
using System;

namespace AvaloniaApplication;

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
    /// Навигация к окну с интерфейсом API 
    /// </summary>
    /// <returns></returns>
    public IObservable<IRoutableViewModel> NavigateApi() =>
        Locator.Current.GetService<IScreen>().Router.Navigate.Execute(Locator.Current.GetService<IRoutableViewModel>("API"));
    #endregion
}
