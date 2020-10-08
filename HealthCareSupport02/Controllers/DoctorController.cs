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
    public class DoctorController : Controller
    {
        private dbHCS02Entities db = new dbHCS02Entities();

        // GET: Doctor
        public ActionResult Index()
        {
            if (Session["Adlogin"].ToString() == "" && Session["Palogin"].ToString() == "")
            {
                return RedirectToAction("PaLogin");
            }
            ViewBag.SpecializationId = new SelectList(db.Specializations, "Id", "Name");
            var doctors = db.Doctors.Include(d => d.BloodGroup).Include(d => d.City).Include(d => d.Gender).Include(d => d.Specialization).Include(d => d.SubCity);
            return View(doctors.ToList());
        }

        // Doctor search
        [HttpPost]
        public ActionResult Index(int? SpecializationId, string search = "")
        {
            if (SpecializationId != null && search != "")
            {
                ViewBag.SpecializationId = new SelectList(db.Specializations, "Id", "Name");
                var doc = db.Doctors.Include(d => d.BloodGroup).Include(d => d.City).Include(d => d.Gender).Include(d => d.Specialization).Include(d => d.SubCity).Where(e => e.Hospital.ToLower().Contains(search.ToLower()) && e.SpecializationId == SpecializationId);
                return View(doc.ToList());
            }

            if (SpecializationId != null)
            {
                ViewBag.SpecializationId = new SelectList(db.Specializations, "Id", "Name");
                var doc = db.Doctors.Include(d => d.BloodGroup).Include(d => d.City).Include(d => d.Gender).Include(d => d.Specialization).Include(d => d.SubCity).Where(b => b.SpecializationId == SpecializationId);
                return View(doc.ToList());
            }

            if (search != "")
            {
                ViewBag.SpecializationId = new SelectList(db.Specializations, "Id", "Name");
                var doc = db.Doctors.Include(d => d.BloodGroup).Include(d => d.City).Include(d => d.Gender).Include(d => d.Specialization).Include(d => d.SubCity).Where(e => e.Hospital.ToLower().Contains(search.ToLower()));
                return View(doc.ToList());
            }

            ViewBag.SpecializationId = new SelectList(db.Specializations, "Id", "Name");
            var doctors = db.Doctors.Include(d => d.BloodGroup).Include(d => d.City).Include(d => d.Gender).Include(d => d.Specialization).Include(d => d.SubCity);
            return View(doctors.ToList());
        }

        public ActionResult LogOut()
        {
            Session["Do"] = new Models.Doctor();
            Session["Dologin"] = "";
            Session["msg"] = "";
            Session["Dng"] = "";
            Session["NT"] = "";
            Session["AP"] = "";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DoLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoLogin(Models.Doctor doc)
        {
            var v = db.Doctors.Where(e => e.Email.ToLower() == doc.Email && e.PassWord == doc.PassWord).ToList();

            if (v.Count <= 0)
            {

                Session["Dng"] = "You Are Not An Admin. Denger Request";
                Session["msg"] = "Invalid Login Please Check Your Email Or PassWard";
                return View(doc);
            }

            Session["Do"] = v.First();
            Session["Dologin"] = "True";
            return RedirectToAction("Index", "Home");
        }

        // GET: Doctor/Details/5
        public ActionResult Details(int? id, Messege messege)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            int? I = ((Models.Doctor)Session["Do"]).Id;
            if (I == id || Session["Palogin"].ToString() != "" || Session["Adlogin"].ToString() != "")
            {
                //Notification For Messege
                //Notification For Messege
                int? dd = ((Models.Doctor)Session["Do"]).Id;
                var v = (db.Messeges.Where(e => e.DoctorId == dd)).ToList();
                if (v.Count > 0)
                {
                    var n = v.Count;
                    Session["NT"] = "Messeges";
                }

                //Notification For Appointment
                //Notification For Appointment
                var b = (db.Apointments.Where(a => a.DoctorId == dd)).ToList();
                if (b.Count > 0)
                {
                    Session["AP"] = "Appointment Request";
                }
                return View(doctor);
            }
           
            return RedirectToAction("DoLogin");
        }

        
        // GET: Doctor/Create
        public ActionResult Create()
        {
            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name");
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name");
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name");
            ViewBag.SpecializationId = new SelectList(db.Specializations, "Id", "Name");
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name");
            return View();
        }

        // POST: Doctor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DateOfBirth,GenderId,BloodGroupId,BMDC_NO,SpecializationId,CityId,SubCityId,NId,Phone,Skype,Hospital,ChemberAddress,Email,PassWord")] Doctor doctor, HttpPostedFileBase DoctorPhoto)
        {
            doctor.DoctorPhoto = System.IO.Path.GetFileName(DoctorPhoto.FileName);
            if (ModelState.IsValid)
            {
                db.Doctors.Add(doctor);
                db.SaveChanges();

                //SendMail(doctor.Email, doctor.PassWord);

                string doc = Server.MapPath("../Upload/DoctorPhoto/" + doctor.Id.ToString() + "_" + doctor.DoctorPhoto);
                DoctorPhoto.SaveAs(doc);
                Session["msg"] = "Your Account Need Admin Approval Please Wait 24 Hours Then Login";
                return RedirectToAction("DoLogin");
            }

            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name", doctor.BloodGroupId);
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", doctor.CityId);
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", doctor.GenderId);
            ViewBag.SpecializationId = new SelectList(db.Specializations, "Id", "Name", doctor.SpecializationId);
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name", doctor.SubCityId);
            return View(doctor);
        }


        //Email Verification For Doctor
        [NonAction]
        //public void SendMail(string EmailId, string Activation)
        //{
        //    var scheme = Request.Url.Scheme;
        //    var host = Request.Url.Host;

        //}




        // GET: Doctor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Dologin"].ToString() == "")
            {
                return RedirectToAction("DoLogin");
            }
            if (((Models.Doctor)Session["Do"]).Id != id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name", doctor.BloodGroupId);
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", doctor.CityId);
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", doctor.GenderId);
            ViewBag.SpecializationId = new SelectList(db.Specializations, "Id", "Name", doctor.SpecializationId);
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name", doctor.SubCityId);
            return View(doctor);
        }

        // POST: Doctor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DateOfBirth,GenderId,BloodGroupId,BMDC_NO,SpecializationId,CityId,SubCityId,NId,Phone,Skype,Hospital,ChemberAddress,Email,PassWord,DoctorPhoto")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "Id", "Name", doctor.BloodGroupId);
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", doctor.CityId);
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", doctor.GenderId);
            ViewBag.SpecializationId = new SelectList(db.Specializations, "Id", "Name", doctor.SpecializationId);
            ViewBag.SubCityId = new SelectList(db.SubCities, "Id", "Name", doctor.SubCityId);
            return View(doctor);
        }

        // GET: Doctor/Delete/5
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
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            db.Doctors.Remove(doctor);
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
