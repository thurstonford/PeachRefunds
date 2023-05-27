using Microsoft.Extensions.Logging;
using PeachPayments.Helpers;
using PeachPayments.Models;
using RestSharp;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PeachPayments
{
    public static class RefundHelper
    {
        private static readonly bool PP_IS_PRODUCTION = Load();
        private static readonly string? CALLING_ASSEMBLY_NAME = Assembly.GetEntryAssembly()?.GetName().Name;
        private static readonly string? PACKAGE = Assembly.GetExecutingAssembly().GetName().Name!;        

        private static bool Load() {
            var isProduction = ConfigurationManager.AppSettings["PeachRefunds.IsProduction"];
            if(string.IsNullOrEmpty(isProduction)) {
                throw new ConfigurationErrorsException("'PeachRefunds.IsProduction' configuration key not found. Refer package readme on NuGet for details.");
            } 

            return Boolean.Parse(isProduction);
        }

        /// <summary>
        /// Submits the refund to the Peach Payments API for processing.
        /// </summary>
        /// <param name="refundConfig">An object representing your Peach Payments configuraion.</param>
        /// <param name="refund">An object representing the refund to be processed.</param>
        /// <param name="logger">Optional logger.</param>
        /// <returns>RefundResult</returns>
        public static async Task<RefundResult?> ProcessRefund(
           RefundConfig refundConfig,
           Refund refund,
           ILogger? logger = null) {

            ValidateRefundConfig(refundConfig);
            ValidateRefund(refund);

            // Basic metrics to determine usage, run on separate thread so
            // we don't hold up the main functionality.
            _ = Task.Run(async () => {
                await SendMetrics();
            });

            Console.WriteLine("processing refundId " + refund.Id);

            RefundResult? refundResult =
                    await PeachPaymentsHelper.ProcessRefund(
                        refundConfig.EntityId!,
                        refundConfig.Secret!,
                        refund.Amount,
                        refund.Currency!,
                        refund.TransactionId!,
                        logger,
                        PP_IS_PRODUCTION);

            if(refundResult != null && refundResult.Result != null) {
                if(refundResult.IsSuccessful) {
                    logger?.LogDebug("RefundId " + refund.Id +
                        " processed successfully, reference " + refundResult.Id);

                } else {
                    logger?.LogDebug("RefundId " + refund.Id + " failed: " +
                        refundResult.Result.Code + ", " + refundResult.Result.Description);
                }
            } else {
                logger?.LogDebug("Invalid response from endpoint");
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

        private static async Task SendMetrics() {
            try {
                var restClient = new RestClient("https://cogware.co.za/");                
                var restRequest = new RestRequest("Metrics", Method.Get);

                //Machine(host) name
                restRequest.AddParameter("m", Environment.MachineName);                
                //Source
                restRequest.AddParameter("s", CALLING_ASSEMBLY_NAME);
                //Package
                restRequest.AddParameter("p", PACKAGE);
                //Instance (PRD/TEST)
                restRequest.AddParameter("v", PP_IS_PRODUCTION ? "PRD" : "TEST");                

                await restClient.ExecuteGetAsync(restRequest);
            } catch(Exception) {
                //do nothing                                
            }
        }
    }
}