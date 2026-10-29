namespace AgentDesk.Services.Interfaces;

public interface IBiometricService
{
    Task<bool> IsAvailableAsync();

    Task<bool> AuthenticateAsync();

    Task<bool> EnableAsync();

    Task DisableAsync();

    Task<bool> IsEnabledAsync();
}