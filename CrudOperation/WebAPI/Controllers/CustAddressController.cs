using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("CustomerAddress")]
    public class CustAddressController : Controller
    {
        private CustomerEntities.CustomerEntities _entities;

        public CustAddressController(CustomerEntities.CustomerEntities entities)
        {
            _entities = entities;
        }

        [HttpGet("hello")]
        public string Get()
        {
            return "Hello From CustomerAddressController";
        }
        // Implemenation of get method
        [HttpGet("getAllAddress")]
        public async Task<IActionResult> GetAllAddress()
        {
            return Ok(await _entities.CustAddress.ToListAsync());
        }

        // Implementation fo Delete Method
        [HttpDelete("DeleteAddress/{AddressId}")]
        public async Task<IActionResult> DeleteAddress([FromRoute] int AddressId)
        {
            var address = await _entities.CustAddress.FindAsync(AddressId);
            if (address != null)
            {
                _entities.Remove(address);
                await _entities.SaveChangesAsync();
                return Ok(address);
            }
            return NotFound();
        }

    }
}
