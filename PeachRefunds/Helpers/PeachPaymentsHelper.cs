using Newtonsoft.Json;
using PeachPayments.Models;
using RestSharp;
using System.Security.Cryptography;
using System.Text;

namespace PeachPayments.Helpers
{
    internal class PeachPaymentsHelper {

        internal static readonly Dictionary<string, string> codes = new(){
            {"000.000.000","Transaction successfully processed in LIVE system" },
            {"000.100.110","Transaction successfully processed in TEST system" },
            {"000.200.000","Transaction pending" },
            {"000.200.100","Successfully created checkout (Indicates that the session is now open" },
            {"100.396.101","Cancelled by user" },
            {"100.396.104","Uncertain status - probably cancelled by user" },
            {"200.300.404","Parameter in the incorrect format (The returned description will give details on which parameter is in the incorrect format)" },
            {"600.200.400","Unsupported Payment Type (only DB is allowed as a payment type)" },
            {"600.200.500","Invalid payment data. You are not configured for this currency of sub type" },
            {"800.100.152","Transaction declined by authorization system" },
            {"800.900.201","Unknown channel (Entity Id in payment request is incorrect)" },
            {"800.900.300","Invalid authentication information (Authentication password is incorrect)" },
        };

        public static async Task<RefundResult?> ProcessRefund(
                string entityId,
                string secret,
                double amount,
                string currency,
                string transactionId) {

            RefundResult? refundResult = null;

            //https://developer.peachpayments.com/docs/checkout-refund                        
            var restClient = new RestClient(SettingsHelper.PP_BASE_URL!);
            var restRequest = new RestRequest(SettingsHelper.PP_REFUND_ENDPOINT, Method.Post);

            restRequest.AddOrUpdateHeader("Content-Type", "application/x-www-form-urlencoded");

            Dictionary<string, string> values = new() {
                { "amount", amount.ToString("0.00") },
                { "authentication.entityId", entityId.Trim() },
                { "currency", currency.Trim() },
                { "id", transactionId.Trim() },
                { "paymentType", "RF" } //RF denotes a refund            
            };

            foreach(var value in values) {
                restRequest.AddParameter(value.Key, value.Value);
            }

            // Signature is added last
            restRequest.AddParameter("signature", GetSignature(values, secret));

            var response = await restClient.ExecutePostAsync(restRequest);

            var result = response.Content;

            // See if we can deserialize into the expected format
            refundResult = JsonConvert.DeserializeObject<RefundResult>(result!);

            //If not, see if we can deserialize into the error format
            if(refundResult?.Result == null) {
                ErrorResponse? errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(result!);

                if(errorResponse != null) {
                    refundResult!.Result = new Result() {
                        // Assign the code and desription to the return object
                        Code = errorResponse.Code,
                        Description = errorResponse.Description
                    };

                    // Append the offending item to the return object description
                    if(errorResponse != null && errorResponse!.ParameterErrors?.Count > 0) {
                        foreach(var error in errorResponse.ParameterErrors) {                            
                            refundResult.Result.Description += " (" + 
                                String.Format("name:'{0}', value:'{1}', message:'{2}'",
                                    error.Name, error.Value, error.Message) + ") ";
                        }
                    }
                }
            }

            return refundResult;
        }

        private static string GetSignature(Dictionary<string, string> values, string secret) {
            //https://developer.peachpayments.com/docs/checkout-authentication#how-to-generate-a-signature-hmac-sha256
            //The signature uses the HMAC SHA256 algorithm, all parameters in the request,
            //and the secret token as the key to generate the signature. 

            //The secret token is a shared secret between the merchant and
            //Peach Payments and you can retrieve it from the Peach Payments Dashboard or Console.

            //To generate the signature, all payment parameters(including empty parameters) must be in alphabetical order,
            //concatenated, without any spaces or special characters, and signed with the secret token as the key.
            //The generated signature itself forms part of the request for validation by Peach Payments.

            StringBuilder stringBuilder = new();

            foreach(var item in values) {
                stringBuilder.Append(item.Key);
                stringBuilder.Append(item.Value);
            }

            using HMACSHA256 hmac = new(UTF8Encoding.UTF8.GetBytes(secret));
            return BitConverter.ToString(hmac.ComputeHash(UTF8Encoding.UTF8.GetBytes(stringBuilder.ToString()))).Replace("-", "").ToLower();
        }
    }
}
