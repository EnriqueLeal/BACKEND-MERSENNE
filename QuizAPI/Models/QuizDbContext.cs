using API.Models;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Models
{
    public class QuizDbContext:DbContext
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options):base(options)
        { }

        public DbSet<Participant> Participants { get; set; }
        public DbSet<Mersenne> Mersenne { get; set; }
    }
}
