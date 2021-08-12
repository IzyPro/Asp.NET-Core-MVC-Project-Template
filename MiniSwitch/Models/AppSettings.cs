using System;
namespace MiniSwitch.Models
{
    public class AppSettings
    {
        public string AppName { get; set; }
        public string AppEmail { get; set; }
        public string BaseURL { get; set; }
        public string ServiceAppId { get; set; }
        public string ServiceAppKey { get; set; }
        public string MailJetAPIKey { get; set; }
        public string MailJetSecKey { get; set; }
    }
}
