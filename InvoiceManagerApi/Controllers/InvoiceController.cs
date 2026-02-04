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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceResponseDto>>> GetAll()
    {
        var invoices = await _service.GetAllAsync();

        return Ok(invoices);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceResponseDto>> GetById(int id)
    {
        var invoice = await _service.GetByIdAsync(id);

        if (invoice is null)
            return NotFound($"Invoice by given id {id} not found");

        return Ok(invoice);
    }

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

    [HttpDelete("soft/{id}")]
    public async Task<ActionResult> DeleteSoft(int id)
    {
        bool isDeleted = await _service.DeleteSoftAsync(id);

        if (!isDeleted)
            return NotFound($"Invoice by given id {id} not found");

        return Ok();
    }

    [HttpDelete("hard/{id}")]
    public async Task<ActionResult> DeleteHard(int id)
    {
        bool isDeleted = await _service.DeleteHardAsync(id);

        if (!isDeleted)
            return NotFound($"Invoice by given id {id} not found");

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<InvoiceResponseDto>> Update(int id, [FromBody] InvoiceUpdateRequest request)
    {
        var invoice = await _service.UpdateAsync(id, request);

        if (invoice is null)
            return NotFound($"Invoice by given id {id} not found");

        return Ok(invoice);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<InvoiceResponseDto>> StatusChange(int id, [FromBody] InvoiceStatusChangeRequest request)
    {
        var invoice = await _service.StatusChangeAsync(id, request);

        if (invoice is null) return BadRequest("Either ID or Status is incorrect");

        return Ok(invoice);
    }

}
