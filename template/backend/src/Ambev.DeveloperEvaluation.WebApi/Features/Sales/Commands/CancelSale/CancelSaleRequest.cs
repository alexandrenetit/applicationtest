using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Commands.CancelSale
{
    /// <summary>
    /// Request model for canceling a sale.
    /// </summary>
    public class CancelSaleRequest
    {
        /// <summary>
        /// The unique identifier of the sale to cancel.
        /// </summary>
        [FromRoute(Name = "id")]
        public Guid Id { get; set; }
    }
}
