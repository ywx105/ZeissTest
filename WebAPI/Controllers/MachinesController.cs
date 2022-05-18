using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachinesController : Controller
    {
        private readonly IMachinesPayloadService _machinesReponseService;
        public MachinesController(IMachinesPayloadService machinesReponseService)
        {
            _machinesReponseService = machinesReponseService;
        }

        // Get : api/v1/Machines
        /// <summary>
        /// Get all Machines payload
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMachinesPayloadListAsync()
        {
            try
            {
                var result = await _machinesReponseService.GetMachinesPayLoadAsync();

                return Ok(result);
            }
            catch
            {
                return View();
            }
        }

    }
}
