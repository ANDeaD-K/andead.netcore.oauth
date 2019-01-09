

namespace andead.netcore.oauth.Models
{
    public class AccessTokenRequest
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string Code { get; set; }

        public override string ToString()
        {
            return $"https://oauth.vk.com/access_token?client_id={ClientId}&client_secret={ClientSecret}&redirect_uri={RedirectUri}&code={Code}";
        }
    }
}