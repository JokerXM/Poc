using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoCPoC.Models;

namespace PoCPoC.Controllers
{
    public class UserController : Controller
    {
        private myDBcontext db = new myDBcontext();

        //
        // GET: /User/

        public ActionResult Index()
        {
            var user = db.User.Include(u => u.role);
            return View(user.ToList());
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(object sender, EventArgs e)
        {

            string username = Request.Form["username"];
            string password = Request.Form["password"];

            var user = from m in db.User
                       select m;
            user = user.Where(s => s.Name.Equals(username));
            foreach (User u in user)
            {
                if (password.Equals(u.Password))
                {
                    Session["username"] = u.Name;
                    Session["ID"] = u.UserID;
        

                    if (u.role.role.Equals("user"))
                    {
                        Session["identify"] = "user";
                        return RedirectToAction("Loginresult_user");
                    }
                    else
                    {
                        Session["identify"] = "admin";
                        return RedirectToAction("Loginresult_admin");
                    }

                }

            }

            return RedirectToAction("Login", "User");
        }

        public ActionResult Loginresult_user()
        {
            List<Events> list = new List<Events>();
            string name = Session["username"].ToString();
            var events = from e in db.Events.Include(e => e.status).Include(e => e.type) select e;
            foreach (var e in events)
            {
                if (e.status.status.Equals("open"))
                {
                    foreach (var u in e.Users)
                    {
                        if (u.Name.Equals(name))
                        {
                            list.Add(e);
                        }
                    }
                }
            }
            return View(list);
        }

        public ActionResult Loginresult_admin()
        {
            var events = db.Events.Include(e => e.status).Include(e => e.type);
            return View(events.ToList());
        }
        //
        // GET: /User/Create

        public ActionResult Create()
        {
            var role = from ro in db.Roles select ro;
            int number = role.Count<Role>();
            if (number == 0)
            {
                Role r = new Role();
                r.role = "user";
                db.Roles.Add(r);
                db.SaveChanges();
                Role r2 = new Role();
                r2.role = "admin";
                db.Roles.Add(r2);
                db.SaveChanges();
            }
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "role");

            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
               
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "role", user.RoleID);
            return View(user);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "role", user.RoleID);
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "role", user.RoleID);
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}