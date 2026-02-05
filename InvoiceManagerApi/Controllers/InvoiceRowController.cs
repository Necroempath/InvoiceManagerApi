using InvoiceManagerApi.DTOs.InvoiceRowDTOs;
using InvoiceManagerApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceRowController : ControllerBase
{
    private readonly IInvoiceRowService _service;

    public InvoiceRowController(IInvoiceRowService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves all invoice rows.
    /// </summary>
    /// <remarks>
    /// Returns a list of all invoice rows.
    /// </remarks>
    /// <returns>
    /// A list of invoice rows.
    /// </returns>
    /// <response code="200">Invoice rows were successfully retrieved.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceRowResponseDto>>> GetAll()
    {
        var invoiceRows = await _service.GetAllAsync();

        return Ok(invoiceRows);
    }

    /// <summary>
    /// Retrieves an invoice row by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice row.</param>
    /// <returns>
    /// The requested invoice row.
    /// </returns>
    /// <response code="200">Invoice row was successfully retrieved.</response>
    /// <response code="404">Invoice row with the specified id was not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceRowResponseDto>> GetById(int id)
    {
        var invoiceRow = await _service.GetByIdAsync(id);

        if (invoiceRow is null)
            return NotFound($"InvoiceRow by given id {id} not found");

        return Ok(invoiceRow);
    }

    /// <summary>
    /// Creates a new invoice row.
    /// </summary>
    /// <param name="request">Invoice row creation data.</param>
    /// <returns>
    /// The newly created invoice row.
    /// </returns>
    /// <response code="201">Invoice row was successfully created.</response>
    /// <response code="400">
    /// The request body is invalid or the related invoice was not found.
    /// </response>
    [HttpPost]
    public async Task<ActionResult<InvoiceRowResponseDto?>> Create([FromBody] InvoiceRowCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var invoiceRow = await _service.CreateAsync(request);

        if (invoiceRow is null) return BadRequest("Invoice by given ID not found");

        return CreatedAtAction(
                    nameof(GetById),
                    new { id = invoiceRow.Id },
                    invoiceRow);
    }

    /// <summary>
    /// Permanently deletes an invoice row.
    /// </summary>
    /// <remarks>
    /// Completely removes the invoice row from the database.
    /// This operation is irreversible.
    /// </remarks>
    /// <param name="id">The unique identifier of the invoice row.</param>
    /// <response code="200">Invoice row was successfully permanently deleted.</response>
    /// <response code="404">Invoice row with the specified id was not found.</response>
    [HttpDelete("hard/{id}")]
    public async Task<ActionResult> DeleteHard(int id)
    {
        bool isDeleted = await _service.DeleteHardAsync(id);

        if (!isDeleted)
            return NotFound($"InvoiceRow by given id {id} not found");

        return Ok();
    }

    /// <summary>
    /// Updates an existing invoice row.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice row.</param>
    /// <param name="request">Updated invoice row data.</param>
    /// <returns>
    /// The updated invoice row.
    /// </returns>
    /// <response code="200">Invoice row was successfully updated.</response>
    /// <response code="404">Invoice row with the specified id was not found.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<InvoiceRowResponseDto>> Update(int id, [FromBody] InvoiceRowUpdateRequest request)
    {
        var invoiceRow = await _service.UpdateAsync(id, request);

        if (invoiceRow is null)
            return NotFound($"InvoiceRow by given id {id} not found");

        return Ok(invoiceRow);
    }

}
