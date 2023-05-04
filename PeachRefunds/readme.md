# Peach Payments Transaction Refunds

Process refunds for transactions submitted via the [Peach Payments payment gateway](https://www.peachpayments.com/)

## Getting Started

Install the standard Nuget package into your ASP.NET Core application.

Package Manager:  
       `Install-Package COGWare.PeachRefunds -Version 1.0.0`  

CLI:  
       `dotnet add package --version 1.0.0 COGWare.PeachRefunds` 
    

## Usage

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
        } catch (Exception ex) {
            Console.WriteLine("ERROR: " + ex.Message);
        }

###Response object:
#### RefundResult
An object representing the response from the Peach Payments API.

- Id:(String) - The Peach Payments transaction identifier for this specific refund.
- ReferenceId: (String) - The original payment transaction identifier.
- PaymentType: (String) - This will be "RF", denoting a refund.
- Amount: (String) - The amount that was refunded.
- Currency: (String) - The currency that the refund was processed in.
- Descriptor: (String) - The merchant payment description.
- MerchantTransactionId (String) - The original merchant transaction id.
- Result (Object) - An object containing the refund response result.
    - Code (String) - The Peach Payments [response code](https://developer.peachpayments.com/docs/checkout-response-codes).
    - Description (String) - Description associated with the response code.
- ResultDetail (Object) - An object containing the technical details of the refund
    - ConnectorTxID1 (String) - Downstream (credit card issuer) connection transaction identifier.
    - ConnectorTxID2 (String) - Additional downstream (credit card issuer) connection transaction identifier.
    - AuthorisationCode (String) - Downstream (credit card issuer) authorization code.
    - AcquirerReference (String) - Downstream (credit card issuer) reference.
    - AcquirerResponse (String) - Downstream (credit card issuer) response.
- BuildNumber (String) - Peach Payments refund module build number.
- Timestamp (DateTime) - When this refund was processed.
- NDC (String) - No idea, will find out.
- IsSuccessful (Boolean) - A flag indicating if the refund was successfully processed.

## Additional documentation

- [Peach Payments refund API documentation](https://developer.peachpayments.com/docs/checkout-refund) 
- [Peach Payments refund response codes](https://developer.peachpayments.com/docs/payments-api-result-codes)

## Feedback

I welcome comments, suggestions, feature requests and even honest criticism :)  
 
- [Github Repo](https://github.com/thurstonford?tab=repositories)  
- [Email](mailto:lance@cogware.co.za)