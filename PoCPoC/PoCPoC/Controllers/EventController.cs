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
    public class EventController : Controller
    {
        private myDBcontext db = new myDBcontext();
        //
        // GET: /Event/

        public ActionResult Index()
        {
            var events = db.Events.Include(e => e.status).Include(e => e.type);
            return View(events.ToList());
        }

        public ActionResult IndexforUser()
        {

            
            List<Events> list = new List<Events>();
            db.Events.Include(e => e.status).Include(e => e.type);
            string name = Session["username"].ToString();
            var events = from e in db.Events.Include(e => e.status).Include(e => e.type) select e;
            foreach (var e in events)
            {
                if (e.Createuser.Equals(name))
                {
                    list.Add(e);
                }
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

        //
        // GET: /Event/Details/5

        public ActionResult Details(int id = 0)
        {
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        //
        // GET: /Event/Create

        public ActionResult Create()
        {
            
            var type = from t in db.Types select t;
            int number = type.Count<EType>();
            if (number == 0)
            {
                EType et1 = new EType();
                EType et2 = new EType();
                EType et3 = new EType();
                EType et4 = new EType();
                EType et5 = new EType();
                et1.type = "breakfast";
                db.Types.Add(et1);
                db.SaveChanges();
                et2.type = "lunch";
                db.Types.Add(et2);
                db.SaveChanges();
                et3.type = "dinner";
                db.Types.Add(et3);
                db.SaveChanges();
                et4.type = "party";
                db.Types.Add(et4);
                db.SaveChanges();
                et5.type = "others";
                db.Types.Add(et5);
                db.SaveChanges();
            }

            var status = from m in db.Status select m;
            int number2 = status.Count<Status>();
            if (number == 0)
            {

                Status s1 = new Status();
                Status s2 = new Status();
                Status s3 = new Status();
                Status s4 = new Status();
                Status s5 = new Status();
                s1.status = "open";
                db.Status.Add(s1);
                db.SaveChanges();
                s2.status = "close";
                db.Status.Add(s2);
                db.SaveChanges();
                s3.status = "lock";
                db.Status.Add(s3);
                db.SaveChanges();
                s4.status = "unlock";
                db.Status.Add(s4);
                db.SaveChanges();
                s5.status = "pending";
                db.Status.Add(s5);
                db.SaveChanges();
            }


            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "status");
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "type");
            return View();
        }

        public ActionResult Addtype()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Addtype(object sender, EventArgs e)
        {
            string typename = Request.Form["typename"];
            EType et = new EType();
            et.type = typename;
            db.Types.Add(et);
            db.SaveChanges();
            return RedirectToAction("Create");

        }

        //
        // POST: /Event/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Events events)
        {
            if (ModelState.IsValid)
            {
                events.Createuser = Session["username"].ToString();
                events.StatusID= 5 ;
                db.Events.Add(events);
                db.SaveChanges();
                if (Session["identify"].Equals("admin"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Session["thisid"] = events.E_ID;
                    return RedirectToAction("IndexforUser");
                }
            }

            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "status", events.StatusID);
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "type", events.TypeID);
            return View(events);
        }

        public ActionResult InviteFriend(int ?id)
        {
            int uid = Convert.ToInt32(Session["ID"]);
            List<Friend> listfriend = new List<Friend>();
            Session["eventid"] = id;

            var myfriend = from e in db.Friend where (e.UserID == uid) select e;

            foreach (Friend f in myfriend)
            {
                listfriend.Add(f);
            }
            return View(listfriend);
        }
        public ActionResult Invite(int ?id)
        {
            int eid = Convert.ToInt32(Session["eventid"].ToString());
            Events e = db.Events.Find(eid);
            db.Entry(e).State = EntityState.Modified;
            User u = db.User.Find(id);
            e.Users.Add(u);
            db.SaveChanges();
            if (Session["identify"].Equals("admin"))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("IndexforUser");
            }
        }

       

        public ActionResult Open(int? id)
        {
            Events events = db.Events.Find(id);
            events.StatusID = 1;
            db.Entry(events).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult Close(int? id)
        {
            Events events = db.Events.Find(id);
            events.StatusID = 2;
            db.Entry(events).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult Lock(int? id)
        {
            Events events = db.Events.Find(id);
            events.StatusID = 3;
            db.Entry(events).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult UnLock(int? id)
        {
            Events events = db.Events.Find(id);
            events.StatusID = 4;
            db.Entry(events).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Whojoin(int? id)
        {
            Events events = db.Events.Find(id);
            List<User> list = new List<User>();
            foreach (var u in events.Users)
            {
                list.Add(u);
            }
            return View(list);
        }

        public ActionResult Contributions(int? id)
        {
            Events events = db.Events.Find(id);
            List<Contribution> list = new List<Contribution>();
            foreach (var c in events.Contributions)
            {
                list.Add(c);
            }
            return View(list);
        }
        //
        // GET: /Event/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "status", events.StatusID);
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "type", events.TypeID);
            return View(events);
        }

        //
        // POST: /Event/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Events events)
        {
            if (ModelState.IsValid)
            {
                db.Entry(events).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "status", events.StatusID);
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "type", events.TypeID);
            return View(events);
        }

        //
        // GET: /Event/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        //
        // POST: /Event/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Events events = db.Events.Find(id);
            db.Events.Remove(events);
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