using InvoiceManagerApi.DTOs.InvoiceDTOs;
using InvoiceManagerApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _service;

    public InvoiceController(IInvoiceService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves all invoices.
    /// </summary>
    /// <remarks>
    /// Returns a list of all invoices that are not soft-deleted.
    /// </remarks>
    /// <returns>
    /// A list of invoices.
    /// </returns>
    /// <response code="200">Invoices were successfully retrieved.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceResponseDto>>> GetAll()
    {
        var invoices = await _service.GetAllAsync();

        return Ok(invoices);
    }

    /// <summary>
    /// Retrieves an invoice by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <returns>
    /// The requested invoice.
    /// </returns>
    /// <response code="200">Invoice was successfully retrieved.</response>
    /// <response code="404">Invoice with the specified id was not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceResponseDto>> GetById(int id)
    {
        var invoice = await _service.GetByIdAsync(id);

        if (invoice is null)
            return NotFound($"Invoice by given id {id} not found");

        return Ok(invoice);
    }

    /// <summary>
    /// Retrieves invoices by related customer's identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the related customer.</param>
    /// <returns>
    /// Returns a list of all invoices that are not soft-deleted.
    /// </returns>
    /// <response code="200">Invoices were successfully retrieved.</response>
    [HttpGet("/customerId/{customerId}")]
    public async Task<ActionResult<IEnumerable<InvoiceResponseDto>>> GetByCustomerId(int customerId)
    {
        var invoices = await _service.GetByCustomerIdAsync(customerId);

        return Ok(invoices);
    }

    /// <summary>
    /// Creates a new invoice.
    /// </summary>
    /// <param name="request">Invoice creation data.</param>
    /// <returns>
    /// The newly created invoice.
    /// </returns>
    /// <response code="201">Invoice was successfully created.</response>
    /// <response code="400">
    /// The request body is invalid or the related customer was not found.
    /// </response>
    [HttpPost]
    public async Task<ActionResult<InvoiceResponseDto?>> Create([FromBody] InvoiceCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var invoice = await _service.CreateAsync(request);

        if (invoice is null) return BadRequest("Customer by given ID not found");

        return CreatedAtAction(
                    nameof(GetById),
                    new { id = invoice.Id },
                    invoice);
    }

    /// <summary>
    /// Soft deletes an invoice.
    /// </summary>
    /// <remarks>
    /// Marks the invoice as deleted without removing it from the database.
    /// </remarks>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <response code="200">Invoice was successfully soft deleted.</response>
    /// <response code="404">Invoice with the specified id was not found.</response>
    [HttpDelete("soft/{id}")]
    public async Task<ActionResult> DeleteSoft(int id)
    {
        bool isDeleted = await _service.DeleteSoftAsync(id);

        if (!isDeleted)
            return NotFound($"Invoice by given id {id} not found");

        return Ok();
    }

    /// <summary>
    /// Permanently deletes an invoice.
    /// </summary>
    /// <remarks>
    /// Completely removes the invoice from the database.
    /// This operation is irreversible.
    /// </remarks>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <response code="200">Invoice was successfully permanently deleted.</response>
    /// <response code="404">Invoice with the specified id was not found.</response>
    [HttpDelete("hard/{id}")]
    public async Task<ActionResult> DeleteHard(int id)
    {
        bool isDeleted = await _service.DeleteHardAsync(id);

        if (!isDeleted)
            return NotFound($"Invoice by given id {id} not found");

        return Ok();
    }

    /// <summary>
    /// Updates an existing invoice.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <param name="request">Updated invoice data.</param>
    /// <returns>
    /// The updated invoice.
    /// </returns>
    /// <response code="200">Invoice was successfully updated.</response>
    /// <response code="404">Invoice with the specified id was not found.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<InvoiceResponseDto>> Update(int id, [FromBody] InvoiceUpdateRequest request)
    {
        var invoice = await _service.UpdateAsync(id, request);

        if (invoice is null)
            return NotFound($"Invoice by given id {id} not found");

        return Ok(invoice);
    }

    /// <summary>
    /// Changes the status of an existing invoice.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <param name="request">Invoice status change data.</param>
    /// <returns>
    /// The invoice with the updated status.
    /// </returns>
    /// <response code="200">Invoice status was successfully updated.</response>
    /// <response code="400">
    /// Either the invoice id or the provided status is invalid.
    /// </response>
    [HttpPatch("{id}")]
    public async Task<ActionResult<InvoiceResponseDto>> StatusChange(int id, [FromBody] InvoiceStatusChangeRequest request)
    {
        var invoice = await _service.StatusChangeAsync(id, request);

        if (invoice is null) return BadRequest("Either ID or Status is incorrect");

        return Ok(invoice);
    }


}
