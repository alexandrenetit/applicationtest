using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Queries.GetSale
{
    /// <summary>
    /// Request model for getting a sale by ID.
    /// </summary>
    public class GetSaleRequest
    {
        /// <summary>
        /// The unique identifier of the sale to retrieve.
        /// </summary>
        [FromRoute(Name = "id")]
        public Guid Id { get; set; }
    }
}