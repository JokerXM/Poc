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
    public class ContributionController : Controller
    {
        private myDBcontext db = new myDBcontext();

        //
        // GET: /Contribution/

        public ActionResult Index()
        {
            var contribute = db.Contribute.Include(c => c.contributiontype).Include(c => c.E);
            return View(contribute.ToList());
        }

        //
        // GET: /Contribution/Details/5

        public ActionResult Details(int id = 0)
        {
            Contribution contribution = db.Contribute.Find(id);
            if (contribution == null)
            {
                return HttpNotFound();
            }
            return View(contribution);
        }

        //
        // GET: /Contribution/Create

        public ActionResult Create()
        {

            var contribute = from con in db.ContributionTypes select con;
            int number = contribute.Count<ContributionType>();
            if (number == 0)
            {
                ContributionType ct1 = new ContributionType(); 
                ct1.type = "food";
                db.ContributionTypes.Add(ct1);
                db.SaveChanges();
                ContributionType ct2 = new ContributionType();
                ct2.type = "money";
                db.ContributionTypes.Add(ct2);
                db.SaveChanges();
                ContributionType ct3 = new ContributionType();
                ct3.type = "berverage";
                db.ContributionTypes.Add(ct3);
                db.SaveChanges();
            }
            ViewBag.TypeID = new SelectList(db.ContributionTypes, "ID", "type");
            ViewBag.E_ID = new SelectList(db.Events, "E_ID", "Name");
            return View();
        }

        //
        // POST: /Contribution/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contribution contribution)
        {
            if (ModelState.IsValid)
            {
                db.Contribute.Add(contribution);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TypeID = new SelectList(db.ContributionTypes, "ID", "type", contribution.TypeID);
            ViewBag.E_ID = new SelectList(db.Events, "E_ID", "Name", contribution.E_ID);
            return View(contribution);
        }

        //
        // GET: /Contribution/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Contribution contribution = db.Contribute.Find(id);
            if (contribution == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeID = new SelectList(db.ContributionTypes, "ID", "type", contribution.TypeID);
            ViewBag.E_ID = new SelectList(db.Events, "E_ID", "Name", contribution.E_ID);
            return View(contribution);
        }

        //
        // POST: /Contribution/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contribution contribution)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contribution).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TypeID = new SelectList(db.ContributionTypes, "ID", "type", contribution.TypeID);
            ViewBag.E_ID = new SelectList(db.Events, "E_ID", "Name", contribution.E_ID);
            return View(contribution);
        }

        //
        // GET: /Contribution/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Contribution contribution = db.Contribute.Find(id);
            if (contribution == null)
            {
                return HttpNotFound();
            }
            return View(contribution);
        }

        //
        // POST: /Contribution/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contribution contribution = db.Contribute.Find(id);
            db.Contribute.Remove(contribution);
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