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
    public class HospitalController : Controller
    {
        private dbHCS02Entities db = new dbHCS02Entities();

        // GET: Hospital
        public ActionResult Index()
        {
            ViewBag.SubCItyId = new SelectList(db.SubCities, "Id", "Name");
            var hospitals = db.Hospitals.Include(h => h.City).Include(h => h.SubCity);
            return View(hospitals.ToList());
        }


        //hospital search
        [HttpPost]
        public ActionResult Index(int? SubCityId, string search = "")
        {
            if(SubCityId != null)
            {
                ViewBag.SubCItyId = new SelectList(db.SubCities, "Id", "Name");
                var hos = db.Hospitals.Include(h => h.City).Include(h => h.SubCity).Where(e => e.SubCItyId == SubCityId);
                return View(hos.ToList());
            }
            if(search != "")
            {
                ViewBag.SubCItyId = new SelectList(db.SubCities, "Id", "Name");
                var hos = db.Hospitals.Include(h => h.City).Include(h => h.SubCity).Where(e => e.Name.ToLower().Contains(search.ToLower()));
                return View(hos.ToList());
            }
            ViewBag.SubCItyId = new SelectList(db.SubCities, "Id", "Name");
            var hospitals = db.Hospitals.Include(h => h.City).Include(h => h.SubCity);
            return View(hospitals.ToList());
        }

        // GET: Hospital/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hospital hospital = db.Hospitals.Find(id);
            if (hospital == null)
            {
                return HttpNotFound();
            }
            return View(hospital);
        }

        // GET: Hospital/Create
        public ActionResult Create()
        {
            if (Session["Adlogin"].ToString() == "")
            {
                return RedirectToAction("AdLogin");
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name");
            ViewBag.SubCItyId = new SelectList(db.SubCities, "Id", "Name");
            return View();
        }

        // POST: Hospital/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CityId,SubCItyId,Phone,WebSite,Specialization,Details")] Hospital hospital)
        {
            if (ModelState.IsValid)
            {
                db.Hospitals.Add(hospital);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", hospital.CityId);
            ViewBag.SubCItyId = new SelectList(db.SubCities, "Id", "Name", hospital.SubCItyId);
            return View(hospital);
        }

        // GET: Hospital/Edit/5
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
            Hospital hospital = db.Hospitals.Find(id);
            if (hospital == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", hospital.CityId);
            ViewBag.SubCItyId = new SelectList(db.SubCities, "Id", "Name", hospital.SubCItyId);
            return View(hospital);
        }

        // POST: Hospital/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CityId,SubCItyId,Phone,WebSite,Specialization,Details")] Hospital hospital)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hospital).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", hospital.CityId);
            ViewBag.SubCItyId = new SelectList(db.SubCities, "Id", "Name", hospital.SubCItyId);
            return View(hospital);
        }

        // GET: Hospital/Delete/5
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
            Hospital hospital = db.Hospitals.Find(id);
            if (hospital == null)
            {
                return HttpNotFound();
            }
            return View(hospital);
        }

        // POST: Hospital/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hospital hospital = db.Hospitals.Find(id);
            db.Hospitals.Remove(hospital);
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
