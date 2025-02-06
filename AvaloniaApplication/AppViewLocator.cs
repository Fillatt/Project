using AvaloniaApplication.ViewModels;
using AvaloniaApplication.Views;
using ReactiveUI;
using System;

namespace AvaloniaApplication;

public class AppViewLocator : ReactiveUI.IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
    {
        LoginViewModel context => new LoginPage { DataContext = context },
        MainViewModel context => new MainPage { DataContext = context },
        RegisterViewModel content => new RegisterPage { DataContext = content },
        JokeAPIViewModel content => new JokeAPIPage { DataContext = content },
        NeuralAPIViewModel content => new NeuralAPIPage { DataContext = content },
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}
