namespace Nop.Plugin.ExternalAuth.QQConnect.Models
{
    public class LoginModel
    {
        public string ExternalIdentifier { get; set; }

        public string KnownProvider { get; set; }

        public string ReturnUrl { get; set; }
    }
}
