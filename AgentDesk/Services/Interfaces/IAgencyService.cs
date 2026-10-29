using AgentDesk.Models;
using AgentDesk.Models.Common;

namespace AgentDesk.Services.Interfaces;

public interface IAgencyService
{
    Task<Response> GetAllAsync(AgencyLister agencyLister);

    Task<Response> AddAsync(Agency agency);

    Task<Response> GetByIdAsync(int id);

    Task<Response> DeleteAsync(int id, int loginUserId);

    Task<Response> GetActiveAgencyAsync();
}