using Android.Content;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace AgentDesk.Platforms.Android;

public class CustomShellRenderer : ShellRenderer
{
    public CustomShellRenderer(Context context)
        : base(context)
    {
    }

    protected override IShellBottomNavViewAppearanceTracker
        CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
    {
        return new CustomBottomNavViewAppearanceTracker(
            this,
            shellItem);
    }
}

public class CustomBottomNavViewAppearanceTracker
    : ShellBottomNavViewAppearanceTracker
{
    public CustomBottomNavViewAppearanceTracker(
        IShellContext shellContext,
        ShellItem shellItem)
        : base(shellContext, shellItem)
    {
    }

    public override void SetAppearance(
        BottomNavigationView bottomView,
        IShellAppearanceElement appearance)
    {
        base.SetAppearance(bottomView, appearance);

        // REMOVE THE GRAY SELECTED TAB BACKGROUND
        bottomView.ItemBackground = null;

        // REMOVE MATERIAL 3 ACTIVE INDICATOR
        bottomView.ItemActiveIndicatorEnabled = false;

        // REMOVE TAP RIPPLE
        bottomView.ItemRippleColor = null;
    }
}