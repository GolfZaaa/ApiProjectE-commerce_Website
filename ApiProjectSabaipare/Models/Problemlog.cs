using ApiProjectSabaipare.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCrudProjectS.Models
{
    public class Problemlog
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }

    }
}
