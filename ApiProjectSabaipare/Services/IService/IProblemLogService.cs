using ApiCrudProjectS.Models;

namespace ApiCrudProjectS.Services.IService
{
    public interface IProblemLogService
    {
        Task<List<Problemlog>> GetProblemLog();
        Task<object> CreateAndUpdateProblemLog(Problemlog problemLog);
        Task<bool> DeleteProblemLog(int id);
    }
}
