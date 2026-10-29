using AgentDesk.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Helpers;

public static class AppSession
{
    public static User CurrentUser { get; set; }
    public static int UserId { get; set; }

    public static string AuthToken { get; set; } = string.Empty;

    public static bool IsLoggedIn =>
        CurrentUser != null &&
        !string.IsNullOrWhiteSpace(AuthToken);

    public static void Clear()
    {
        CurrentUser = null;
        AuthToken = string.Empty;
    }
}
