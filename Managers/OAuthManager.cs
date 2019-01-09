using System.Net.Http;
using System.Threading.Tasks;
using andead.netcore.oauth.Controllers;
using andead.netcore.oauth.Models;
using Microsoft.Extensions.Logging;

namespace andead.netcore.oauth.Managers
{
    public class OAuthManager
    {
        private readonly string API_VERSION = "5.92";
        private readonly ILogger _logger;

        public OAuthManager(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<AccessTokenResponse> GetAccessTokenAsync(AccessTokenRequest accessTokenRequest)
        {
            using (HttpClient client = new HttpClient())
            {
                AccessTokenResponse response = null;

                try	
                {
                    var result = await client.GetAsync(accessTokenRequest.ToString());
                    response = await result.Content.ReadAsAsync<AccessTokenResponse>();
                }  
                catch (HttpRequestException e)
                {
                    _logger.LogError(e.Message);
                }

                return response;
            }
        }

        public async Task<UsersGetResponse> GetUserInfoAsync(string accessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                UsersGetResponse response = null;

                try	
                {
                    var result = await client.GetAsync(new UsersGetRequest()
                    {
                        AccessToken = accessToken,
                        Version = API_VERSION
                    }
                    .ToString());
                    response = await result.Content.ReadAsAsync<UsersGetResponse>();
                }  
                catch (HttpRequestException e)
                {
                    _logger.LogError(e.Message);
                }

                return response;
            }
        }
    }
}