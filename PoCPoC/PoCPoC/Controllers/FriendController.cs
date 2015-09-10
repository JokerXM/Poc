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
    public class FriendController : Controller
    {
        private myDBcontext db = new myDBcontext();

        //
        // GET: /Friend/

        public ActionResult Index()
        {
            int uid = Convert.ToInt32(Session["ID"]);
            List<Friend> listfriend = new List<Friend>();


            var myfriend = from e in db.Friend where( e.UserID == uid) select e;
            
            foreach (Friend f in myfriend)
            {
                listfriend.Add(f);
            }
            //var friend = db.Friend.Include(f => f.User);
            return View(listfriend);
        }

        //
        // GET: /Friend/Details/5

        public ActionResult Details(int id = 0)
        {
            Friend friend = db.Friend.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        //
        // GET: /Friend/Create

        public ActionResult Create()
        {
            ViewBag.Friend_ID = new SelectList(db.User, "UserID", "Name");
            return View();
        }

        //
        // POST: /Friend/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Friend friend)
        {
            if (ModelState.IsValid)
            {
                db.Friend.Add(friend);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Friend_ID = new SelectList(db.User, "UserID", "Name", friend.Friend_ID);
            return View(friend);
        }

        public ActionResult Add(int? id)
        {
            int userid = Convert.ToInt32(Session["ID"]);
            int friendid = Convert.ToInt32(id);
            
            //get friendname
            User u =new User();
            u = db.User.Find(id);
            Friend friend = new Friend() { UserID = userid, Friend_ID = friendid, User = db.User.Find(id) ,FriendName = u.Name};
            db.Friend.Add(friend);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult SearchResult(object sender, EventArgs e)
        {

            string searchString = Request.Form["username"];
            var searchresult = from m in db.User
                               where (m.Name.Contains(searchString)|| m.Nickname.Contains(searchString))
                             select m;
            

            return View(searchresult);

        }



        //
        // GET: /Friend/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Friend friend = db.Friend.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            ViewBag.Friend_ID = new SelectList(db.User, "UserID", "Name", friend.Friend_ID);
            return View(friend);
        }

        //
        // POST: /Friend/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Friend friend)
        {
            if (ModelState.IsValid)
            {
                db.Entry(friend).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Friend_ID = new SelectList(db.User, "UserID", "Name", friend.Friend_ID);
            return View(friend);
        }

        //
        // GET: /Friend/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Friend friend = db.Friend.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        //
        // POST: /Friend/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Friend friend = db.Friend.Find(id);
            db.Friend.Remove(friend);
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