using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PicSayAndPlay.Services
{
    public static class BaseApiEndpoint
    {
        //protected abstract string BaseUri { get; }
        //protected abstract HttpClient HttpClient { get; }

        public static async Task<TResult> GetApiAsync<TResult>(string relativeUri)
        {
            var requestUri = new Uri(relativeUri);

            var httpRequestMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, requestUri);
            HttpResponseMessage response = await new HttpClient().SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(jsonResponse);
            }
            else
            {
                throw await ValidateErrorResponse("GetApiAsync", requestUri.AbsoluteUri, response);
            }
        }

        public static async Task PostApiAsync(string relativeUri, object model)
        {
            var requestUri = new Uri(relativeUri);

            var body = JsonConvert.SerializeObject(model);
            StringContent stringContent = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await new HttpClient().PostAsync(requestUri, stringContent);

            if (!response.IsSuccessStatusCode)
                throw await ValidateErrorResponse("PostApiAsync", requestUri.AbsoluteUri, response);
        }

        public static async Task<TResult> PostApiAsync<TResult>(string relativeUri, object model)
        {
            var requestUri = new Uri(relativeUri);

            var body = JsonConvert.SerializeObject(model);
            StringContent stringContent = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await new HttpClient().PostAsync(requestUri, stringContent);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(jsonResponse);
            }
            else
            {
                throw await ValidateErrorResponse("PostApiAsync", requestUri.AbsoluteUri, response);
            }
        }

        public static async Task PutApiAsync(string relativeUri)
        {
            var requestUri = new Uri(relativeUri);

            var httpRequestMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Put, requestUri);
            HttpResponseMessage response = await new HttpClient().SendAsync(httpRequestMessage);

            if (!response.IsSuccessStatusCode)
                throw await ValidateErrorResponse("PutApiAsync", requestUri.AbsoluteUri, response);
        }

        private static async Task<ApiInvalidOperationException> ValidateErrorResponse(string method, string url, HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new ApiInvalidOperationException { AppExceptionCode = "Error_NotFound", AppExceptionType = ApiInvalidOperationExceptionType.Client };
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var applicationException = JsonConvert.DeserializeObject<ApiInvalidOperationException>(jsonResponse);
                applicationException.AppExceptionType = ApiInvalidOperationExceptionType.ServerLogic;
                return applicationException;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return new ApiInvalidOperationException { AppExceptionType = ApiInvalidOperationExceptionType.ServerAuth };
            }
            else
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return new ApiInvalidOperationException { AppExceptionCode = "Error_InternalServerError", AppExceptionType = ApiInvalidOperationExceptionType.ServerLogic };
            }
        }
    }
}