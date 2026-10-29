using AgentDesk.APIService;
using AgentDesk.Constants;
using AgentDesk.Helpers;
using AgentDesk.Models;
using AgentDesk.Models.Common;
using AgentDesk.Services.Interfaces;

namespace AgentDesk.Services;

public class ClientService : IClientService
{
    private readonly ApiServiceHandler _apiServiceHandler;

    public ClientService(ApiServiceHandler apiServiceHandler)
    {
        _apiServiceHandler = apiServiceHandler;
    }

    public async Task<Response> GetAllAsync(ClientLister clientLister)
    {
        return await _apiServiceHandler.PostAsync<ClientLister, ClientLister>(
            ApiEndpoints.GetClients,
            clientLister,
            AppSession.AuthToken);
    }

    public async Task<Response> AddAsync(Client client)
    {
        return await _apiServiceHandler.PostAsync<Client, Client>(
            ApiEndpoints.AddClient,
            client,
            AppSession.AuthToken);
    }

    public async Task<Response> GetByIdAsync(int id)
    {
        return await _apiServiceHandler.GetAsync<Client>(
            string.Format(ApiEndpoints.GetClient, id),
            AppSession.AuthToken);
    }

    public async Task<Response> DeleteAsync(int id, int loginUserId)
    {
        return await _apiServiceHandler.DeleteAsync(
            string.Format(ApiEndpoints.DeleteClient, id, loginUserId),
            AppSession.AuthToken);
    }
public async Task<Response> AddClientIdentificationAsync(
    ClientIdentification clientIdentification)
    {
        return await _apiServiceHandler.PostAsync<
            ClientIdentification, ClientIdentification>(
                ApiEndpoints.AddClientIdentification,
                clientIdentification,
                AppSession.AuthToken);
    }

    public async Task<Response> GetClientIdentificationByIdAsync(int id)
    {
        return await _apiServiceHandler.GetAsync<ClientIdentification>(
            string.Format(ApiEndpoints.GetClientIdentification, id),
            AppSession.AuthToken);
    }

    public async Task<Response> DeleteClientIdentificationAsync(
        int id, int loginUserId)
    {
        return await _apiServiceHandler.DeleteAsync(
            string.Format(
                ApiEndpoints.DeleteClientIdentification,
                id,
                loginUserId),
            AppSession.AuthToken);
    }
    public async Task<Response> AddFinancialInfoAsync(
    ClientFinancialInfo financialInfo)
    {
        return await _apiServiceHandler.PostAsync<
            ClientFinancialInfo, ClientFinancialInfo>(
                ApiEndpoints.AddFinancialInfo,
                financialInfo,
                AppSession.AuthToken);
    }

    public async Task<Response> GetFinancialInfoByIdAsync(int id)
    {
        return await _apiServiceHandler.GetAsync<ClientFinancialInfo>(
            string.Format(ApiEndpoints.GetFinancialInfo, id),
            AppSession.AuthToken);
    }

    public async Task<Response> DeleteFinancialInfoAsync(
        int id, int loginUserId)
    {
        return await _apiServiceHandler.DeleteAsync(
            string.Format(
                ApiEndpoints.DeleteFinancialInfo,
                id,
                loginUserId),
            AppSession.AuthToken);
    }

    public async Task<Response> AddClientHealthAsync(ClientHealth clientHealth)
    {
        return await _apiServiceHandler.PostAsync<ClientHealth, ClientHealth>(
            ApiEndpoints.AddClientHealth,
            clientHealth,
            AppSession.AuthToken);
    }

    public async Task<Response> GetClientHealthByIdAsync(int id)
    {
        return await _apiServiceHandler.GetAsync<ClientHealth>(
            string.Format(ApiEndpoints.GetClientHealth, id),
            AppSession.AuthToken);
    }

    public async Task<Response> DeleteClientHealthAsync(int id, int loginUserId)
    {
        return await _apiServiceHandler.DeleteAsync(
            string.Format(
                ApiEndpoints.DeleteClientHealth,
                id,
                loginUserId),
            AppSession.AuthToken);
    }

    public async Task<Response> AddClientDocumentAsync(
    ClientDocument clientDocument)
    {
        return await _apiServiceHandler.PostAsync<
            ClientDocument, ClientDocument>(
                ApiEndpoints.AddClientDocument,
                clientDocument,
                AppSession.AuthToken);
    }

    public async Task<Response> GetClientDocumentByIdAsync(int id)
    {
        return await _apiServiceHandler.GetAsync<ClientDocument>(
            string.Format(ApiEndpoints.GetClientDocument, id),
            AppSession.AuthToken);
    }

    public async Task<Response> DeleteClientDocumentAsync(
        int id, int loginUserId)
    {
        return await _apiServiceHandler.DeleteAsync(
            string.Format(
                ApiEndpoints.DeleteClientDocument,
                id,
                loginUserId),
            AppSession.AuthToken);
    }
}