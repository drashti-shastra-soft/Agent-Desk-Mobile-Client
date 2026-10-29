using AgentDesk.APIService;
using AgentDesk.Constants;
using AgentDesk.Helpers;
using AgentDesk.Models;
using AgentDesk.Models.Common;
using AgentDesk.Services.Interfaces;

namespace AgentDesk.Services;

public class AgencyService : IAgencyService
{
    private readonly ApiServiceHandler _apiServiceHandler;

    public AgencyService(ApiServiceHandler apiServiceHandler)
    {
        _apiServiceHandler = apiServiceHandler;
    }

    public async Task<Response> GetAllAsync(AgencyLister agencyLister)
    {
        return await _apiServiceHandler.PostAsync<AgencyLister, AgencyLister>(
            ApiEndpoints.GetAgencies,
            agencyLister,
            AppSession.AuthToken);
    }

    public async Task<Response> AddAsync(Agency agency)
    {
        return await _apiServiceHandler.PostAsync<Agency, Agency>(
            ApiEndpoints.AddAgency,
            agency,
            AppSession.AuthToken);
    }

    public async Task<Response> GetByIdAsync(int id)
    {
        return await _apiServiceHandler.GetAsync<Agency>(
            string.Format(ApiEndpoints.GetAgency, id),
            AppSession.AuthToken);
    }

    public async Task<Response> DeleteAsync(int id, int loginUserId)
    {
        return await _apiServiceHandler.DeleteAsync(
            string.Format(ApiEndpoints.DeleteAgency, id, loginUserId),
            AppSession.AuthToken);
    }

    public async Task<Response> GetActiveAgencyAsync()
    {
        return await _apiServiceHandler.GetAsync<List<Agency>>(
            ApiEndpoints.GetActiveAgencies,
            AppSession.AuthToken);
    }
}