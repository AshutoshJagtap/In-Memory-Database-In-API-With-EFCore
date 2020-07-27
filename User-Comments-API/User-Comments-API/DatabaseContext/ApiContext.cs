using Microsoft.EntityFrameworkCore;
using User_Comments_API.DbModels;

namespace User_Comments_API.DatabaseContext
{
    public class ApiContext :DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options):base(options){

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
