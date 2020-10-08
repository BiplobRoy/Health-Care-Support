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
    public class ApointmentController : Controller
    {
        private dbHCS02Entities db = new dbHCS02Entities();

        // GET: Apointment
        public ActionResult Index()
        {
            if (Session["Dologin"].ToString() == "" && Session["Palogin"].ToString() == "")
            {
                return RedirectToAction("PaLogin");
            }
            int? d = ((Models.Doctor)Session["Do"]).Id;
            int? p = ((Models.Patient)Session["Pa"]).Id;
            var apointments = db.Apointments.Where(s => s.DoctorId == d || s.PatientId == p).Include(s => s.Doctor).Include(s => s.Patient);
            Session["AP"] = "";
            return View(apointments.ToList());
        }

        // GET: Apointment/Details/5
        public ActionResult Details(int? id, int? p, int? d)
        {
            if (Session["Palogin"].ToString() == "" && Session["Dologin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apointment apointment = db.Apointments.Find(id);
            if (apointment == null)
            {
                return HttpNotFound();
            }
            if (((Models.Doctor)Session["Do"]).Id == d)
            {
                Session["Msg"] = "";
                return View(apointment);
            }
            if(((Models.Patient)Session["Pa"]).Id == p)
            {
                Session["Msg"] = "";
                return View(apointment);
            }
            Session["Msg"] = "No Information For You";
            return RedirectToAction("Index");
        }

        // GET: Apointment/Create
        public ActionResult Create(int? p, int? d)
        {
            if (Session["Palogin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (d == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", d);
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Name", p);
            return View();
        }

        // POST: Apointment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DoctorId,PatientId,ProblemName")] Apointment apointment, int p, int d)
        {
            if (ModelState.IsValid)
            {
                apointment.DoctorId = d;
                apointment.PatientId = p;
                db.Apointments.Add(apointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(apointment);
        }

        // GET: Apointment/Edit/5
        public ActionResult Edit(int? id, int? p)
        {
            if (Session["Palogin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (((Models.Patient)Session["Pa"]).Id != p)
            {
                Session["Msg"] = "You Have No Information";
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apointment apointment = db.Apointments.Find(id);
            if (apointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", apointment.DoctorId);
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Name", apointment.PatientId);
            return View(apointment);
        }

        // POST: Apointment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DoctorId,PatientId,ProblemName")] Apointment apointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", apointment.DoctorId);
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Name", apointment.PatientId);
            return View(apointment);
        }

        // GET: Apointment/Delete/5
        public ActionResult Delete(int? id, int? p)
        {
            if (Session["Palogin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (((Models.Patient)Session["Pa"]).Id != p)
            {
                Session["Msg"] = "You Have No Information";
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apointment apointment = db.Apointments.Find(id);
            if (apointment == null)
            {
                return HttpNotFound();
            }
            return View(apointment);
        }

        // POST: Apointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Apointment apointment = db.Apointments.Find(id);
            db.Apointments.Remove(apointment);
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
