using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using andead.netcore.oauth.Models;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace andead.netcore.oauth.Managers
{
    public class JwtManager
    {
        private readonly ConfigurationManager _configurationManager;
        private readonly ILogger _logger;

        public JwtManager(ConfigurationManager configurationManager, ILogger<JwtManager> logger)
        {
            _configurationManager = configurationManager;
            _logger = logger;
        }

        public string GenerateJwtToken(UsersGetResponse userInfo)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(
                new
                {
                    info = $"Generate JWT Token for user {userInfo.response[0].first_name} ({userInfo.response[0].id})"
                }
            ));

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.response[0].id.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, userInfo.response[0].first_name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "User")
            };

            var token = new JwtSecurityToken(
                issuer: _configurationManager.GetValue(ConfigurationKey.ISSUER),
                audience: _configurationManager.GetValue(ConfigurationKey.AUDIENCE),
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
                signingCredentials: new SigningCredentials(_configurationManager.GetSymmetricSecurityKey(ConfigurationKey.SIGNING_KEY), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}