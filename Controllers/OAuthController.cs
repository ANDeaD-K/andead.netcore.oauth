using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using andead.netcore.oauth.Managers;
using andead.netcore.oauth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace andead.netcore.oauth.Controllers
{
    [Route("api/[controller]")]
    public class OAuthController : Controller
    {
        private readonly ILogger _logger;
        private readonly ConfigurationManager _configuration;
        private readonly JwtManager _jwtManager;

        public OAuthController(ILogger<OAuthController> logger, ConfigurationManager configuration, JwtManager jwtManager)
        {
            _logger = logger;
            _configuration = configuration;
            _jwtManager = jwtManager;
        }

        [HttpGet("vk")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public IActionResult GetAccessToken(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                OAuthManager oauthManager = new OAuthManager(_logger);

                AccessTokenResponse response = oauthManager.GetAccessTokenAsync(new AccessTokenRequest()
                    {
                        ClientId = _configuration.GetValue(ConfigurationKey.CLIENT_ID),
                        ClientSecret = _configuration.GetValue(ConfigurationKey.CLIENT_SECRET),
                        RedirectUri = _configuration.GetValue(ConfigurationKey.REDIRECT_URI),
                        Code = code
                    })
                    .Result;

                if (response != null && response.access_token != null)
                {
                    UsersGetResponse userInfo = oauthManager.GetUserInfoAsync(response.access_token).Result;

                    if (userInfo != null && userInfo.response != null)
                    {
                        HttpContext.Response.Cookies.Append("access_token", _jwtManager.GenerateJwtToken(userInfo));
                        return Redirect("/");
                    }
                }
            }

            return Unauthorized();
        }

        [Authorize, HttpGet("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public IActionResult GetUserLogin()
        {            
            return Ok(JsonConvert.SerializeObject(
                new
                {
                    id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                    first_name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value,
                    role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value
                }, 
                new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
    }
}