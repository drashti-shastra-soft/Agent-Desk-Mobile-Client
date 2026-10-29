using AgentDesk.APIService;
using AgentDesk.Constants;
using AgentDesk.Helpers;
using AgentDesk.Models.Auth;
using AgentDesk.Models.Common;
using AgentDesk.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Services;

public class AuthService : IAuthService
{

    private readonly ApiServiceHandler _apiServiceHandler;

    public AuthService(ApiServiceHandler apiServiceHandler)
    {
        _apiServiceHandler = apiServiceHandler;
    }

    public async Task<Response> LoginAsync(Credentials credentials)
    {
        return await _apiServiceHandler.PostAsync<
            Credentials,User>(ApiEndpoints.Login,credentials);
    }
    public async Task<Response> ForgotPasswordAsync(ForgotPassword forgotPassword)
    {
        return await _apiServiceHandler.PostAsync< ForgotPassword, ForgotPassword>(ApiEndpoints.ForgotPassword, forgotPassword);
    }

    public async Task<Response> ChangePasswordAsync(ChangePassword changePassword)
    {
        return await _apiServiceHandler.PutAsync<ChangePassword>(ApiEndpoints.ChangePassword,changePassword,AppSession.AuthToken);
    }
}
