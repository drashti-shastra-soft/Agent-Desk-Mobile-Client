using AgentDesk.APIService;
using AgentDesk.Handlers;
using AgentDesk.Services;
using AgentDesk.Services.Interfaces;
using AgentDesk.ViewModels;
using AgentDesk.ViewModels.Authentication;
using AgentDesk.Views;
using AgentDesk.Views.Authentication;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Biometric;
using Syncfusion.Maui.Core.Hosting;
using AgentDesk.ViewModels.Popups;
using AgentDesk.Views.Popups;



#if ANDROID
using AgentDesk.Platforms.Android;
#endif

namespace AgentDesk
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Ubuntu-R.ttf", "UbuntuRegular");
                    fonts.AddFont("Ubuntu-L.ttf", "UbuntuLight");
                    fonts.AddFont("Ubuntu-M.ttf", "UbuntuMedium");
                    fonts.AddFont("Ubuntu-B.ttf", "UbuntuBold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // =========================
            // SERVICES
            builder.Services.AddSingleton<ApiServiceHandler>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IDialogueService, DialogueService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IGroupService, GroupService>();
            builder.Services.AddSingleton<IAgencyService, AgencyService>();
            builder.Services.AddSingleton<IClientService, ClientService>();
            builder.Services.AddSingleton<IPolicyService, PolicyService>();

            // Biometric
            builder.Services.AddSingleton<IBiometric>(BiometricAuthenticationService.Default);
            builder.Services.AddSingleton<IBiometricService, BiometricService>();

            // App startup
            builder.Services.AddSingleton<IAppStartupService, AppStartupService>();

            // =========================
            // Shell
            builder.Services.AddSingleton<AppShell>();


            // =========================
            // VIEW MODELS
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<ForgotPasswordPageViewModel>();
            builder.Services.AddTransient<ChangePasswordPageViewModel>();
            builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<GroupPageViewModel>();
            builder.Services.AddTransient<AddGroupPageViewModel>();
            builder.Services.AddTransient<ClientPageViewModel>();
            builder.Services.AddTransient<AddClientPageViewModel>();
            builder.Services.AddTransient<PolicyPageViewModel>();
            builder.Services.AddTransient<AgencyPageViewModel>();
            builder.Services.AddTransient<AddAgencyPageViewModel>();

            //popups
            builder.Services.AddTransient<CommonPopupPageViewModel>();
            builder.Services.AddTransient<CommonPopupPage>();
            builder.Services.AddTransient<CommonAlertPopupPageViewModel>();
            builder.Services.AddTransient<CommonAlertPopupPage>();
            builder.Services.AddTransient<UpdateFupPopupPage>();
            builder.Services.AddTransient<UpdateFupPopupPageViewModel>();


            // =========================
            // PAGES
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ForgotPasswordPage>();
            builder.Services.AddTransient<ChangePasswordPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<GroupPage>();
            builder.Services.AddTransient<AddGroupPage>();
            builder.Services.AddTransient<ClientPage>();
            builder.Services.AddTransient<AddClientPage>();
            builder.Services.AddTransient<PolicyPage>();
            builder.Services.AddTransient<AgencyPage>();
            builder.Services.AddTransient<AddAgencyPage>();


            BoderlessEntryHandler.RemoveBorder();
#if ANDROID
            builder.ConfigureMauiHandlers(handlers =>
            {
                //handlers.AddHandler(
                //    typeof(Shell),
                //    typeof(Platforms.Android.CustomShellRenderer));

                handlers.AddHandler<Shell, CustomShellRenderer>();
            });
#endif
            return builder.Build();
        }
    }
}
