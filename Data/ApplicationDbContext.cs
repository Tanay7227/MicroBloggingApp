using Blog_MSP.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog_MSP.Data
{
    //First Method (optimize)
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<AppUser>(options)
    {
    } 

}


//Second Method
//public class ApplicationDbContext : IdentityDbContext<AppUser>
//{
//    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
//    {

//    }
//}

