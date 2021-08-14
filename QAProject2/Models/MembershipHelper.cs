using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace QAProject2.Models
{
    public class MembershipHelper
    {
        static ApplicationDbContext db = new ApplicationDbContext();
        static RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
        static UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        public static List<String> GetAllRoles()
        {
            var result = roleManager.Roles.Select(r => r.Name).ToList();
            return result;
        }
        // check if user is in Role
        public static bool CheckIfuserIsInRole(string userId, string roleId)
        {
            var result = userManager.IsInRole(userId, roleId);
            return result;
        }
        // AdduserToRole
        public static bool AddUserToRole(string userId, string roleName)
        {
            if (CheckIfuserIsInRole(userId, roleName))
            {
                return false;
            }
            else
            {
                userManager.AddToRole(userId, roleName);
                return true;
            }
        }
        //RemoveuserFromRole
        //AddRole
        public static void AddRole(string roleName)
        {
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole { Name = roleName });
            }
        }
        //RemoveRole
        public static void RemoveRole(string roleName)
        {
            if (roleManager.RoleExists(roleName))
            {
                roleManager.Delete(new IdentityRole { Name = roleName });
            }
        }
        //GetAllRoleOfUser
        public static List<string> GetAllRoleOfUser(string userId)
        {
            return userManager.GetRoles(userId).ToList();
        }
    }
}