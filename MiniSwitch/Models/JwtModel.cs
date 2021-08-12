using System;
namespace MiniSwitch.Models
{
    public class JwtModel
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
