using FreeDOW.API.Core.Abstract;
using FreeDOW.API.Core.Entities;
using FreeDOW.API.WebHost.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeDOW.API.WebHost.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrgStructController : ControllerBase
    {
        private readonly IOrgRepository<OrgStruct> _repoOrgStr;

        public OrgStructController(IOrgRepository<OrgStruct> repoOrgStr)
        {
            _repoOrgStr = repoOrgStr;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrgStructResponce>>> GetOrgStructsAsync()
        {
            var orgClaim = User.Claims.FirstOrDefault(cl => cl.Type == "OrganizationId");
            if (null == orgClaim) return BadRequest("organizationId not defined");
            Guid orgId;
            try { orgId = Guid.Parse(orgClaim.Value); } catch { return BadRequest("wrong organizationId"); };
            var orgStructs = await _repoOrgStr.GetByConditionAsync(rec => rec.OrganizationId == orgId);
            var res = orgStructs.Select(rec => new OrgStructResponce()
            {
                Id = rec.Id,
                ParentId = rec.ParentId,
                Name = rec.Name,
                OrgId = rec.OrganizationId,
            });
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrgStructResponce>> GetOrgStructAsync(Guid id)
        {
            var orgClaim = User.Claims.FirstOrDefault(cl => cl.Type == "OrganizationId");
            if (null == orgClaim) return BadRequest("organizationId not defined");
            Guid orgId;
            try { orgId = Guid.Parse(orgClaim.Value); } catch { return BadRequest("wrong organizationId"); };
            var orgStr = await _repoOrgStr.GetByIdAsync(id);
            if (null == orgStr) return NotFound();
            if (orgStr.OrganizationId != orgId) return BadRequest("wrong organizationId");
            return Ok(new OrgStructResponce()
            {
                Id = orgStr.Id,
                ParentId = orgStr.ParentId,
                Name = orgStr.Name,
                isDeleted = orgStr.IsDeleted,
                OrgId = orgStr.OrganizationId,
            });
        }

        [HttpPost("{request}")]
        public async Task<ActionResult<OrgStructResponce>> CreateOrgStructAsync(CreateOrEditOrgStruct request)
        {
            if (null == request) return BadRequest();
            var orgClaim = User.Claims.FirstOrDefault(cl => cl.Type == "OrganizationId");
            if (null == orgClaim) return BadRequest("organizationId not defined");
            Guid orgId;
            try { orgId = Guid.Parse(orgClaim.Value); } catch { return BadRequest("wrong organizationId"); };
            if (Guid.Empty != request.ParentId)
            {
                var parent = await _repoOrgStr.GetByIdAsync(request.ParentId);
                if (null == parent) return BadRequest("parentId invalid");
                if(parent.OrganizationId != orgId) return BadRequest("wrong organizationId");
            }
            var orgStr = new OrgStruct()
            {
                Id = request.Id,
                ParentId = request.ParentId,
                OrganizationId = orgId,
                Name = request.Name,
            };
            var res = await _repoOrgStr.CreateAsync(orgStr);
            return Ok(new OrgStructResponce()
            {
                Id = res.Id,
                ParentId = res.ParentId,
                Name = res.Name,
                OrgId = res.OrganizationId,
            });
        }

        [HttpPut("{request}")]
        public  async Task<ActionResult> UpdateOrgStructAsync(CreateOrEditOrgStruct request)
        {
            if (null == request) return BadRequest();
            var orgClaim = User.Claims.FirstOrDefault(cl => cl.Type == "OrganizationId");
            if (null == orgClaim) return BadRequest("organizationId not defined");
            Guid orgId;
            try { orgId = Guid.Parse(orgClaim.Value); } catch { return BadRequest("wrong organizationId"); };
            var orgStr = await _repoOrgStr.GetByIdAsync(request.Id);
            if (null == orgStr) return NotFound();
            if (orgStr.OrganizationId != orgId) return BadRequest("wrong organization");
            var entity = new OrgStruct()
            {
                Id = request.Id,
                ParentId = orgStr.ParentId,
                Name = request.Name,
                OrganizationId = orgStr.OrganizationId,
                IsDeleted = false,
            };
            await _repoOrgStr.UpdateAsync(entity);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrgStructAsync(Guid id)
        {
            var orgStr = await _repoOrgStr.GetByIdAsync(id);
            if(null == orgStr) return NotFound();
            var orgClaim = User.Claims.FirstOrDefault(cl => cl.Type == "OrganizationId");
            if (null == orgClaim) return BadRequest("organizationId not defined");
            Guid orgId;
            try { orgId = Guid.Parse(orgClaim.Value); } catch { return BadRequest("wrong organizationId"); };
            if (orgStr.OrganizationId != orgId) return BadRequest("wrong organization");
            orgStr.IsDeleted = true;
            await _repoOrgStr.UpdateAsync(orgStr);
            return Ok();
        }
    }
}
