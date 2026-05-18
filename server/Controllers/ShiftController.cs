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
        public async Task<ActionResult<IEnumerable<Shift>>> GetShifts(int skip=1,int take=7)
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
        [HttpPut("{id}")]
        public async Task<ActionResult<Shift>> UpdateShift(Guid id,string StartHour,string FinishHour)
        {
            // if(id!= shiftdto.Id)
            // {
            //     return BadRequest();
            // }
            if (!ShiftExists(id))
            {
                return NotFound();
            }
            var shiftEntity=await _context.Shifts.FindAsync(id) ;
            shiftEntity.StartHour=StartHour.Trim() ?? shiftEntity.StartHour;
            shiftEntity.FinishHour=FinishHour.Trim() ?? shiftEntity.FinishHour;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(Guid id)
        {
            var shift=await _context.Shifts.FindAsync(id);
            if(shift==null)
            {
                return NotFound();
            }
            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();
            return NoContent();

        }
        private bool ShiftExists(Guid id)
        {
            return _context.Shifts.Any(e=>e.Id==id);
        }
    }
}
