using AgentDesk.Models;
using AgentDesk.Models.Common;

namespace AgentDesk.Services.Interfaces;

public interface IGroupService
{
    Task<Response> GetAllAsync(GroupLister groupLister);

    Task<Response> AddAsync(GroupModel group);

    Task<Response> GetByIdAsync(int id);

    Task<Response> DeleteAsync(int id, int loginUserId);

    Task<Response> GetActiveGroupAsync();

}
