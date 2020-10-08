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
    public class ApointmentFixedController : Controller
    {
        private dbHCS02Entities db = new dbHCS02Entities();

        // GET: ApointmentFixed
        public ActionResult Index()
        {
            if (Session["Dologin"].ToString() == "" && Session["Palogin"].ToString() == "")
            {
                return RedirectToAction("PaLogin");
            }
            int? d = ((Models.Doctor)Session["Do"]).Id;
            int? p = ((Models.Patient)Session["Pa"]).Id;
            var apointmentFixeds = db.ApointmentFixeds.Where(s => s.DoctorId == d || s.PatientId == p).Include(s => s.Doctor).Include(s => s.Patient);

            //var apointmentFixeds = db.ApointmentFixeds.Include(a => a.Doctor).Include(a => a.Patient);
            Session["AP"] = "";
            return View(apointmentFixeds.ToList());
        }

        // GET: ApointmentFixed/Details/5
        public ActionResult Details(int? id, int? p, int? d)
        {
            if (Session["Dologin"].ToString() == "" && Session["Palogin"].ToString() == "")
            {
                return RedirectToAction("PaLogin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApointmentFixed apointmentFixed = db.ApointmentFixeds.Find(id);
            if (apointmentFixed == null)
            {
                return HttpNotFound();
            }
            if (((Models.Patient)Session["Pa"]).Id == p)
            {
                Session["Msg"] = "";
                return View(apointmentFixed);
            }
            if (((Models.Doctor)Session["Do"]).Id == d)
            {
                Session["Msg"] = "";
                return View(apointmentFixed);
            }
            Session["Msg"] = "No Information For You";
            return RedirectToAction("Index");
        }

        // GET: ApointmentFixed/Create
        public ActionResult Create(int? p, int? d)
        {

            if (Session["Dologin"].ToString() == "")
            {
                return RedirectToAction("PaLogin");
            }
            if (p == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", d);
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Name", p);
            return View();
        }

        // POST: ApointmentFixed/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DoctorId,PatientId,Location,Date,Time")] ApointmentFixed apointmentFixed, int? dd, int p, int d)
        {
            if (ModelState.IsValid)
            {
                apointmentFixed.DoctorId = d;
                apointmentFixed.PatientId = p;
                db.ApointmentFixeds.Add(apointmentFixed);
                db.SaveChanges();

                Apointment apointment = db.Apointments.Find(dd);
                db.Apointments.Remove(apointment);
                db.SaveChanges();
                //return RedirectToAction("Index");


                return RedirectToAction("Index");
            }
            return View(apointmentFixed);
        }

        // GET: ApointmentFixed/Edit/5
        public ActionResult Edit(int? id, int? d)
        {
            if (Session["Dologin"].ToString() == "")
            {
                return RedirectToAction("PaLogin");
            }
            if (((Models.Patient)Session["Do"]).Id != d)
            {
                Session["Msg"] = "You Have No Information";
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApointmentFixed apointmentFixed = db.ApointmentFixeds.Find(id);
            if (apointmentFixed == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", apointmentFixed.DoctorId);
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Name", apointmentFixed.PatientId);
            return View(apointmentFixed);
        }

        // POST: ApointmentFixed/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DoctorId,PatientId,Location,Date,Time")] ApointmentFixed apointmentFixed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apointmentFixed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", apointmentFixed.DoctorId);
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Name", apointmentFixed.PatientId);
            return View(apointmentFixed);
        }

        // GET: ApointmentFixed/Delete/5
        public ActionResult Delete(int? id, int? d)
        {
            if (Session["Dologin"].ToString() == "")
            {
                return RedirectToAction("PaLogin");
            }
            if (((Models.Patient)Session["Do"]).Id != d)
            {
                Session["Msg"] = "You Have No Information";
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApointmentFixed apointmentFixed = db.ApointmentFixeds.Find(id);
            if (apointmentFixed == null)
            {
                return HttpNotFound();
            }
            return View(apointmentFixed);
        }

        // POST: ApointmentFixed/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApointmentFixed apointmentFixed = db.ApointmentFixeds.Find(id);
            db.ApointmentFixeds.Remove(apointmentFixed);
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
