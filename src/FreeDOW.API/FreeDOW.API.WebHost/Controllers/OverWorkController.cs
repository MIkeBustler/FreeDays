using FreeDOW.API.Core.Abstract;
using FreeDOW.API.Core.Entities;
using FreeDOW.API.WebHost.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeDOW.API.WebHost.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OverWorkController : ControllerBase
    {
        private readonly IWorkTimeManageRepository<OverWork> _repoOW;
        private readonly IOrgRepository<Equipment> _repoEq;
        private readonly IOrgRepository<Employee> _repoEmp;
        public OverWorkController(IWorkTimeManageRepository<OverWork> repoOW, IOrgRepository<Equipment> repoEq, IOrgRepository<Employee> repoEmp)
        {
            _repoOW = repoOW;
            _repoEq = repoEq;
            _repoEmp = repoEmp;
        }

        [HttpGet]
        public async Task<ActionResult<OverWorkResponce>> GetOverWorksAsync()
        {
            var ow = await _repoOW.GetAllAsync();
            var res = ow.Select(ow => new OverWorkResponce()
            {

            }).ToList();
            return Ok(res);

        }
        [HttpGet("GetOverworkByEmployeeId")]
        public async Task<ActionResult<IEnumerable<OverWorkResponce>>> GetOverWorkByEmployeeId(OverWorkRequest request)
        {
            if (Guid.Empty == request.EmployeeId) return BadRequest();
            if (request.DTTo <= request.DTFrom) return BadRequest();
            var owL = await _repoOW.GetByConditionAsync(rec => 
                rec.EmployeeId == request.EmployeeId
                && rec.DTStart > request.DTFrom
                && rec.DTEnd <= request.DTTo
            );
            if (null == owL) return NotFound();
            var res = owL.Select(async rec => new OverWorkResponce()
            {
                EmployeeId = rec.EmployeeId,
                DTStart = rec.DTStart,
                DTEnd = rec.DTEnd,    
                EquipmentId = rec.EquipmentId,
                EquipmentDetail = rec.EquipmentDetail,
                EquipmentPath = await GetEquipmentPath(rec.EquipmentId),
                Confirmed = rec.Confirmed,
                Description = rec.Desription,
                Id = rec.Id,
                Reminder = rec.Reminder,
            });
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OverWorkResponce>> GetOverWorkAsync(Guid id)
        {
            var ow = await _repoOW.GetByIdAsync(id);
            if (null == ow) return NotFound();
            var res = new OverWorkResponce()
            {
                EmployeeId = ow.EmployeeId,
                DTStart = ow.DTStart,
                DTEnd = ow.DTEnd,
                EquipmentId = ow.EquipmentId,
                EquipmentDetail = ow.EquipmentDetail,
                EquipmentPath = await GetEquipmentPath(ow.EquipmentId),
                Confirmed = ow.Confirmed,
                Description = ow.Desription,
                Id = ow.Id,
                Reminder = ow.Reminder,
            };
            return Ok(res);
        }

        [HttpPost("{request}")]
        public async Task<ActionResult<OverWorkResponce>> CreateOverWorkAsync(CreateOrEditOverWork request)
        {
            if (request.DTStart >= request.DTEnd) return BadRequest();
            var equipment = await _repoEq.GetByIdAsync(request.EquipmentId);
            if (null == equipment) return BadRequest();
            var emp = await _repoEmp.GetByIdAsync(request.EmployeeId);
            if (null == emp) return BadRequest();
            var ow = Mappers.OverWorkMapper.MapFromModel(request, equipment, emp);

            return CreatedAtAction(nameof(GetOverWorkAsync), new { id = ow.Id }, null);
        }

        /// <summary>
        /// return full path to equipment devided by dot
        /// </summary>
        /// <param name="eqId"></param>
        /// <returns></returns>
        private async Task<string> GetEquipmentPath(Guid eqId)
        {
            string path = "";
            if (Guid.Empty == eqId) return path;
            var res = await _repoEq.GetByIdAsync(eqId);
            path = res.Name;
            while(Guid.Empty != res.ParentId)
            {
                res = await _repoEq.GetByIdAsync(eqId);
                path = $"{res.Name}.{path}";
            }
            return path;
        }
    }
}
