using InvoiceManagerApi.DTOs.CustomerDTOs;
using InvoiceManagerApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _service;

    public CustomerController(ICustomerService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetAll()
    {
        var customers = await _service.GetAllAsync();

        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> GetById(int id)
    {
        var customer = await _service.GetByIdAsync(id);

        if (customer is null)
            return NotFound($"Customer by given id {id} not found");

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponseDto>> Create([FromBody] CustomerCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var customer = await _service.CreateAsync(request);

        return CreatedAtAction(
                    nameof(GetById),
                    new { id = customer.Id },
                    customer);
    }

    [HttpDelete("soft/{id}")]
    public async Task<ActionResult> DeleteSoft(int id)
    {
        bool isDeleted = await _service.DeleteSoftAsync(id);

        if (!isDeleted)
            return NotFound($"Customer by given id {id} not found");

        return Ok();
    }

    [HttpDelete("hard/{id}")]
    public async Task<ActionResult> DeleteHard(int id)
    {
        bool isDeleted = await _service.DeleteHardAsync(id);

        if (!isDeleted)
            return NotFound($"Customer by given id {id} not found");

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> Update(int id, [FromBody] CustomerUpdateRequest request)
    {
        var customer = await _service.UpdateAsync(id, request);

        if(customer is null)
            return NotFound($"Customer by given id {id} not found");

        return Ok(customer);
    }
}
