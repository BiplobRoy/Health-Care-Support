using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HealthCareSupport02.Models;

namespace HealthCareSupport02.Controllers
{
    public class HealthTipController : Controller
    {
        private dbHCS02Entities db = new dbHCS02Entities();

        // GET: HealthTip
        public ActionResult Index()
        {

            return View(db.HealthTips.ToList());
        }

        //Health tips Search
        [HttpPost]
        public ActionResult Index(string search = "")
        {
            if (search != "")
            {
                var v = db.HealthTips.Where(e => e.Name.ToLower().Contains(search.ToLower()));
            }
            return View(db.HealthTips.ToList());
        }

        // GET: HealthTip/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HealthTip healthTip = db.HealthTips.Find(id);
            if (healthTip == null)
            {
                return HttpNotFound();
            }
            return View(healthTip);
        }

        // GET: HealthTip/Create
        public ActionResult Create()
        {
            if (Session["Adlogin"].ToString() == "" && Session["Dologin"].ToString() == "")
            {
                return RedirectToAction("DoLogin");
            }
            return View();
        }

        // POST: HealthTip/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Details")] HealthTip healthTip)
        {
            if (ModelState.IsValid)
            {
                db.HealthTips.Add(healthTip);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(healthTip);
        }

        // GET: HealthTip/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Adlogin"].ToString() == "" && Session["Dologin"].ToString() == "")
            {
                return RedirectToAction("DoLogin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HealthTip healthTip = db.HealthTips.Find(id);
            if (healthTip == null)
            {
                return HttpNotFound();
            }
            return View(healthTip);
        }

        // POST: HealthTip/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Details")] HealthTip healthTip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(healthTip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(healthTip);
        }

        // GET: HealthTip/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Adlogin"].ToString() == "" && Session["Dologin"].ToString() == "")
            {
                return RedirectToAction("DoLogin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HealthTip healthTip = db.HealthTips.Find(id);
            if (healthTip == null)
            {
                return HttpNotFound();
            }
            return View(healthTip);
        }

        // POST: HealthTip/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HealthTip healthTip = db.HealthTips.Find(id);
            db.HealthTips.Remove(healthTip);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
