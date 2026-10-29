using AgentDesk.Constants;
using AgentDesk.Models.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AgentDesk.APIService;

public class ApiServiceHandler
{
    private readonly HttpClient _httpClient;

    public ApiServiceHandler()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(ApiConstants.BaseUrl),
            Timeout = TimeSpan.FromMinutes(15)
        };

        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue(
                "application/json"));
    }

    public async Task<Response> GetAsync<T>(
        string uri,
        string authToken = "")
    {
        try
        {
            SetAuthorization(authToken);

            var response = await _httpClient.GetAsync(uri);

            return await ApiResponseHandler
                .HandlerHttpResponse<T>(response);
        }
        catch (Exception ex)
        {
            return new Response
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<Response> PostAsync<T, TR>(
        string uri,
        T data,
        string authToken = "")
    {
        try
        {
            SetAuthorization(authToken);

            var json = JsonConvert.SerializeObject(data);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response =
                await _httpClient.PostAsync(uri, content);

            return await ApiResponseHandler
                .HandlerHttpResponse<TR>(response);
        }
        catch (Exception ex)
        {
            return new Response
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<Response> PutAsync<T>(
    string uri,
    T data,
    string authToken = "")
    {
        try
        {
            using var requestMessage = new HttpRequestMessage(
                HttpMethod.Put,
                uri);

            if (!string.IsNullOrWhiteSpace(authToken))
            {
                requestMessage.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        authToken);
            }

            var json = JsonConvert.SerializeObject(data);

            requestMessage.Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var responseMessage = await _httpClient.SendAsync(requestMessage);

            return await ApiResponseHandler
                .HandlerHttpResponse<T>(responseMessage);
        }
        catch (Exception ex)
        {
            return new Response
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<Response> DeleteAsync(
    string uri,
    string authToken = "")
    {
        try
        {
            using var requestMessage = new HttpRequestMessage(
                HttpMethod.Delete,
                uri);

            if (!string.IsNullOrWhiteSpace(authToken))
            {
                requestMessage.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        authToken);
            }

            var responseMessage =
                await _httpClient.SendAsync(requestMessage);

            return await ApiResponseHandler
                .HandlerHttpResponse<object>(responseMessage);
        }
        catch (Exception ex)
        {
            return new Response
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }
    private void SetAuthorization(string authToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrWhiteSpace(authToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    authToken);
        }
    }
}