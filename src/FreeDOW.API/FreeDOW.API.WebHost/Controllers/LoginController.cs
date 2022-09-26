using FreeDOW.API.Core.Abstract;
using FreeDOW.API.Core.Entities;
using FreeDOW.API.Core.Helpers;
using FreeDOW.API.DataAccess.Repositries;
using FreeDOW.API.WebHost.Models;
using FreeDOW.API.WebHost.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace FreeDOW.API.WebHost.Controllers
{
    /// <summary>
    /// user login and new organization schema owner rgistration
    /// 
    [ApiController]
    [Route("api/v1/[controller]")]
    
    public class LoginController : ControllerBase
    {
        private readonly EmployeeRepository _repoEmpl;
        private readonly IOrgRepository<OrgStruct> _repoOrgStr;
        private readonly IOrgRepository<Organization> _repoOrg;
        private readonly IOrgRepository<RegUserEntry> _repoReg;
        private readonly IOrgRepository<Role> _repoRole;
        private readonly IOrgRepository<Equipment> _repoEquip;
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config,EmployeeRepository repoEmpl, IOrgRepository<OrgStruct> repoOrgstr,
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
        /// Login through user login and password, calculate hash using login as salt and 
        /// compare with database value
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<JsonResult>> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            var userL = await _repoEmpl.GetByConditionAsync(rec => rec.Login == loginModel.UserName);
            var user = userL.FirstOrDefault();
            if (null == user) return BadRequest();
            var org = await _repoOrg.GetByIdAsync(user.OrgStruct.OrganizationId);
            try
            {
                var PasswValid = HashedPassword.VerifyHashedPassword(user.Password, loginModel.UserName, loginModel.Password);
                if (!PasswValid) return NotFound();

                var claims = new List<Claim>
                {
                    new Claim("userId",user.Id.ToString()),
                    new Claim("orgStructId", user.OrgStructId.ToString()),
                    new Claim("orgId", user.OrgStruct.OrganizationId.ToString()),
                };
                var jwtServ = new JwtService(_config);
                var token = jwtServ.CreateToken(claims);
                return Ok(new JsonResult(new
                {
                    token= token,
                    user = user.FirstName,
                    userLastName = user.LastName,
                    Orgstr = user.OrgStruct.Name,
                    Organization = org.Name
                }));
            }
            catch { return StatusCode(500); }
        }
        
        /// <summary>
        /// redericted from letter notification to frontend - fill user data
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="emp"></param>
        /// <returns></returns>
        [HttpPost("ConfirmRegister")]
        public async Task<ActionResult> ConfirmRegister([FromQuery] String Key, CreateOrEditEmployee emp)
        {
            var regEntryRec = await _repoReg.GetByConditionAsync(rec => rec.RegCode == Guid.Parse(Key));
            if (null == regEntryRec) return BadRequest();
            var regEntry = regEntryRec.FirstOrDefault();
            if (null == regEntry) return StatusCode(500);
            var roleRec = await _repoRole.GetByConditionAsync(rec => rec.Id == regEntry.RoleId);
            var role = roleRec?.FirstOrDefault();
            if (null == role) return StatusCode(500);

            var empId = Guid.NewGuid();
            var empl = _repoEmpl.CreateAsync(new Employee()
            {
                Id = empId,
                FirstName = regEntry.FirstName,
                LastName = regEntry.LastName,
                Email = regEntry.Email,
                OrgStructId = regEntry.OrgStructId,
                Login = regEntry.Login,
                Password = HashedPassword.GetPasswordHash(regEntry.Login, regEntry.Password),
                EmployeeRoles = new List<EmployeeRole>()
                    {
                        new EmployeeRole()
                        {
                            RoleId = role.Id,
                            EmployeeID = empId,
                        }
                    }
            });
            await _repoReg.DeleteAsync(regEntry);
            return Ok();
        }

        /// <summary>
        /// Invite employee to the org structure 
        /// in case user has admin role allow to map on any orgstruct
        /// </summary>
        /// <param name="orgStructId"></param>
        /// <param name="inviteName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("InviteEmployee")]
        public async Task<ActionResult> InviteEmployee(Guid orgStructId, String inviteName, string email)
        {
            var userIdClaim = User.Claims.FirstOrDefault(cl => cl.Type == "userId");
            if (null == userIdClaim) return BadRequest("User id isn't defined");
            Guid userId;
            try { userId = Guid.Parse(userIdClaim.Value); } catch { return BadRequest("wrong user Id"); };
            var roles = await _repoRole.GetByConditionAsync(rec => rec.Name == "Employee");
            var role = roles?.First();
            if (null == role) return StatusCode(500,"role doesn't exist");
            var regEntry = new RegUserEntry()
            {
                Email = email,
                InviteName = inviteName,
                OrgStructId = orgStructId,
                RegCode = Guid.NewGuid(),
                RegDate = DateTime.Now,
            };
            await _repoReg.CreateAsync(regEntry);
            return Ok();
        }
    }
}
