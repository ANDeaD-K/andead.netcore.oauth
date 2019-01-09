using System.Collections.Generic;

namespace andead.netcore.oauth.Models
{
    public class Response
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public bool is_closed { get; set; }
        public bool can_access_closed { get; set; }
    }

    public class UsersGetResponse
    {
        public List<Response> response { get; set; }
    }
}