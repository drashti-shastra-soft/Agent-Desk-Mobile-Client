using AgentDesk.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.APIService;

public static class ApiResponseHandler
{
    public static async Task<Response> HandlerHttpResponse<T>(
        HttpResponseMessage responseMessage)
    {
        var response = new Response();

        try
        {
            var jsonResult = responseMessage.Content != null
                ? await responseMessage.Content.ReadAsStringAsync()
                : null;

            
            // 1. SUCCESS
            
            if (responseMessage.IsSuccessStatusCode &&
                (responseMessage.StatusCode == HttpStatusCode.OK ||
                 responseMessage.StatusCode == HttpStatusCode.NoContent))
            {
                response.IsSuccess = true;
                response.StatusCode = responseMessage.StatusCode;

                if (!string.IsNullOrWhiteSpace(jsonResult))
                {
                    response.Value =
                        JsonConvert.DeserializeObject<T>(
                            jsonResult);
                }
            }

            
            // 2. BAD REQUEST - 400
            
            else if (responseMessage.StatusCode ==
                     HttpStatusCode.BadRequest)
            {
                response.IsSuccess = false;
                response.StatusCode =
                    HttpStatusCode.BadRequest;

                response.ErrorMessage =
                    GetErrorMessage(jsonResult);
            }

            
            // 3. UNAUTHORIZED - 401
            
            else if (responseMessage.StatusCode ==
                     HttpStatusCode.Unauthorized)
            {
                response.IsSuccess = false;
                response.StatusCode =
                    HttpStatusCode.Unauthorized;

                response.ErrorMessage =
                    GetErrorMessage(jsonResult)
                    ?? "Session expired.";

                // Clear session and navigate to login
                await SessionManager.LogoutAsync();
            }

            
            // 4. FORBIDDEN - 403
            
            else if (responseMessage.StatusCode ==
                     HttpStatusCode.Forbidden)
            {
                response.IsSuccess = false;
                response.StatusCode =
                    HttpStatusCode.Forbidden;

                response.ErrorMessage =
                    GetErrorMessage(jsonResult)
                    ?? "You do not have permission to perform this action.";
            }

            
            // 5. NOT FOUND - 404
            
            else if (responseMessage.StatusCode ==
                     HttpStatusCode.NotFound)
            {
                response.IsSuccess = false;
                response.StatusCode =
                    HttpStatusCode.NotFound;

                response.ErrorMessage =
                    GetErrorMessage(jsonResult)
                    ?? "Requested resource was not found.";
            }

            
            // 6. OTHER ERRORS
            
            else
            {
                response.IsSuccess = false;
                response.StatusCode =
                    responseMessage.StatusCode;

                response.ErrorMessage =
                    GetErrorMessage(jsonResult)
                    ?? "Something went wrong.";
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }

    private static string GetErrorMessage(string jsonResult)
    {
        if (string.IsNullOrWhiteSpace(jsonResult))
            return null;

        try
        {
            var apiMessage =
                JsonConvert.DeserializeObject<ApiMessage>(
                    jsonResult);

            return apiMessage?.Message ?? jsonResult;
        }
        catch
        {
            return jsonResult;
        }
    }
}

#region Version 1
//public static class ApiResponseHandler
//{
//    public static async Task<Response> HandlerHttpResponse<T>(HttpResponseMessage responseMessage)
//    {
//        var response = new Response();

//        try
//        {
//            var jsonResult = responseMessage.Content != null
//                ? await responseMessage.Content
//                    .ReadAsStringAsync()
//                : null;

//            if (responseMessage.IsSuccessStatusCode)
//            {
//                response.IsSuccess = true;
//                response.StatusCode = responseMessage.StatusCode;
//                if (!string.IsNullOrWhiteSpace(jsonResult))
//                {
//                    response.Value =
//                        JsonConvert.DeserializeObject<T>(
//                            jsonResult);
//                }
//            }
//            else
//            {
//                response.IsSuccess = false;
//                response.StatusCode = responseMessage.StatusCode;
//                response.ErrorMessage = jsonResult;
//            }
//        }
//        catch (Exception ex)
//        {
//            response.IsSuccess = false;
//            response.ErrorMessage = ex.Message;
//        }

//        return response;
//    }
//}

#endregion

#region Version 2
//public static class ApiResponseHandler
//{
//    public static async Task<Response> HandlerHttpResponse<T>(
//        HttpResponseMessage responseMessage)
//    {
//        var response = new Response();

//        try
//        {
//            var jsonResult =
//                responseMessage.Content != null
//                    ? await responseMessage.Content
//                        .ReadAsStringAsync()
//                    : null;

//            
//            // SUCCESS
//            
//            if (responseMessage.IsSuccessStatusCode)
//            {
//                response.IsSuccess = true;

//                response.StatusCode =
//                    responseMessage.StatusCode;

//                if (!string.IsNullOrWhiteSpace(jsonResult))
//                {
//                    response.Value =
//                        JsonConvert.DeserializeObject<T>(
//                            jsonResult);
//                }

//                return response;
//            }

//            
//            // UNAUTHORIZED - TOKEN EXPIRED / INVALID
//            
//            if (responseMessage.StatusCode ==
//                HttpStatusCode.Unauthorized)
//            {
//                response.IsSuccess = false;

//                response.StatusCode =
//                    HttpStatusCode.Unauthorized;

//                if (!string.IsNullOrWhiteSpace(jsonResult))
//                {
//                    response.ErrorMessage =
//                        JsonConvert
//                            .DeserializeObject<ApiMessage>(
//                                jsonResult)
//                            ?.Message;
//                }

//                if (string.IsNullOrWhiteSpace(
//                    response.ErrorMessage))
//                {
//                    response.ErrorMessage =
//                        "Session expired. Please login again.";
//                }

//                // Logout user and navigate to LoginPage
//                await SessionManager.LogoutAsync();

//                return response;
//            }

//            
//            // BAD REQUEST
//            
//            if (responseMessage.StatusCode ==
//                HttpStatusCode.BadRequest)
//            {
//                response.IsSuccess = false;

//                response.StatusCode =
//                    HttpStatusCode.BadRequest;

//                if (!string.IsNullOrWhiteSpace(jsonResult))
//                {
//                    response.ErrorMessage =
//                        JsonConvert
//                            .DeserializeObject<ApiMessage>(
//                                jsonResult)
//                            ?.Message;
//                }

//                if (string.IsNullOrWhiteSpace(
//                    response.ErrorMessage))
//                {
//                    response.ErrorMessage =
//                        "Invalid request.";
//                }

//                return response;
//            }

//            
//            // OTHER ERRORS
//            
//            response.IsSuccess = false;

//            response.StatusCode =
//                responseMessage.StatusCode;

//            response.ErrorMessage =
//                jsonResult;

//            return response;
//        }
//        catch (Exception ex)
//        {
//            response.IsSuccess = false;

//            response.ErrorMessage = ex.Message;

//            return response;
//        }
//    }
//}
#endregion

#region old method
//public class ApiResponseHandler
//{
//    public static async Task<Response> HandlerHttpResponse<T>(HttpResponseMessage responseMessage)
//    {
//        Response response = new Response();
//        try
//        {
//            string jsonResult = responseMessage.Content != null ? await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false) : null;

//            if (responseMessage.IsSuccessStatusCode &&
//                (responseMessage.StatusCode == System.Net.HttpStatusCode.OK ||
//                responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent))
//            {
//                response.IsSuccess = true;
//                response.StatusCode = System.Net.HttpStatusCode.OK;
//                response.Value = JsonConvert.DeserializeObject<T>(jsonResult);
//            }
//            else if (responseMessage.IsSuccessStatusCode && responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
//            //else if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
//            {
//                response.IsSuccess = false;
//                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
//                response.ErrorMessage = JsonConvert.DeserializeObject<ApiMessage>(jsonResult)?.Message;
//            }
//            //else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
//            else if (!responseMessage.IsSuccessStatusCode && responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
//            {
//                response.IsSuccess = false;
//                response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
//                //response.ErrorMessage = "Session expired.";
//                response.ErrorMessage = JsonConvert.DeserializeObject<ApiMessage>(jsonResult)?.Message;
//                await SessionManager.LogoutAsync();
//            }
//            else
//            {
//                response.IsSuccess = false;
//                response.StatusCode = responseMessage.StatusCode;
//                response.ErrorMessage = jsonResult;
//            }
//        }
//        catch (Exception ex)
//        {
//            _ = ex.Message;
//        }
//        return response;
//    }
//}
#endregion
