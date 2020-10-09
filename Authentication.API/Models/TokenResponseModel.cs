using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.API.Models
{
    public class TokenResponseModel
    {
        public string AccessToken { get; set; }

        public TokenResponseModel(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
