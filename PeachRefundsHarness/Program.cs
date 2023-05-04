using PeachPayments.Models;

internal class Program
{
    static async Task Main(string[] args) {
        //await PeachPayments.RefundHelper.ProcessRefunds();
        try {
            RefundResult? refundResult = await PeachPayments.RefundHelper.ProcessRefund(
                // An instance of the RefundConfig object containing details of the
                // Peach Payments entity that the original transaction was processed against.
                // These details are available to the merchant in the Peach Payments Console. 
                new RefundConfig() {
                    // Merchant's entityId.
                    EntityId = "8ac7a4ca7279acb401727a24eb48013b",
                    // Merchant's secret.
                    Secret = "3e5fdfdda59511ea93d502d14de18c0c"
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
                    TransactionId = "8ac7a4a28793c9d50187945b84dc5cf9"
                });

            // Checks the status of the refund against the API response codes to 
            // determine if the refund can be considered successful.
            if(refundResult!.IsSuccessful) {
                // Update the status of the refund in your refund system.
                Console.WriteLine("Refund processed successfully! " + 
                    "Refund reference: '" + refundResult.Id + "'");
            } else {
                // Update the status of the refund in your refund system, optionally including the
                // return code and description for analysis and manual resolution (if necessary).
                Console.WriteLine("Refund failed: " + 
                    refundResult.Result!.Code + ", " + 
                    refundResult.Result!.Description);
            }
        } catch (Exception ex) {
            Console.WriteLine("ERROR: " + ex.Message);
        }

        Console.ReadLine();
    }
}