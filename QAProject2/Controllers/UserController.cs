using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QAProject2.Models;
using Microsoft.AspNet.Identity;

namespace QAProject2.Controllers
{
    public class UserController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        // GET: User
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult ShowMyRoles()
        {
            if (User.Identity.IsAuthenticated)
            {
                var id = User.Identity.GetUserId();
                var roles = MembershipHelper.GetAllRoleOfUser(id);
                return View(roles);
            }
            return new HttpUnauthorizedResult();
        }
    }
}