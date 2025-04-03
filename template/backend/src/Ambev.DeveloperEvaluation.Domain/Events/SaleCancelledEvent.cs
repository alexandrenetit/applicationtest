using System;
using System.Text.Json.Serialization;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    /// <summary>
    /// Represents an event when a sale is cancelled
    /// </summary>
    public class SaleCancelledEvent
    {
        /// <summary>
        /// The unique identifier of the cancelled sale
        /// </summary>
        public Guid SaleId { get; set; }

        /// <summary>
        /// The sale number/identifier
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// The status of the sale (should be Cancelled)
        /// </summary>
        public SaleStatus Status { get; set; }

        /// <summary>
        /// The timestamp when cancellation occurred
        /// </summary>
        public DateTime CancelledAt { get; set; }

        /// <summary>
        /// The reason for cancellation (optional)
        /// </summary>
        public string? CancellationReason { get; set; }

        // Parameterless constructor for serialization
        public SaleCancelledEvent()
        {
            CancelledAt = DateTime.UtcNow;
            Status = SaleStatus.Cancelled;
        }

        [JsonConstructor]
        public SaleCancelledEvent(
            Guid saleId,
            string saleNumber,
            SaleStatus status,
            DateTime cancelledAt,
            string? cancellationReason = null)
        {
            SaleId = saleId;
            SaleNumber = saleNumber;
            Status = status;
            CancelledAt = cancelledAt;
            CancellationReason = cancellationReason;
        }

        /// <summary>
        /// Creates a SaleCancelledEvent from a Sale entity
        /// </summary>
        public static SaleCancelledEvent CreateFrom(Sale sale, string? cancellationReason = null)
        {
            if (sale == null)
                throw new DomainException(nameof(sale));

            if (sale.Status != SaleStatus.Cancelled)
                throw new DomainException("Cannot create cancellation event for non-cancelled sale");

            return new SaleCancelledEvent(
                sale.Id,
                sale.SaleNumber,
                sale.Status,
                DateTime.UtcNow,
                cancellationReason);
        }
    }
}