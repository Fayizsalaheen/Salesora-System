using Microsoft.AspNetCore.Mvc;
using Backend_Salesora_System.DTO;
namespace Backend_Salesora_System.Controllers
{
     
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseInvoiceController : ControllerBase
    {
        private readonly PurchaseInvoiceService _service;

        public PurchaseInvoiceController(PurchaseInvoiceService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] PurchaseInvoiceDto dto)
        {
            var (success, message) = await _service.CreateInvoiceAsync(dto);
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }
    }
}
