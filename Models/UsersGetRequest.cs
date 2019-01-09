using System.Collections.Generic;

namespace andead.netcore.oauth.Models
{
    public class UsersGetRequest
    {
        public string AccessToken { get; set; }
        public string Version { get; set; }

        public override string ToString()
        {
            return $"https://api.vk.com/method/users.get?access_token={AccessToken}&v={Version}";
        } 
    }
}