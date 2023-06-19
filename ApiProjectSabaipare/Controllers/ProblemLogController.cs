using ApiCrudProjectS.Models;
using ApiCrudProjectS.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCrudProjectS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemLogController : ControllerBase
    {
        private readonly IProblemLogService _problemLogService;

        public ProblemLogController(IProblemLogService problemLogService)
        {
            _problemLogService = problemLogService;
        }

        [HttpGet]
        [Route("GetProblemLog")]
        public async Task<IEnumerable<Problemlog>> GetProblemLogs()
        {
            return await _problemLogService.GetProblemLog();
        }

        [HttpPost]
        [Route("CreateAndUpdateProblemLog")]
        public async Task<Object> CreateAndUpdateProblemLog(Problemlog problemLog)
        {
            return await _problemLogService.CreateAndUpdateProblemLog(problemLog);
        }

        [HttpDelete]
        [Route("DeleteProblemLog/{id}")]
        public async Task<bool> DeleteProblemLog(int id)
        {
            return await _problemLogService.DeleteProblemLog(id);
        }

    }
}
