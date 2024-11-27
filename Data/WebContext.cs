using Microsoft.EntityFrameworkCore;
using WebRencanaProduksi.Models;

namespace WebRencanaProduksi.Data
{
    public class WebContext : DbContext
    {
        public WebContext(DbContextOptions<WebContext> options) : base(options) {
        }

        public DbSet<RencanaProduksi> RencanaProduksi { get; set; }
        public DbSet<LogEfisiensi> LogEfisiensi { get; set; }


    }

    

}
