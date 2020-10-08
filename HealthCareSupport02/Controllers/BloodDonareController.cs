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
    public class BloodDonareController : Controller
    {
        private dbHCS02Entities db = new dbHCS02Entities();

        // GET: BloodDonare
        public ActionResult Index()
        {
            if (Session["Adlogin"].ToString() == "" && Session["Dologin"].ToString() == "" && Session["Palogin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name");
            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name");
            var bloodDonares = db.BloodDonares.Include(b => b.BloodGroup).Include(b => b.City).Include(b => b.Gender).Include(b => b.SubCity);
            return View(bloodDonares.ToList());
        }

        // Search index
        [HttpPost]
        public ActionResult Index(int? SubCityId, int? BloodGroupId)
        {
            if (Session["Adlogin"].ToString() == "" && Session["Dologin"].ToString() == "" && Session["Palogin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (SubCityId != null && BloodGroupId != null)
            {
                ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name");
                ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name");
                var bloodDonares = db.BloodDonares.Include(b => b.BloodGroup).Include(b => b.City).Include(b => b.Gender).Include(b => b.SubCity).Where(b => b.SubCityId == SubCityId && b.BloodGroupId == BloodGroupId);
                return View(bloodDonares.ToList());
            }

            if (BloodGroupId != null)
            {
                ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name");
                ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name");
                var bloodDonares = db.BloodDonares.Include(b => b.BloodGroup).Include(b => b.City).Include(b => b.Gender).Include(b => b.SubCity).Where(b => b.BloodGroupId == BloodGroupId);
                return View(bloodDonares.ToList());
            }

            if (SubCityId != null)
            {
                ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name");
                ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name");
                var bloodDonares = db.BloodDonares.Include(b => b.BloodGroup).Include(b => b.City).Include(b => b.Gender).Include(b => b.SubCity).Where(b => b.SubCityId == SubCityId);
                return View(bloodDonares.ToList());
            }

            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name");
            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name");
            var v = db.BloodDonares.Include(b => b.BloodGroup).Include(b => b.City).Include(b => b.Gender).Include(b => b.SubCity);
            return View(v.ToList());
            //var bloodDonares = db.BloodDonares.Include(b => b.BloodGroup).Include(b => b.City).Include(b => b.Gender).Include(b => b.SubCity);
            //return View(bloodDonares.ToList());
        }

        // GET: BloodDonare/Details/5
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
            BloodDonare bloodDonare = db.BloodDonares.Find(id);
            if (bloodDonare == null)
            {
                return HttpNotFound();
            }
            return View(bloodDonare);
        }

        // GET: BloodDonare/Create
        public ActionResult Create()
        {
            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name");
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name");
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name");
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name");
            return View();
        }

        // POST: BloodDonare/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,GenderId,BloodGroupId,CityId,SubCityId,Phone")] BloodDonare bloodDonare)
        {
            if (ModelState.IsValid)
            {
                db.BloodDonares.Add(bloodDonare);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name", bloodDonare.BloodGroupId);
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", bloodDonare.CityId);
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", bloodDonare.GenderId);
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name", bloodDonare.SubCityId);
            return View(bloodDonare);
        }

        // GET: BloodDonare/Edit/5
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
            BloodDonare bloodDonare = db.BloodDonares.Find(id);
            if (bloodDonare == null)
            {
                return HttpNotFound();
            }
            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name", bloodDonare.BloodGroupId);
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", bloodDonare.CityId);
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", bloodDonare.GenderId);
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name", bloodDonare.SubCityId);
            return View(bloodDonare);
        }

        // POST: BloodDonare/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,GenderId,BloodGroupId,CityId,SubCityId,Phone")] BloodDonare bloodDonare)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bloodDonare).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name", bloodDonare.BloodGroupId);
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", bloodDonare.CityId);
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", bloodDonare.GenderId);
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name", bloodDonare.SubCityId);
            return View(bloodDonare);
        }

        // GET: BloodDonare/Delete/5
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
            BloodDonare bloodDonare = db.BloodDonares.Find(id);
            if (bloodDonare == null)
            {
                return HttpNotFound();
            }
            return View(bloodDonare);
        }

        // POST: BloodDonare/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BloodDonare bloodDonare = db.BloodDonares.Find(id);
            db.BloodDonares.Remove(bloodDonare);
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
