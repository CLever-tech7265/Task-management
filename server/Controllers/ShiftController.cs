using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.modules;

namespace TaskManagement.Controllers{
    [ApiController]
    [Route("api/shift")]
    public class ShiftController:ControllerBase{
        private readonly TaskDbContext _context;
        public ShiftController(TaskDbContext context)
        {
            _context=context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shift>>> GetShifts(int skip=0,int take=2)
        {
            return await _context.Shifts.Skip(skip).Take(take).ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<Shift>> CreateShift(ShiftDTO shiftdto){
            if(string.IsNullOrWhiteSpace(shiftdto.StartHour)||string.IsNullOrWhiteSpace(shiftdto.FinishHour)){
                return BadRequest("StartHour and FinishHour is required");
            }
            // if(
            //     _context.Shifts.Any(async async=>a.StartHour!=null&&a.StartHour)
            // )
            var shift=new Shift
            {
                Id=Guid.NewGuid(),
                StartHour=shiftdto.StartHour.Trim(),
                FinishHour=shiftdto.FinishHour.Trim(),
                Specs=[]
            };
            _context.Shifts.Add(shift);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetShifts),shift) ;
        }
    }
}
