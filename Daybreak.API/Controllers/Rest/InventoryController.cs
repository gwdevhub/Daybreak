using Daybreak.API.Services;
using Daybreak.Shared.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Net.Sdk.Web;

namespace Daybreak.API.Controllers.Rest;

[GenerateController("api/v1/rest/inventory")]
public sealed class InventoryController(
    InventoryService inventoryService)
{
    private readonly InventoryService inventoryService = inventoryService;

    [GenerateGet]
    [EndpointName("GetInventory")]
    [EndpointSummary("Get Inventory information")]
    [EndpointDescription("Get the inventory of the current player. Returns a serialized InventoryInformation object")]
    [ProducesResponseType<InventoryInformation>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IResult> GetInventory(CancellationToken cancellationToken)
    {
        var inventoryInformation = await this.inventoryService.GetInventoryInformation(cancellationToken);
        return inventoryInformation is null ?
            Results.StatusCode(StatusCodes.Status503ServiceUnavailable) :
            Results.Ok(inventoryInformation);
    }
}
