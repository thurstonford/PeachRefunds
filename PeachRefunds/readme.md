# Peach Payments Transaction Refunds

Process refunds for transactions submitted via the [Peach Payments payment gateway](https://www.peachpayments.com/)

## Getting Started

Install this NuGet package and the `Microsoft.Extensions.Logging.Abstractions` package into your .NET Core 6 or .NET Framework 4.8 application.

Package Manager:  
       `Install-Package COGWare.PeachRefunds -Version <version>`  
       `Install-Package  Microsoft.Extensions.Logging.Abstractions`
CLI:  
       `dotnet add package --version <version> COGWare.PeachRefunds` 
       `dotnet add package Microsoft.Extensions.Logging.Abstractions`   
    

## Add Config
Add the PeachRefunds.IsProduction key to your settings file, eg:  
       `<add key="PeachRefunds.IsProduction" value="false" />`  


## Usage
### With logging:
        // Create your logger from the logging framework's extensions package (in this case, NLog)
        var logger = LoggerFactory.Create(builder => builder.AddNLog()).CreateLogger<Program>();

        logger.LogInformation("starting");

        try {
            RefundResult? refundResult = await PeachPayments.RefundHelper.ProcessRefund(
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
                logger);

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
        } catch(Exception ex) {
            Console.WriteLine("ERROR: " + ex.GetBaseException().Message);
        }

        logger.LogInformation("complete");
    
### Without logging:
**Note**: Even though you have opted not log, your project will still require a reference to `Microsoft.Extensions.Logging.Abstractions`.  

        try {
            RefundResult? refundResult = await PeachPayments.RefundHelper.ProcessRefund(
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
        } catch(Exception ex) {
            Console.WriteLine("ERROR: " + ex.GetBaseException().Message);
        }

### Response object:
#### RefundResult
An object representing the response from the Peach Payments API.

- `Id`:(String) - The Peach Payments transaction identifier for this specific refund.
- `ReferenceId`: (String) - The original payment transaction identifier.
- `PaymentType`: (String) - This will be "RF", denoting a refund.
- `Amount`: (String) - The amount that was refunded.
- `Currency`: (String) - The currency that the refund was processed in.
- `Descriptor`: (String) - The merchant payment description.
- `MerchantTransactionId` (String) - The original merchant transaction id.
- `Result` (Object) - An object containing the refund response result.
    - `Code` (String) - The Peach Payments [response code](https://developer.peachpayments.com/docs/checkout-response-codes).
    - `Description` (String) - Description associated with the response code.
- `ResultDetail` (Object) - An object containing the technical details of the refund
    - `ConnectorTxID1` (String) - Downstream (credit card issuer) connection transaction identifier.
    - `ConnectorTxID2` (String) - Additional downstream (credit card issuer) connection transaction identifier.
    - `AuthorisationCode` (String) - Downstream (credit card issuer) authorization code.
    - `AcquirerReference` (String) - Downstream (credit card issuer) reference.
    - `AcquirerResponse` (String) - Downstream (credit card issuer) response.
- `BuildNumber` (String) - Peach Payments refund module build number.
- `Timestamp` (DateTime) - When this refund was processed.
- `NDC` (String) - No idea, will find out.
- `IsSuccessful` (Boolean) - A flag indicating if the refund was successfully processed.

## Logging:
Implements Microsoft.Extensions.Logging, so log away with any compatible logging framework,eg: [NLog](https://github.com/NLog/NLog.Extensions.Logging)
This package logs at DEBUG level.

## Additional documentation

- [Peach Payments refund API documentation](https://developer.peachpayments.com/docs/checkout-refund) 
- [Peach Payments refund response codes](https://developer.peachpayments.com/docs/payments-api-result-codes)

## Feedback

I welcome comments, suggestions, feature requests and even honest criticism :)  

 
- [Github Repo](https://github.com/thurstonford?tab=repositories)  
- Email: lance@cogware.co.za

## Want to show your appreciation?
That's mighty generous - thank you!  
[Buy me a coffee](https://www.buymeacoffee.com/cogware)