using FreeDOW.API.Core.Abstract;
using FreeDOW.API.Core.Entities;
using FreeDOW.API.WebHost.Authentication;
using FreeDOW.API.WebHost.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeDOW.API.WebHost.Controllers
{
    /// <summary>
    /// API Controller for Equipment management
    /// could be apply only for current organization
    /// organzationId take from JWTToken
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    //[Authorize(AuthenticationSchemes = AuthSchemas.Jwt)]
    public class EquipmentController : ControllerBase
    {
        private readonly IOrgRepository<Equipment> _equipRepo;
        public EquipmentController(IOrgRepository<Equipment> eq)
        {
            _equipRepo = eq;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentResponce>>> GetEquipmentsAsync()
        {
            var orgClaim = User.Claims.FirstOrDefault(cl => cl.Type == "OrganizationId");
            if (null == orgClaim) return BadRequest("organizationId not defined");
            Guid orgId;
            try { orgId = Guid.Parse(orgClaim.Value); } catch { return BadRequest("wrong organizationId"); };

            var res = await _equipRepo.GetByConditionAsync(rec => rec.OrganizationId == orgId);
            var eqL = res.Select(rec => new EquipmentResponce()
            {
                Id = rec.Id,
                ParentId = rec.ParentId,
                Name = rec.Name,
            });
            return Ok(eqL);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentResponce>> GetEquipmentAsync(Guid id)
        {
            var res = await _equipRepo.GetByIdAsync(id);
            if (null == res) return NotFound();
            return Ok(new EquipmentResponce()
            {
                Id = res.Id,
                ParentId = res.ParentId,
                Name = res.Name,
            });
        }

        [HttpPost("{request}")]
        public async Task<ActionResult<EquipmentResponce>> CreateEquipmentAsync(CreateOreditEquipment request)
        {
            var orgClaim = User.Claims.FirstOrDefault(cl => cl.Type == "OrganizationId");
            if (null == orgClaim) return BadRequest("organizationId not defined");
            Guid orgId;
            try { orgId = Guid.Parse(orgClaim.Value); } catch { return BadRequest("wrong organizationId"); };
            if (Guid.Empty != request.ParentId)
            {
                var pEq = await _equipRepo.GetByIdAsync(request.Id);
                if (null == pEq) return BadRequest("parent equipment not found");
                if (pEq.OrganizationId != orgId) return BadRequest("wrong organizationId");
            }
            var equip = new Equipment()
            {
                Name = request.Name,
                Id = request.Id,
                ParentId = request.ParentId,
                OrganizationId = orgId
            };
            equip = await _equipRepo.CreateAsync(equip);
            return Ok(new EquipmentResponce()
            {
                Id = equip.Id,
                ParentId = equip.ParentId,
                Name = equip.Name,
            });
        }

        [HttpPut("{request}")]
        public async Task<ActionResult> UpdateEquipmentAsync(CreateOreditEquipment request)
        {
            var orgClaim = User.Claims.FirstOrDefault(cl => cl.Type == "OrganizationId");
            if (null == orgClaim) return BadRequest("organizationId not defined");
            Guid orgId;
            try { orgId = Guid.Parse(orgClaim.Value); } catch { return BadRequest("wrong organizationId"); };
            var equip = await _equipRepo.GetByIdAsync(request.Id);
            if (null == equip) return NotFound();
            if (orgId != equip.OrganizationId) return BadRequest();
            await _equipRepo.UpdateAsync(equip);
            return Ok();
        }

        /// <summary>
        ///  entity isn't physically deleted, just change IsDeleted property for entity
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteEquipmentAsync(Guid Id)
        {
            var orgClaim = User.Claims.FirstOrDefault(cl => cl.Type == "OrganizationId");
            if (null == orgClaim) return BadRequest("organizationId not defined");
            Guid orgId;
            try { orgId = Guid.Parse(orgClaim.Value); } catch { return BadRequest("wrong organizationId"); };
            var equip = await _equipRepo.GetByIdAsync(Id);
            if (null == equip) return NotFound();
            if (orgId != equip.OrganizationId) return BadRequest();
            equip.IsDeleted = true;
            await _equipRepo.UpdateAsync(equip);
            return Ok();
        }
    }
}
