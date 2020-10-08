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
    public class PatientController : Controller
    {
        private dbHCS02Entities db = new dbHCS02Entities();

        // GET: Patient
        public ActionResult Index()
        {
            if(Session["Adlogin"].ToString() == "")
            {
                return RedirectToAction("AdLogin");
            }
            var patients = db.Patients.Include(p => p.BloodGroup).Include(p => p.City).Include(p => p.Gender).Include(p => p.SubCity);
            return View(patients.ToList());
        }

        public ActionResult LogOut()
        {
            Session["Pa"] = new Models.Patient();
            Session["Palogin"] = "";
            Session["msg"] = "";
            Session["Dng"] = "";
            Session["NT"] = "";
            Session["AP"] = "";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult PaLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PaLogin(Models.Patient pa)
        {
            var v = db.Patients.Where(e => e.Email.ToLower() == pa.Email && e.PassWord == pa.PassWord).ToList();

            if (v.Count <= 0)
            {

                Session["Dng"] = "You Are Not An Admin. Denger Request";
                Session["msg"] = "Invalid Login Please Check Your Email Or PassWard";
                return View(pa);
            }

            Session["Pa"] = v.First();
            Session["Palogin"] = "True";

            return RedirectToAction("Index", "Home");
        }

        // GET: Patient/Details/5
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            int? I = ((Models.Patient)Session["Pa"]).Id;
            if (I == id || Session["Dologin"].ToString() != "" || Session["Adlogin"].ToString() != "")
            {
                //Notification For Messege
                //Notification For Messege
                int? dd = ((Models.Patient)Session["Pa"]).Id;
                var v = (db.Solutions.Where(e => e.PatientId == dd)).ToList();
                if (v.Count > 0)
                {
                    var n = v.Count;
                    Session["NT"] = "Sulution";
                }

                //Notification For Apointment
                //Notification For Apointment
                var b = (db.ApointmentFixeds.Where(a => a.PatientId == dd)).ToList();
                if (b.Count > 0)
                {
                    Session["AP"] = "Apointment Fixed";
                }
                return View(patient);
            }

            return RedirectToAction("PaLogin");
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name");
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name");
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name");
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name");
            return View();
        }

        // POST: Patient/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DateOfBirth,GenderId,BloodGroupId,CityId,SubCityId,NId,Skype,Phone,Email,PassWord")] Patient patient, HttpPostedFileBase PatientPhoto)
        {
            if (ModelState.IsValid)
            {
                patient.PatientPhoto = System.IO.Path.GetFileName(PatientPhoto.FileName);
                db.Patients.Add(patient);
                db.SaveChanges();

                string pa = Server.MapPath("../Upload/PatientPhoto/" + patient.Id.ToString() + "_" + patient.PatientPhoto);
                PatientPhoto.SaveAs(pa);
                return RedirectToAction("PaLogin");
            }

            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name", patient.BloodGroupId);
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", patient.CityId);
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", patient.GenderId);
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name", patient.SubCityId);
            return View(patient);
        }

        // GET: Patient/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || ((Models.Patient)Session["Pa"]).Id != id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name", patient.BloodGroupId);
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", patient.CityId);
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", patient.GenderId);
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name", patient.SubCityId);
            return View(patient);
        }

        // POST: Patient/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DateOfBirth,GenderId,BloodGroupId,CityId,SubCityId,NId,Skype,Phone,Email,PassWord,PatientPhoto")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name", patient.BloodGroupId);
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", patient.CityId);
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", patient.GenderId);
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name", patient.SubCityId);
            return View(patient);
        }

        // GET: Patient/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null || ((Models.Patient)Session["Pa"]).Id != id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
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
