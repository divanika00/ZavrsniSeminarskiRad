using IdentityModel.Client;
using Newtonsoft.Json;
using System.Text;

namespace ZavrsniSeminarskiRad.Services
{
    public class ServiceClientBase
    {
        private readonly HttpClient _httpClient;

        public ServiceClientBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Method that send request to the endpoint with http client
        /// </summary>
        /// <param name="url">Specified action url</param>
        /// <param name="httpMethod">Specified http method</param>
        /// <param name="token">Specified access token</param>
        /// <param name="contentObj">Specified request body</param>
        /// <returns></returns>
        protected Task<HttpResponseMessage> DoRequest(string url, HttpMethod httpMethod, string token, object contentObj = null)
        {
            var request = new HttpRequestMessage(httpMethod, url);

            if (contentObj != null)
            {
                var content = new StringContent(JsonConvert.SerializeObject(contentObj), Encoding.UTF8, "application/json");

                request.Content = content;
            }
            _httpClient.SetBearerToken(token);

            return _httpClient.SendAsync(request);
        }


        protected Task<HttpResponseMessage> DoRequest(string url, HttpMethod httpMethod, object contentObj = null)
        {
            var request = new HttpRequestMessage(httpMethod, url);

            if (contentObj != null)
            {
                var content = new StringContent(JsonConvert.SerializeObject(contentObj), Encoding.UTF8, "application/json");

                request.Content = content;
            }
            return _httpClient.SendAsync(request);
        }


        protected Task<HttpResponseMessage> DoRequest(string url, HttpMethod httpMethod)
        {
            var request = new HttpRequestMessage(httpMethod, url);
            return _httpClient.SendAsync(request);
        }

        /// <summary>
        /// Method that try to get to the response content and deserialize it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseMessage">Specified response message</param>
        /// <param name="readResponseBody">Specified if body need to be read</param>
        /// <param name="unsuccessfulResponseAction"></param>
        /// <returns></returns>
        protected async Task<T> TryGetResponseContent<T>(HttpResponseMessage responseMessage, bool readResponseBody, Action<HttpResponseMessage> unsuccessfulResponseAction)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                if (readResponseBody)
                {
                    var response = await responseMessage.Content.ReadAsStringAsync();
                    var responseObj = JsonConvert.DeserializeObject<T>(response);

                    return responseObj;
                }
            }
            else
            {
                if (unsuccessfulResponseAction != null)
                {
                    unsuccessfulResponseAction.Invoke(responseMessage);
                }
            }

            return default(T);
        }

        protected async Task<T> DoRequestAndTryGetResponseContent<T>(string url, HttpMethod httpMethod, string token, bool readResponseBody, object contentObj = null,
            Action<HttpResponseMessage> unsuccessfulResponseAction = null)
        {
            var responseMessage = await DoRequest(url, httpMethod, token, contentObj);

            return await TryGetResponseContent<T>(responseMessage, readResponseBody, unsuccessfulResponseAction);
        }

        protected async Task<T> DoRequestAndTryGetResponseContent<T>(string url, HttpMethod httpMethod, bool readResponseBody,
    Action<HttpResponseMessage> unsuccessfulResponseAction = null)
        {
            var responseMessage = await DoRequest(url, httpMethod);
            return await TryGetResponseContent<T>(responseMessage, readResponseBody, unsuccessfulResponseAction);
        }
    }
}
