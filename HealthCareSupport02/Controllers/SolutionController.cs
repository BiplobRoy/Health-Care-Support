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
    public class SolutionController : Controller
    {
        private dbHCS02Entities db = new dbHCS02Entities();

        // GET: Solution
        public ActionResult Index()
        {
            int? d = ((Models.Doctor)Session["Do"]).Id;
            int? p = ((Models.Patient)Session["Pa"]).Id;
            var solutions = db.Solutions.Where(s => s.DoctorId == d || s.PatientId == p).Include(s => s.Doctor).Include(s => s.Patient);
            Session["NT"] = "";
            return View(solutions.ToList());
        }

        // GET: Solution/Details/5
        public ActionResult Details(int? id, int? p, int? d)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solution solution = db.Solutions.Find(id);
            if (solution == null)
            {
                return HttpNotFound();
            }
            if (((Models.Patient)Session["Pa"]).Id == p)
            {
                return View(solution);
            }
            if (((Models.Doctor)Session["Do"]).Id == d)
            {
                return View(solution);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Solution/Create
        public ActionResult Create(int? p, int? d)
        {
            if(Session["Dologin"].ToString() == "")
            {
                return RedirectToAction("PaLogin");
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", d);
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Name", p);
            return View();
        }

        // POST: Solution/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DoctorId,PatientId,ProblemName,Solution1, File1, File2, File3, File4")] Solution solution, int? dd, int p, int d)
        {
            if (ModelState.IsValid)
            {
                solution.DoctorId = d;
                solution.PatientId = p;

                db.Solutions.Add(solution);
                db.SaveChanges();

                Messege messege = db.Messeges.Find(dd);
                db.Messeges.Remove(messege);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(solution);
        }

        // GET: Solution/Edit/5
        public ActionResult Edit(int? id, int? d)
        {
            if (Session["Adlogin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solution solution = db.Solutions.Find(id);
            if (solution == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", solution.DoctorId);
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Name", solution.PatientId);
            return View(solution);
        }

        // POST: Solution/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DoctorId,PatientId,ProblemName,Solution1,File1,File2,File3,File4")] Solution solution)
        {
            if (ModelState.IsValid)
            {
                db.Entry(solution).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", solution.DoctorId);
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Name", solution.PatientId);
            return View(solution);
        }

        // GET: Solution/Delete/5
        public ActionResult Delete(int? id, int ? p)
        {
            if (id == null || ((Models.Patient)Session["Pa"]).Id != p)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solution solution = db.Solutions.Find(id);
            if (solution == null)
            {
                return HttpNotFound();
            }
            return View(solution);
        }

        // POST: Solution/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Solution solution = db.Solutions.Find(id);
            db.Solutions.Remove(solution);
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
