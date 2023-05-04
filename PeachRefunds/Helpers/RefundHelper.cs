using PeachPayments.Helpers;
using PeachPayments.Models;

namespace PeachPayments
{
    public static class RefundHelper
    {
        public static async Task<RefundResult?> ProcessRefund(
            RefundConfig refundConfig, 
            Refund refund) {

            ValidateRefundConfig(refundConfig);
            ValidateRefund(refund);

            Logger.Debug("processing refundId " + refund.Id);

            RefundResult? refundResult =
                    await PeachPaymentsHelper.ProcessRefund(
                        refundConfig.EntityId!,
                        refundConfig.Secret!,
                        refund.Amount,
                        refund.Currency!,
                        refund.TransactionId!);

            if(refundResult != null && refundResult.Result != null) {
                if(refundResult.IsSuccessful) {
                    Logger.Info("RefundId " + refund.Id +
                        " processed successfully, reference " + refundResult.Id);

                } else {
                    Logger.Error("RefundId " + refund.Id + " failed: " +
                        refundResult.Result.Code +
                        ", " + refundResult.Result.Description);
                }
            } else {
                string peachRefundError = "Invalid response from endpoint";

                Logger.Error(peachRefundError);
            }

            return refundResult;
        }

        public static string? GetDescriptionByStatusCode(string? statusCode) { 
            return (from p in PeachPaymentsHelper.codes 
                    where p.Key == statusCode 
                    select p.Value).FirstOrDefault();
        }

        private static void ValidateRefundConfig(RefundConfig refundConfig) {
            if(refundConfig == null) {
                throw new ArgumentNullException(nameof(refundConfig));
            }

            if(String.IsNullOrEmpty(refundConfig.EntityId)) {
                throw new ArgumentNullException(nameof(refundConfig.EntityId));
            }

            if(String.IsNullOrEmpty(refundConfig.Secret)) {
                throw new ArgumentNullException(nameof(refundConfig.Secret));
            }
        }

        private static void ValidateRefund(Refund refund) {
            if(refund == null) {
                throw new ArgumentNullException(nameof(refund));
            }

            if(refund.Amount <= 0) {
                throw new ArgumentException("Refund amount must be a positive value");
            }

            if(String.IsNullOrEmpty(refund.Currency)) {
                throw new ArgumentNullException(nameof(refund.Currency));
            }

            if(String.IsNullOrEmpty(refund.TransactionId)) {
                throw new ArgumentNullException(nameof(refund.TransactionId));
            }
        }
    }
}