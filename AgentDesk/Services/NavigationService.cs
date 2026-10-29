using AgentDesk.Services.Interfaces;

namespace AgentDesk.Services;

public class NavigationService : INavigationService
{
    /// <summary>
    /// Navigate to a registered Shell route.
    /// </summary>
    public Task NavigateToAsync( string route,bool animate = false)
    {
        return Shell.Current.GoToAsync(
            route,
            animate);
    }

    /// <summary>
    /// Navigate to a registered Shell route
    /// with parameters.
    /// </summary>
    public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters,bool animate = false)
    {
        return Shell.Current.GoToAsync(
            route,
            animate,
            routeParameters);
    }

    /// <summary>
    /// Navigate back to the previous page.
    /// </summary>
    //public Task PopAsync(bool animate = false)
    //{
    //    return Shell.Current.GoToAsync(
    //        "..");
    //}

    public Task PopAsync(bool animate = false)
    {
        return Shell.Current.GoToAsync(
            "..",
            animate);
    }

    /// <summary>
    /// Navigate back to the previous page
    /// with route parameters.
    /// </summary>
    public Task PopAsync(IDictionary<string, object> routeParameters,bool animate = false)
    {
        return Shell.Current.GoToAsync(
            "..",
            animate,
            routeParameters);
    }

    /// <summary>
    /// Navigate back to the root page.
    /// </summary>
    public Task RootPopAsync(bool animate = false)
    {
        return Shell.Current.Navigation.PopToRootAsync(animate);
    }

    /// <summary>
    /// Navigate back to a specific route.
    /// </summary>
    public Task PopToAsync(string route,bool animate = false)
    {
        return Shell.Current.GoToAsync( $"//{route}", animate);
    }

    /// <summary>
    /// Navigate to the root route.
    /// </summary>
    public Task PopToRootAsync( bool animate = false)
    {
        return Shell.Current.GoToAsync(
            "//",
            animate);
    }

    /// <summary>
    /// Navigate back to a specific route
    /// using Shell relative navigation.
    /// </summary>
    public Task GoBackToAsync(
        string route,
        bool animate = false)
    {
        return Shell.Current.GoToAsync(
            $"../{route}",
            animate);
    }
}