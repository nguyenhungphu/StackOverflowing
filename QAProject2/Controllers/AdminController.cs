using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using QAProject2.Models;

namespace QAProject2.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllRoles()
        {
            var roles = MembershipHelper.GetAllRoles();
            return View(roles);
        }
        [HttpGet]
        public ActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRole(string Name)
        {
            MembershipHelper.AddRole(Name);
            return RedirectToAction("AllRoles");
        }
        [HttpGet]
        public ActionResult AddUserToRole()
        {
            // selectList Roles
            ViewBag.roleName = new SelectList(db.Roles.ToList(), "Name", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult AddUserToRole(string userId, string roleName)
        {
            userId = User.Identity.GetUserId();
            // selectList Roles
            ViewBag.roleName = new SelectList(db.Roles.ToList(), "Name", "Name");
            MembershipHelper.AddUserToRole(userId, roleName);
            return View();
        }
    }
}