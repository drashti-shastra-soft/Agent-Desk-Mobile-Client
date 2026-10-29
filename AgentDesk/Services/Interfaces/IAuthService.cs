using AgentDesk.Models.Auth;
using AgentDesk.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Services.Interfaces;

public interface IAuthService
{
    Task<Response> LoginAsync(Credentials credentials);

    Task<Response> ForgotPasswordAsync(ForgotPassword forgotPassword);

    Task<Response> ChangePasswordAsync(ChangePassword changePassword);
}
