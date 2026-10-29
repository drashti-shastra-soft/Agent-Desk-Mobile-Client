using AgentDesk.Models;
using AgentDesk.Models.Common;

namespace AgentDesk.Services.Interfaces;

public interface IClientService
{
    Task<Response> GetAllAsync(ClientLister clientLister);

    Task<Response> AddAsync(Client client);

    Task<Response> GetByIdAsync(int id);

    Task<Response> DeleteAsync(int id, int loginUserId);

    Task<Response> AddClientIdentificationAsync(ClientIdentification clientIdentification);

    Task<Response> GetClientIdentificationByIdAsync(int id);

    Task<Response> DeleteClientIdentificationAsync( int id, int loginUserId);

    Task<Response> AddFinancialInfoAsync(ClientFinancialInfo financialInfo);

    Task<Response> GetFinancialInfoByIdAsync(int id);

    Task<Response> DeleteFinancialInfoAsync(int id, int loginUserId);

    Task<Response> AddClientHealthAsync(ClientHealth clientHealth);

    Task<Response> GetClientHealthByIdAsync(int id);

    Task<Response> DeleteClientHealthAsync(int id, int loginUserId);

    Task<Response> AddClientDocumentAsync(ClientDocument clientDocument);

    Task<Response> GetClientDocumentByIdAsync(int id);

    Task<Response> DeleteClientDocumentAsync(int id, int loginUserId);
}
