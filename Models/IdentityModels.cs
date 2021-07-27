using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FYPProject.Models
{

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FullName { set; get; }
        public bool IsEnabled { get; set; }
        public string StripeToken { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public ApplicationUser RetrieveUserProfile(string userId)
        {
            return (new ApplicationDbContext()).Users.FirstOrDefault(s => s.Id == userId);
        }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<FYPProject.Models.Campaign> Campaigns { get; set; }
        public System.Data.Entity.DbSet<FYPProject.Models.User> ApplicationUsers { get; set; }
        public System.Data.Entity.DbSet<FYPProject.Models.Comments> Comments { get; set; }
        public System.Data.Entity.DbSet<FYPProject.Models.Updates> Update { get; set; }
        public System.Data.Entity.DbSet<FYPProject.Models.Follows> Follow { get; set; }
        public System.Data.Entity.DbSet<FYPProject.Models.EditUserViewModel> EditUserViewModels { get; set; }
        public System.Data.Entity.DbSet<FYPProject.Models.Reports> Report { get; set; }
        public System.Data.Entity.DbSet<FYPProject.Models.Checkout> Checkouts { get; set; }
        public System.Data.Entity.DbSet<FYPProject.Models.CreateCampaignViewModel> CreateCampaignViewModels { get; set; }

        public System.Data.Entity.DbSet<FYPProject.Models.FollowViewModel> FollowViewModels { get; set; }
    }
}