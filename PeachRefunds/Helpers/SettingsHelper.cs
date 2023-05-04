using System.Collections.Specialized;
using System.Configuration;

namespace PeachPayments.Helpers
{
    internal class SettingsHelper
    {		
		// Instance for serving application properties		
		private static readonly NameValueCollection appSettings = ConfigurationManager.AppSettings;

        public static readonly string? PP_BASE_URL = appSettings["PeachPayments.APIBaseUrl"];
        public static readonly string? PP_REFUND_ENDPOINT = appSettings["PeachPayments.RefundEndpoint"];

        public static readonly string VERSION = "0.1.0.0";
		public static readonly string? ENVIRONMENT = appSettings["environment"];        
    }
}
