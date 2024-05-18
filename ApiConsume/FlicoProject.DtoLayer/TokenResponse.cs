using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DtoLayer
{
    public class TokenResponse
    {
        public TokenResponse(string token, DateTime expireTime)
        {
            Token = token;
            ExpireTime = expireTime;
        }

        public string Token { get; set; }

        public DateTime ExpireTime { get; set; }

    }
}
