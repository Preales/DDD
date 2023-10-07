using Application.Customers.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.API.Controllers.Base;

namespace Web.API.Controllers;

[Route("api/[controller]")]
public class CustomersController : ApiController
{
    private readonly ISender _meditor;

    public CustomersController(ISender meditor)
    {
        _meditor = meditor ?? throw new ArgumentNullException(nameof(meditor));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command)
    {
        var createCustomerResult = await _meditor.Send(command);

        return createCustomerResult.Match(
            customer => Ok(),
            errors => Problem(errors)
        );
    }
}