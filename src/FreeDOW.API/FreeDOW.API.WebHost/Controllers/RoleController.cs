using FreeDOW.API.Core.Abstract;
using FreeDOW.API.Core.Entities;
using FreeDOW.API.WebHost.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeDOW.API.WebHost.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IOrgRepository<Role> _repRole;
        public RoleController(IOrgRepository<Role> repRole)
        {
            _repRole = repRole;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<RoleResponce>>> RolesAsync()
        {
            var roles = await _repRole.GetAllAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleResponce>> RoleAsync(Guid id)
        {
            var role = await _repRole.GetByIdAsync(id);
            if (null == role) return NotFound();
            return Ok(role);
        }
    }
}
