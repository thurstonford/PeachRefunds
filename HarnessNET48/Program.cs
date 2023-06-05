//using Microsoft.Extensions.Logging;
//using NLog.Extensions.Logging;
using PeachPayments.Models;
using System;
using System.Threading.Tasks;

namespace HarnessNET48
{
    internal class Program
    {
        static async Task Main(string[] args) {
            // Create your logger from the logging framework's extensions package (in this case, NLog)
            //var logger = LoggerFactory.Create(builder => builder.AddNLog()).CreateLogger<Program>();

           // logger.LogInformation("starting");

            try {
                RefundResult refundResult = await PeachPayments.RefundHelper.ProcessRefund(
                    // An instance of the RefundConfig object containing details of the
                    // Peach Payments entity that the original transaction was processed against.
                    // These details are available in the Peach Payments Console. 
                    new RefundConfig() {
                        // Your merchant entityId.
                        EntityId = "xyz123",
                        // Your merchant secret.
                        Secret = "123xyz"
                    },
                    // An instance of a Refund object representing the refund to be processed.
                    new Refund() {
                        // The amount to be refunded.
                        Amount = 1.00,
                        // Currency code.
                        Currency = "ZAR",
                        // Your refund identifier.
                        Id = "1",
                        // The original (Peach Payments) transactionId of the payment.
                        TransactionId = "456xyz"
                    },
                    // optional logger
                    null);

                // Checks the status of the refund against the API response codes to 
                // determine if the refund can be considered successful.
                if(refundResult.IsSuccessful) {
                    // Update the status of the refund in your refund system.
                    Console.WriteLine("Refund processed successfully! " +
                        "Refund reference: '" + refundResult.Id + "'");
                } else {
                    // Update the status of the refund in your refund system, optionally including the
                    // return code and description for analysis and manual resolution (if necessary).
                    Console.WriteLine("Refund failed: " +
                        refundResult.Result.Code + ", " +
                        refundResult.Result.Description);
                }
            } catch(Exception ex) {
                Console.WriteLine("ERROR: " + ex.GetBaseException().Message);
            }

            //logger.LogInformation("complete");

            Console.ReadLine();
        }
    }
}
