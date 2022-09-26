using FreeDOW.API.Core.Abstract;
using FreeDOW.API.Core.Entities;
using FreeDOW.API.WebHost.Authentication;
using FreeDOW.API.WebHost.Helpers;
using FreeDOW.API.WebHost.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeDOW.API.WebHost.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    //[Authorize(AuthenticationSchemes =AuthSchemas.Jwt)]
    public class EmployeeController : ControllerBase
    {
        private readonly IOrgRepository<Employee> _repoEmpl;
        private readonly IOrgRepository<OrgStruct> _repoOrgStr;
        private readonly IOrgRepository<Organization> _repoOrg;
        private readonly IOrgRepository<RegUserEntry> _repoReg;
        private readonly IOrgRepository<Role> _repoRole;
        private readonly IOrgRepository<Equipment> _repoEquip;
        private readonly IConfiguration _config;

        public EmployeeController(IConfiguration config, IOrgRepository<Employee> repoEmpl, IOrgRepository<OrgStruct> repoOrgstr,
            IOrgRepository<Organization> repoOrg, IOrgRepository<RegUserEntry> repoReg, IOrgRepository<Role> repoRole,
            IOrgRepository<Equipment> repoEquip)
        {
            _repoEmpl = repoEmpl;
            _config = config;
            _repoOrgStr = repoOrgstr;
            _repoOrg = repoOrg;
            _repoReg = repoReg;
            _repoRole = repoRole;
            _repoEquip = repoEquip;
        }

        /// <summary>
        /// Get employees for the user's orgstruct 
        /// need claim 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Employees")]
        public async Task<ActionResult<List<EmployeeResponce>>> GetEmployeesAsync()
        {
            var orgStrClaim = User.Claims.FirstOrDefault(cl => cl.Type == "OrgStructId");
            if (null == orgStrClaim) return BadRequest("organiation structure's not defined");
            Guid orgStrId;
            try { orgStrId = Guid.Parse(orgStrClaim.Value); } catch { return BadRequest("wrong organization stucture Id"); };
            var Ids = await OrganizationLists.GetChildOrgStructIds(orgStrId, _repoOrgStr);
            var emps = await _repoEmpl.GetByConditionAsync(rec => Ids.Contains(rec.OrgStructId));
            if (null == emps) return NotFound();
            var res = emps.Select(async rec => new EmployeeResponce()
            {
                Id = rec.Id,
                FirstName = rec.FirstName,
                LastName = rec.LastName,
                Email = rec.Email,
                InviteName = rec.InviteName,
                OrgStrId = rec.OrgStructId,
                OrgStrName = (await _repoOrgStr.GetByIdAsync(rec.OrgStructId)).Name
            }).ToList();
            return Ok(res);
        }

        [HttpGet("Employee/{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeAsync(Guid id)
        {
            var emp = await _repoEmpl.GetByIdAsync(id);
            if (null == emp) return NotFound();
            var orgStrClaim = User.Claims.FirstOrDefault(cl => cl.Type == "OrgStructId");
            if (null == orgStrClaim) return BadRequest("organiation structure's not defined");
            Guid orgStrId;
            try { orgStrId = Guid.Parse(orgStrClaim.Value); } catch { return BadRequest("wrong organization stucture Id"); };
            var Ids = await OrganizationLists.GetChildOrgStructIds(orgStrId, _repoOrgStr);
            if (!Ids.Contains(emp.OrgStructId)) return NotFound();
            var res = new EmployeeResponce()
            {
                Id = emp.Id,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Email = emp.Email,
                InviteName = emp.InviteName,
                OrgStrId = emp.OrgStructId,
                OrgStrName = (await _repoOrgStr.GetByIdAsync(emp.OrgStructId)).Name
            };
            return Ok(res);
        }

        /// <summary>
        /// Get all employees for the user organization and and its childs only
        /// Admin role is to have
        /// </summary>
        /// <returns></returns>
        [HttpGet("MgtEmployees")]
        public async Task<ActionResult<IEnumerable<EmployeeResponce>>> MgtGetEmployeesAsync()
        {
            var orgClaim = User.Claims.FirstOrDefault(cl => cl.Type == "OrganizationId");
            if (null == orgClaim) return null;
            Guid orgId;
            try { orgId = Guid.Parse(orgClaim.Value); } catch { return BadRequest("wrong organizationId"); };
            var orgs = await OrganizationLists.GetOrgIds(orgId, _repoOrg);
            if (null == orgs) return BadRequest("No organizations linked to the user");
            List<Guid> orgOSIds = new List<Guid>();
            foreach (var Id in orgs)
            {
                var osids = await OrganizationLists.GetOrgStructIds(Id, _repoOrgStr);
                if (osids.Any()) orgOSIds.AddRange(osids);
            }
            var empl = await _repoEmpl.GetByConditionAsync(rec => orgOSIds.Contains(rec.OrgStructId));
            var res = empl.Select(async rec => new EmployeeResponce()
            {
                Id = rec.Id,
                FirstName = rec.FirstName,
                LastName = rec.LastName,
                Email = rec.Email,
                InviteName = rec.InviteName,
                OrgStrId = rec.OrgStructId,
                OrgStrName = (await _repoOrgStr.GetByIdAsync(rec.OrgStructId)).Name
            }).ToList();
            return Ok(res);

        }

        
        /// <summary>
        /// Get Employee by Id search in user organization and it's childs only 
        /// Admin role is to have
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("MgtEmployee/{id}")]
        public async Task<ActionResult<Employee>> MgtGetEmployeeAsync(Guid id)
        {
            var orgClaim = User.Claims.FirstOrDefault(cl => cl.Type == "OrganizationId");
            if (null == orgClaim) return BadRequest("organizationId not defined");
            Guid orgId;
            try { orgId = Guid.Parse(orgClaim.Value); } catch { return BadRequest("wrong organizationId"); };
            var emp = await _repoEmpl.GetByIdAsync(id);
            if (null == emp) return NotFound();
            var res = new EmployeeResponce()
            {
                Id = emp.Id,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Email = emp.Email,
                InviteName = emp.InviteName,
                OrgStrId = emp.OrgStructId,
                OrgStrName = (await _repoOrgStr.GetByIdAsync(emp.OrgStructId)).Name
            };
            var orgstr = await _repoOrgStr.GetByIdAsync(emp.OrgStructId);
            if (null == orgstr) return Ok(res); // not linked employee
            var eorgs = await OrganizationLists.GetOrgIds(orgId, _repoOrg);
            if (eorgs.Contains(orgId)) return Ok(res);
            // employee doesn't belong to user organizations 
            return NotFound();
        }

        
        
    }
}
