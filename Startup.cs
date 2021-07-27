using FYPProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.IO;
using System.Linq;
using System.Web;
[assembly: OwinStartupAttribute(typeof(FYPProject.Startup))]
namespace FYPProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRootUserAndRoles();
        }

        public void CreateRootUserAndRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                //create admin role
                var role = new IdentityRole("Admin");
                roleManager.Create(role);

                //create root user
                Admin user = new Admin
                {
                    UserName = "root",
                    FullName = "administrator",
                    Email = "root@domain.com",
                    IsEnabled = true
                };
                string pwd = "@Abc123";

                var newuser = userManager.Create(user, pwd);
                if (newuser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }

            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole("User");
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Moderator"))
            {
                var role = new IdentityRole("Moderator");
                roleManager.Create(role);

                Moderator mod = new Moderator
                {
                    UserName = "mod",
                    FullName = "moderator",
                    Email = "mod@domain.com",
                    IsEnabled = true
                };

                string pwd = "@Abc123";

                var newuser = userManager.Create(mod, pwd);
                if (newuser.Succeeded)
                {
                    userManager.AddToRole(mod.Id, "Moderator");
                }
            }
        }
    }
}
