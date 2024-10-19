using Mango.Web.Models;
using Mango.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public class BaseService : IBaseService
    {
        //HttpClientFactoy object
        private readonly IHttpClientFactory _httpClientFactory;

        //HttpClientFactoy object inject
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<ResponseDTO?> SendAsync(RequestDTO requestDTO)
        {
            #region Message
            //Create the rquired HttpMessage
            HttpRequestMessage message = new();
            //Add Headers
            message.Headers.Add("Accept", "application/json");
            //Request URI
            message.RequestUri = new Uri(requestDTO.Url);
            //Content -- if POST PUT DELETE then -- Serialize
            if (requestDTO.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(requestDTO.Data), Encoding.UTF8, "application/json");
            }
            //Add method Type
            switch (requestDTO.ApiType)
            {
                case ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;

            }
            #endregion

            //token

            #region Send the Message Package through httpClientFactory
            //CreateClient
            var httpClient = _httpClientFactory.CreateClient("MangoAPI");
            //Send message and get the response
            HttpResponseMessage? apiResponseMessage = null;
            apiResponseMessage = await httpClient.SendAsync(message);
            #endregion

            #region Check the Response on the basis of status code
            switch (apiResponseMessage.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new() { IsSuccess = false , Message = "Not Found" };
                case HttpStatusCode.Forbidden:
                    return new() { IsSuccess = false, Message = "Access Denied" };
                case HttpStatusCode.Unauthorized:
                    return new() { IsSuccess = false, Message = "Unauthorized" };
                case HttpStatusCode.InternalServerError:
                    return new() { IsSuccess = false, Message = "Internal Server Error" };
                default:
                    var apiContent = await apiResponseMessage.Content.ReadAsStringAsync();
                    //Deserialize the Content
                    var apiResponse = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
                    return apiResponse;
            }
            #endregion
        }
    }
}
