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

    /// <summary>
    /// Retrieves all customers.
    /// </summary>
    /// <remarks>
    /// Returns a list of all customers that are not soft-deleted.
    /// </remarks>
    /// <returns>
    /// A list of customers.
    /// </returns>
    /// <response code="200">Customers were successfully retrieved.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetAll()
    {
        var customers = await _service.GetAllAsync();

        return Ok(customers);
    }

    /// <summary>
    /// Retrieves a customer by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <returns>
    /// The requested customer.
    /// </returns>
    /// <response code="200">Customer was successfully retrieved.</response>
    /// <response code="404">Customer with the specified id was not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> GetById(int id)
    {
        var customer = await _service.GetByIdAsync(id);

        if (customer is null)
            return NotFound($"Customer by given id {id} not found");

        return Ok(customer);
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="request">Customer creation data.</param>
    /// <returns>
    /// The newly created customer.
    /// </returns>
    /// <response code="201">Customer was successfully created.</response>
    /// <response code="400">The request body is invalid.</response>
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

    /// <summary>
    /// Soft deletes a customer.
    /// </summary>
    /// <remarks>
    /// Marks the customer as deleted without removing it from the database.
    /// </remarks>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <response code="200">Customer was successfully soft deleted.</response>
    /// <response code="404">Customer with the specified id was not found.</response>
    [HttpDelete("soft/{id}")]
    public async Task<ActionResult> DeleteSoft(int id)
    {
        bool isDeleted = await _service.DeleteSoftAsync(id);

        if (!isDeleted)
            return NotFound($"Customer by given id {id} not found");

        return Ok();
    }

    /// <summary>
    /// Permanently deletes a customer.
    /// </summary>
    /// <remarks>
    /// Completely removes the customer from the database.
    /// This operation is irreversible.
    /// </remarks>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <response code="200">Customer was successfully permanently deleted.</response>
    /// <response code="404">Customer with the specified id was not found.</response>
    [HttpDelete("hard/{id}")]
    public async Task<ActionResult> DeleteHard(int id)
    {
        bool isDeleted = await _service.DeleteHardAsync(id);

        if (!isDeleted)
            return NotFound($"Customer by given id {id} not found");

        return Ok();
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <param name="request">Updated customer data.</param>
    /// <returns>
    /// The updated customer.
    /// </returns>
    /// <response code="200">Customer was successfully updated.</response>
    /// <response code="404">Customer with the specified id was not found.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> Update(int id, [FromBody] CustomerUpdateRequest request)
    {
        var customer = await _service.UpdateAsync(id, request);

        if (customer is null)
            return NotFound($"Customer by given id {id} not found");

        return Ok(customer);
    }
}
