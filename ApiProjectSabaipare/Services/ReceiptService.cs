using ApiCrudProjectS.Services.IService;
using ApiProjectSabaipare.Data;

namespace ApiCrudProjectS.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly DataContext _dataContext;

        public ReceiptService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
