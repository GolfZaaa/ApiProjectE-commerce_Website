using ApiCrudProjectS.Models;
using ApiCrudProjectS.Services.IService;
using ApiProjectSabaipare.Data;
using ApiProjectSabaipare.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCrudProjectS.Services
{
    public class ProblemLogService : ControllerBase, IProblemLogService
    {
        private readonly DataContext _dataContext;

        public ProblemLogService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<List<Problemlog>> GetProblemLog()
        {
            return await _dataContext.Problemlog.ToListAsync();
        }

        public async Task<Object> CreateAndUpdateProblemLog(Problemlog problemLog)
        {
            var user = await _dataContext.Users.FindAsync(problemLog.UserId);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseReport { Status = "404", Message = "User Not found." });
            }
            else
            {
                if (problemLog.Id == 0)
                {
                    _dataContext.Entry(problemLog).State = EntityState.Added;
                }
                else
                {
                    _dataContext.Entry(problemLog).State = EntityState.Modified;
                }
            }
            
            await _dataContext.SaveChangesAsync();
            return problemLog;
        }


        public async Task<bool> DeleteProblemLog(int id)
        {
            bool check = false;
            var problemLog = await _dataContext.Problemlog.FindAsync(id);

            if (problemLog != null)
            {
                check = true;
                _dataContext.Entry(problemLog).State = EntityState.Deleted;
                await _dataContext.SaveChangesAsync();
            }
            else check = false;

            return check;
        }
    }
}
