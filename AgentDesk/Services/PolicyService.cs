using AgentDesk.APIService;
using AgentDesk.Constants;
using AgentDesk.Helpers;
using AgentDesk.Models;
using AgentDesk.Models.Common;
using AgentDesk.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Services;

public class PolicyService : IPolicyService
{
    private readonly ApiServiceHandler _apiServiceHandler;

    public PolicyService(ApiServiceHandler apiServiceHandler)
    {
        _apiServiceHandler = apiServiceHandler;
    }

    public async Task<Response> GetAllAsync(PolicyLister policyLister)
    {
        return await _apiServiceHandler.PostAsync<PolicyLister, PolicyLister>(
            ApiEndpoints.GetPolicies,
            policyLister,
            AppSession.AuthToken);
    }

    public async Task<Response> AddAsync(Policy policy)
    {
        return await _apiServiceHandler.PostAsync<Policy, Policy>(
            ApiEndpoints.AddPolicy,
            policy,
            AppSession.AuthToken);
    }

    public async Task<Response> GetByIdAsync(int id)
    {
        return await _apiServiceHandler.GetAsync<Policy>(
            string.Format(ApiEndpoints.GetPolicy, id),
            AppSession.AuthToken);
    }

    public async Task<Response> DeleteAsync(int id, int loginUserId)
    {
        return await _apiServiceHandler.DeleteAsync(
            string.Format(ApiEndpoints.DeletePolicy, id, loginUserId),
            AppSession.AuthToken);
    }

    public async Task<Response> AddPremiumAsync(PolicyPremium policyPremium)
    {
        return await _apiServiceHandler.PostAsync<PolicyPremium, PolicyPremium>(
            ApiEndpoints.AddPolicyPremium,
            policyPremium,
            AppSession.AuthToken);
    }
}
