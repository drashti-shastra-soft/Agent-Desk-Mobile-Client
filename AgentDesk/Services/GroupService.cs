using AgentDesk.APIService;
using AgentDesk.Constants;
using AgentDesk.Helpers;
using AgentDesk.Models;
using AgentDesk.Models.Common;
using AgentDesk.Services.Interfaces;

namespace AgentDesk.Services;

public class GroupService : IGroupService
{
    private readonly ApiServiceHandler _apiServiceHandler;

    public GroupService(ApiServiceHandler apiServiceHandler)
    {
        _apiServiceHandler = apiServiceHandler;
    }

    public async Task<Response> GetAllAsync(GroupLister groupLister)
    {
        return await _apiServiceHandler.PostAsync<GroupLister, GroupLister>(
            ApiEndpoints.GetGroups,
            groupLister,
            AppSession.AuthToken);
    }

    public async Task<Response> AddAsync(GroupModel group)
    {
        return await _apiServiceHandler.PostAsync<GroupModel, GroupModel>(
            ApiEndpoints.AddGroup,
            group,
            AppSession.AuthToken);
    }

    public async Task<Response> GetByIdAsync(int id)
    {
        return await _apiServiceHandler.GetAsync<GroupModel>(
            string.Format(ApiEndpoints.GetGroup, id),
            AppSession.AuthToken);
    }

    public async Task<Response> DeleteAsync( int id, int loginUserId)
    {
        return await _apiServiceHandler.DeleteAsync(
            string.Format(ApiEndpoints.DeleteGroup, id, loginUserId),
            AppSession.AuthToken);
    }

    public async Task<Response> GetActiveGroupAsync()
    {
        return await _apiServiceHandler.GetAsync<List<GroupModel>>(
            ApiEndpoints.GetActiveGroups,
            AppSession.AuthToken);
    }
}