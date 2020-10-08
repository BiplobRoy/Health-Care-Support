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
    public class SubCityController : Controller
    {
        private dbHCS02Entities db = new dbHCS02Entities();

        // GET: SubCity
        public ActionResult Index()
        {
            if (Session["Adlogin"].ToString() == "")
            {
                return RedirectToAction("AdLogin");
            }
            var subCities = db.SubCities.Include(s => s.City);
            return View(subCities.ToList());
        }

        // GET: SubCity/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Adlogin"].ToString() == "")
            {
                return RedirectToAction("AdLogin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCity subCity = db.SubCities.Find(id);
            if (subCity == null)
            {
                return HttpNotFound();
            }
            return View(subCity);
        }

        // GET: SubCity/Create
        public ActionResult Create()
        {
            if (Session["Adlogin"].ToString() == "")
            {
                return RedirectToAction("AdLogin");
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name");
            return View();
        }

        // POST: SubCity/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CityId,Name")] SubCity subCity)
        {
            if (ModelState.IsValid)
            {
                db.SubCities.Add(subCity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", subCity.CityId);
            return View(subCity);
        }

        // GET: SubCity/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Adlogin"].ToString() == "")
            {
                return RedirectToAction("AdLogin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCity subCity = db.SubCities.Find(id);
            if (subCity == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", subCity.CityId);
            return View(subCity);
        }

        // POST: SubCity/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CityId,Name")] SubCity subCity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subCity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", subCity.CityId);
            return View(subCity);
        }

        // GET: SubCity/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Adlogin"].ToString() == "")
            {
                return RedirectToAction("AdLogin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCity subCity = db.SubCities.Find(id);
            if (subCity == null)
            {
                return HttpNotFound();
            }
            return View(subCity);
        }

        // POST: SubCity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubCity subCity = db.SubCities.Find(id);
            db.SubCities.Remove(subCity);
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
