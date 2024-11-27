using System.ComponentModel.DataAnnotations;

namespace WebRencanaProduksi.Models
{
    public class LogEfisiensi
    {
        [Key]public int Id { get; set; }
        public int IdRencanaProduksi { get; set; }
        public string? Hari {  get; set; }
        public int? Produksi { get; set; }
    }
}
