using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Wel.Data
{

    public class user
    {
        [JsonProperty ("accessToken")]
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<object> UserRoles { get; set; }
        public bool Subscribed { get; set; }
        public int GenderId { get; set; }
        public int UserId { get; set; }
    }
        public class userObject
        {
            public int statusCode { get; set; }
            public bool success { get; set; }
            public string value { get; set; }

        }

    
}
