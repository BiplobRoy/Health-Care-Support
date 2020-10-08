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
    public class AmbulanceController : Controller
    {
        private dbHCS02Entities db = new dbHCS02Entities();

        // GET: Ambulance
        public ActionResult Index(string search = "")
        {
            if (Session["Adlogin"].ToString() == "" && Session["Dologin"].ToString() == "" && Session["Palogin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name");
            var v = db.Ambulances.ToList();
            var ambulances = db.Ambulances.Include(a => a.City).Include(a => a.SubCity);
            return View(v);
        }

        [HttpPost]
        public ActionResult Index(int? SubCityId, string search = "")
        {
            if (search == "")
            {
                ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name");
                var ambulances = db.Ambulances.Include(a => a.City).Include(a => a.SubCity).Where(a => a.SubCityId == SubCityId);
                return View(ambulances.ToList());
            }

            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name");
            var v = db.Ambulances.Include(a => a.City).Include(a => a.SubCity).Where(e => e.Hospital.ToLower().Contains(search.ToLower())).ToList();
            return View(v.ToList());
        }

        // GET: Ambulance/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Adlogin"].ToString() == "" && Session["Dologin"].ToString() == "" && Session["Palogin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ambulance ambulance = db.Ambulances.Find(id);
            if (ambulance == null)
            {
                return HttpNotFound();
            }
            return View(ambulance);
        }

        // GET: Ambulance/Create
        public ActionResult Create()
        {
            if (Session["Adlogin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name");
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name");
            return View();
        }

        // POST: Ambulance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DriverName,CityId,SubCityId,Phone,CarNumber,Hospital")] Ambulance ambulance)
        {
            if (ModelState.IsValid)
            {
                db.Ambulances.Add(ambulance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", ambulance.CityId);
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name", ambulance.SubCityId);
            return View(ambulance);
        }

        // GET: Ambulance/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Adlogin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ambulance ambulance = db.Ambulances.Find(id);
            if (ambulance == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", ambulance.CityId);
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name", ambulance.SubCityId);
            return View(ambulance);
        }

        // POST: Ambulance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DriverName,CityId,SubCityId,Phone,CarNumber,Hospital")] Ambulance ambulance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ambulance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", ambulance.CityId);
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name", ambulance.SubCityId);
            return View(ambulance);
        }

        // GET: Ambulance/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Adlogin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ambulance ambulance = db.Ambulances.Find(id);
            if (ambulance == null)
            {
                return HttpNotFound();
            }
            return View(ambulance);
        }

        // POST: Ambulance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ambulance ambulance = db.Ambulances.Find(id);
            db.Ambulances.Remove(ambulance);
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
