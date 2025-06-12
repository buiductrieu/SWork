

namespace SWork.Common.Helper
{
    public class PayOSSettings
    {
        public const string SectionName = "PayOSSettings";

        public string ClientID { get; set; } = string.Empty;
        public string APIKey { get; set; } = string.Empty;
        public string ChecksumKey { get; set; } = string.Empty;
        public string PayOSBaseUrl { get; set; } = string.Empty;

    }
}
