namespace AgentDesk.Services.Interfaces;

public interface INavigationService
{
    Task NavigateToAsync(
        string route,
        bool animate = false);

    Task NavigateToAsync(
        string route,
        IDictionary<string, object> routeParameters,
        bool animate = false);

    Task PopAsync(
        bool animate = false);

    Task PopAsync(
        IDictionary<string, object> routeParameters,
        bool animate = false);

    Task RootPopAsync(
        bool animate = false);

    Task PopToAsync(
        string route,
        bool animate = false);

    Task PopToRootAsync(
        bool animate = false);

    Task GoBackToAsync(
        string route,
        bool animate = false);
}
