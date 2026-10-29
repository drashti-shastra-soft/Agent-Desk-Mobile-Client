using AgentDesk.Services.Interfaces;
using AgentDesk.ViewModels;
using AgentDesk.ViewModels.Authentication;
using AgentDesk.Views;
using AgentDesk.Views.Authentication;

namespace AgentDesk;

public partial class AppShell : Shell
{
    private readonly IAppStartupService _startupService;
    private bool _startupCompleted;

    //public AppShell()
    //{
    //    InitializeComponent();

    //    RegisterRoutes();
    //}

    public AppShell(
            IAppStartupService startupService)
    {
        InitializeComponent();

        _startupService = startupService;

        RegisterRoutes();
    }

    private void RegisterRoutes()
    {
        RegisterRoute<ForgotPasswordPage>();
        RegisterRoute<ChangePasswordPage>();
        RegisterRoute<HomePage>();
        RegisterRoute<GroupPage>();
        RegisterRoute<AddGroupPage>();
        RegisterRoute<ClientPage>();
        RegisterRoute<AddClientPage>();
        RegisterRoute<PolicyPage>();
        RegisterRoute<AgencyPage>();
        RegisterRoute<AddAgencyPage>();
    }

    private void RegisterRoute<TPage>()
        where TPage : Page
    {
        Routing.RegisterRoute(
            typeof(TPage).Name,
            typeof(TPage));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_startupCompleted)
            return;

        _startupCompleted = true;

        await Task.Delay(1500);
        //await Task.Delay(10000);

        await _startupService.InitializeAsync();
    }
}
