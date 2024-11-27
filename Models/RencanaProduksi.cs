using System.ComponentModel.DataAnnotations;

namespace WebRencanaProduksi.Models
{
    public class RencanaProduksi
    {
        [Key]public int Id { get; set; }
        public string? Hari {  get; set; }
        public int? Produksi { get; set; }
    }
}
