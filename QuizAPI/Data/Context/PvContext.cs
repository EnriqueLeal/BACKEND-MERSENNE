using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data.Context
{
    public partial class PvContext : DbContext
    {
        public PvContext()
        {
        }

        public PvContext(DbContextOptions<PvContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Participant> Participants { get; set; }
    }
}
