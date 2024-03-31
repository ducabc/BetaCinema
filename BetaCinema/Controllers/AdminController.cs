using BetaCinema.Entity;
using BetaCinema.IService;
using BetaCinema.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        public AdminController()
        {
            adminService = new AdminService();
        }
        [HttpPost("AddCinema"),Authorize(Roles = "Admin")]
        public IActionResult AddCinema([FromBody]Cinema cinema)
        {
            var res = adminService.AddCinema(cinema);
            return Ok(res);
        }
        [HttpPost("AddRoom"),Authorize(Roles = "Admin")]
        public IActionResult AddRoom([FromBody] Room room)
        {
            var res = adminService.AddRoom(room);
            return Ok(res);
        }
        [HttpPost("AddSeat"), Authorize(Roles = "Admin")]
        public IActionResult AddSeat([FromBody] Seat seat)
        {
            var res = adminService.AddSeat(seat);
            return Ok(res);
        }
        [HttpPost("AddFood"), Authorize(Roles = "Admin")]
        public IActionResult AddFood([FromBody] Food food)
        {
            var res = adminService.AddFood(food);
            return Ok(res);
        }
        [HttpPost("AddMovie"), Authorize(Roles = "Admin")]
        public IActionResult AddMovie([FromBody] Movie movie)
        {
            var res = adminService.AddMovie(movie);
            return Ok(res);
        }
        [HttpPost("AddSchedule"), Authorize(Roles = "Admin")]
        public IActionResult AddSchedule([FromBody] Schedule sche)
        {
            var res = adminService.AddSchedule(sche);
            return Ok(res);
        }

        [HttpPut("UpdateCinema"),Authorize(Roles = "Admin")]
        public IActionResult UpdateCinema([FromBody] Cinema cinema)
        {
            var res = adminService.UpdateCinema(cinema);
            return Ok(res);
        }
        [HttpPut("UpdateRoom"), Authorize(Roles = "Admin")]
        public IActionResult UpdateRoom([FromBody] Room room)
        {
            var res = adminService.UpdateRoom(room);
            return Ok(res);
        }
        [HttpPut("UpdateSeat"), Authorize(Roles = "Admin")]
        public IActionResult UpdateSeat([FromBody] Seat seat)
        {
            var res = adminService.UpdateSeat(seat);
            return Ok(res);
        }
        [HttpPut("UpdateFood"), Authorize(Roles = "Admin")]
        public IActionResult UpdateFood([FromBody] Food food)
        {
            var res = adminService.UpdateFood(food);
            return Ok(res);
        }
        [HttpPut("UpdateMovie"), Authorize(Roles = "Admin")]
        public IActionResult UpdateMovie([FromBody] Movie movie)
        {
            var res = adminService.UpdateMovie(movie);
            return Ok(res);
        }
        [HttpPut("UpdateSchedule"), Authorize(Roles = "Admin")]
        public IActionResult UpdateSchedule([FromBody] Schedule sche)
        {
            var res = adminService.UpdateSchedule(sche);
            return Ok(res);
        }

        [HttpDelete("DeleteCinema"),Authorize(Roles = "Admin")]
        public IActionResult DeleteCinema([FromQuery]int id) 
        {
            var res = adminService.DeleteCinema(id);
            return Ok(res);
        }
        [HttpDelete("DeleteRoom"), Authorize(Roles = "Admin")]
        public IActionResult DeleteRoom([FromQuery] int id)
        {
            var res = adminService.DeleteRoom(id);
            return Ok(res);
        }
        [HttpDelete("DeleteSeat"), Authorize(Roles = "Admin")]
        public IActionResult DeleteSeat([FromQuery] int id)
        {
            var res = adminService.DeleteSeat(id);
            return Ok(res);
        }
        [HttpDelete("DeleteFood"), Authorize(Roles = "Admin")]
        public IActionResult DeleteFood([FromQuery] int id)
        {
            var res = adminService.DeleteFood(id);
            return Ok(res);
        }
        [HttpDelete("DeleteMovie"), Authorize(Roles = "Admin")]
        public IActionResult DeleteMovie([FromQuery] int id)
        {
            var res = adminService.DeleteMovie(id);
            return Ok(res);
        }
        [HttpDelete("DeleteSchedule"), Authorize(Roles = "Admin")]
        public IActionResult DeleteSchedule([FromQuery] int id)
        {
            var res = adminService.DeleteSchedule(id);
            return Ok(res);
        }
        [HttpGet("DoanhSo"), Authorize(Roles = "Admin")]
        public IActionResult DoanhSo([FromQuery] DateTime before, DateTime then)
        {
            var res = adminService.DoanhSo(before,then);
            return Ok(res);
        }
        [HttpGet("ListFoodHot"), Authorize(Roles = "Admin")]
        public IActionResult ListFoodHot()
        {
            var res = adminService.ListFoodHot();
            return Ok(res);
        }
    }
}
