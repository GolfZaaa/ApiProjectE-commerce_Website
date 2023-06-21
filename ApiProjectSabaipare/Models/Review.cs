using ApiProjectSabaipare.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiCrudProjectS.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Star { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }

    }
}
