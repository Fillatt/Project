using AvaloniaApplication.ViewModels;
using AvaloniaApplication.Views;
using ReactiveUI;
using System;

namespace AvaloniaApplication;

public class AppViewLocator : ReactiveUI.IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
    {
        LoginViewModel context => new LoginView { DataContext = context },
        MainViewModel context => new MainView { DataContext = context },
        RegisterViewModel content => new RegisterView { DataContext = content },
        JokeAPIViewModel content => new JokeAPIView { DataContext = content },
        NeuralAPIViewModel content => new NeuralAPIView { DataContext = content },
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}
