using Microsoft.AspNetCore.Mvc;

namespace ConsumerComplaints.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        public class DummyDto
        {
            public string? Message { get; set; }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] DummyDto dto)
        {
            if (id == 123 && dto?.Message != null)
            {
                return Ok(new { status = "updated", id, message = dto.Message });
            }

            return BadRequest("Invalid input.");
        }
    }
}