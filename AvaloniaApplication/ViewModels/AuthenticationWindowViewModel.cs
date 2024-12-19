using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace AvaloniaApplication.ViewModels;

public class AuthenticationWindowViewModel : ViewModelBase
{
    public AuthenticationWindowViewModel()
    {
        StartLoginCommand = ReactiveCommand.CreateFromTask(StartLoginAsync);
        StartRegisterCommand = ReactiveCommand.CreateFromTask(StartRegisterAsync);
    }

    public event EventHandler AuthenticationSuccess;    

    public string Message { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public ReactiveCommand<Unit, Unit> StartLoginCommand { get; }

    public ReactiveCommand<Unit, Unit> StartRegisterCommand { get; }

    private async Task StartLoginAsync()
    {
        EndAuthentication();
    }

    private async Task StartRegisterAsync()
    {
        EndAuthentication();
    }

    private void EndAuthentication() { if (AuthenticationSuccess != null) AuthenticationSuccess(this, new EventArgs()); }
}