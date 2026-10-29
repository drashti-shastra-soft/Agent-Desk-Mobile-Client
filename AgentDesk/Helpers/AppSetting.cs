namespace AgentDesk.Helpers;

public static class AppSetting
{
    private const string EmailKey = "LoginEmail";
    private const string PasswordKey = "LoginPassword";
    private const string RememberMeKey = "RememberMe";

    public static void SetEmail(string email)
    {
        Preferences.Set(EmailKey, email);
    }

    public static string GetEmail()
    {
        return Preferences.Get(EmailKey, string.Empty);
    }

    public static void SetPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            SecureStorage.Remove(PasswordKey);
            return;
        }

        SecureStorage.SetAsync(PasswordKey, password);
    }

    public static async Task<string> GetPasswordAsync()
    {
        try
        {
            return await SecureStorage.GetAsync(PasswordKey)
                   ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    public static void SetRememberMe(bool value)
    {
        Preferences.Set(RememberMeKey, value);
    }

    public static bool GetRememberMe()
    {
        return Preferences.Get(RememberMeKey, false);
    }

    public static void ClearLogin()
    {
        Preferences.Remove(EmailKey);
        Preferences.Remove(RememberMeKey);

        SecureStorage.Remove(PasswordKey);
    }
}
