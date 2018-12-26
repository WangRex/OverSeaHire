namespace Apps.Models.Common
{
    public class SignatureModel
    {
        public string noncestr { get; set; }
        public string Signature { get; set; }
        public string timestamp { get; set; }
        public string access_token { get; set; }
        public string jsapi_ticket { get; set; }
    }
}
