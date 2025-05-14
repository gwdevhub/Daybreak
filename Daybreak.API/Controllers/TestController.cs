using Net.Sdk.Web;

namespace Daybreak.API.Controllers;

[GenerateController("api/test")]
public sealed class TestController
{
    [GenerateGet("testing")]
    public IResult GetTest(CancellationToken token)
    {
        return Results.Text("Hello from injected ASP-NET Core!", "text/plain");
    }
}
