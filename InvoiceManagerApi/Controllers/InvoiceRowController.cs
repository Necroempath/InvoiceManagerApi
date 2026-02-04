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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceRowResponseDto>>> GetAll()
    {
        var invoiceRows = await _service.GetAllAsync();

        return Ok(invoiceRows);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceRowResponseDto>> GetById(int id)
    {
        var invoiceRow = await _service.GetByIdAsync(id);

        if (invoiceRow is null)
            return NotFound($"InvoiceRow by given id {id} not found");

        return Ok(invoiceRow);
    }

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


    [HttpDelete("hard/{id}")]
    public async Task<ActionResult> DeleteHard(int id)
    {
        bool isDeleted = await _service.DeleteHardAsync(id);

        if (!isDeleted)
            return NotFound($"InvoiceRow by given id {id} not found");

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<InvoiceRowResponseDto>> Update(int id, [FromBody] InvoiceRowUpdateRequest request)
    {
        var invoiceRow = await _service.UpdateAsync(id, request);

        if (invoiceRow is null)
            return NotFound($"InvoiceRow by given id {id} not found");

        return Ok(invoiceRow);
    }
}
