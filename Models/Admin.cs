using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPProject.Models
{
    public class Admin : ApplicationUser
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public List<ApplicationUser> RetrieveUsers(string search)
        {
            return context.Users.Where(x => x.UserName.Contains(search) || search == null).ToList();
        }

        public ApplicationUser CreateUser(CreateUserViewModel model)
        {
            if (model.Role == "User")
            {
                return new User { UserName = model.UserName, Email = model.Email, FullName = model.Fullname, IsEnabled = true };
            }
            else if (model.Role == "Admin")
            {
                return new Admin { UserName = model.UserName, Email = model.Email, FullName = model.Fullname, IsEnabled = true };
            }
            /*else if (model.Role == "Moderator")
            {
                return new Moderator { UserName = model.UserName, Email = model.Email, FullName = model.Fullname, IsEnabled = true };
            }*/
            return null;
        }

        public EditUserViewModel EditUser(ApplicationUser user)
        {
            EditUserViewModel editUserViewModel = new EditUserViewModel();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var role = userManager.GetRoles(user.Id);

            if (role.FirstOrDefault() == "User")
            {
                var studentManager = new UserManager<User>(new UserStore<User>(context));
                User student = studentManager.FindById(user.Id);
                editUserViewModel.Id = student.Id;
                editUserViewModel.Email = student.Email;
                editUserViewModel.FullName = student.FullName;
                editUserViewModel.UserName = student.UserName;
                if (student.IsEnabled)
                {
                    editUserViewModel.isEnabled = false;
                }
                else
                {
                    editUserViewModel.isEnabled = true;
                }
            }
            else
            {
                editUserViewModel.Id = user.Id;
                editUserViewModel.Email = user.Email;
                editUserViewModel.FullName = user.FullName;
                editUserViewModel.UserName = user.UserName;
                if (user.IsEnabled)
                {
                    editUserViewModel.isEnabled = false;
                }
                else
                {
                    editUserViewModel.isEnabled = true;
                }
            }
            return editUserViewModel;
        }

        public async Task<IdentityResult> EditUser(EditUserViewModel model, ApplicationUserManager userManager)
        {
            var role = userManager.GetRoles(model.Id);

            if (role.FirstOrDefault() == "User")
            {
                User user = (User)await userManager.FindByIdAsync(model.Id);
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FullName = model.FullName;
                if (model.isEnabled)
                {
                    user.IsEnabled = false;
                }
                else
                {
                    user.IsEnabled = true;
                }
                return await userManager.UpdateAsync(user);
            }
            else if (role.FirstOrDefault() == "Admin")
            {
                Admin user = (Admin)await userManager.FindByIdAsync(model.Id);

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FullName = model.FullName;
                if (model.isEnabled)
                {
                    user.IsEnabled = false;
                }
                else
                {
                    user.IsEnabled = true;
                }
                return await userManager.UpdateAsync(user);
            }
            else if (role.FirstOrDefault() == "Moderator")
            {
                Moderator user = (Moderator)await userManager.FindByIdAsync(model.Id);

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FullName = model.FullName;
                if (model.isEnabled)
                {
                    user.IsEnabled = false;
                }
                else
                {
                    user.IsEnabled = true;
                }
                return await userManager.UpdateAsync(user);
            }
            return IdentityResult.Failed("Role does not exist for this user");
        }
    }
}