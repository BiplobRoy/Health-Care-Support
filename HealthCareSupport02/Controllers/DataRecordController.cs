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
    public class DataRecordController : Controller
    {
        private dbHCS02Entities db = new dbHCS02Entities();

        // GET: DataRecord
        public ActionResult Index()
        {
            if (Session["Palogin"].ToString() == "")
            {
                return RedirectToAction("PaLogin");
            }
            int? p = ((Models.Patient)Session["Pa"]).Id;
            var dataRecords = db.DataRecords.Where(e => e.PatientId == p).Include(d => d.Patient);
            return View(dataRecords.ToList());
        }


        //DataRecord Search
        [HttpPost]
        public ActionResult Index(string search = "")
        {
            if (search != "")
            {
                var data = db.DataRecords.Include(d => d.Patient).Where(e => e.Details.ToLower().Contains(search.ToLower()));
                return View(data.ToList());
            }
            var dataRecords = db.DataRecords.Include(d => d.Patient);
            return View(dataRecords.ToList());
        }

        // GET: DataRecord/Details/5
        public ActionResult Details(int? id, int? p)
        {
            if (Session["Palogin"].ToString() == "")
            {
                return RedirectToAction("PaLogin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataRecord dataRecord = db.DataRecords.Find(id);
            if(((Models.Patient)Session["Pa"]).Id != p)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (dataRecord == null)
            {
                return HttpNotFound();
            }
            return View(dataRecord);
        }

        // GET: DataRecord/Create
        public ActionResult Create()
        {
            if (Session["Palogin"].ToString() == "")
            {
                return RedirectToAction("PaLogin");
            }
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Name");
            return View();
        }

        // POST: DataRecord/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PatientId,ProblemDate,Details")] DataRecord dataRecord, HttpPostedFileBase PrescriptionImg, HttpPostedFileBase ReportImag)
        {
            dataRecord.PrescriptionImg = System.IO.Path.GetFileName(PrescriptionImg.FileName);
            dataRecord.ReportImag = System.IO.Path.GetFileName(ReportImag.FileName);
            if (ModelState.IsValid)
            {
                db.DataRecords.Add(dataRecord);
                db.SaveChanges();

                string pi = Server.MapPath("../Upload/Prescription/" + dataRecord.Id.ToString() + "_" + dataRecord.PrescriptionImg);
                PrescriptionImg.SaveAs(pi);
                string ri = Server.MapPath("../Upload/Report/" + dataRecord.Id.ToString() + "_" + dataRecord.ReportImag);
                ReportImag.SaveAs(ri);
                return RedirectToAction("Index");
            }

            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Name", dataRecord.PatientId);
            return View(dataRecord);
        }

        // GET: DataRecord/Edit/5
        public ActionResult Edit(int? id, int? p)
        {
            if (Session["Palogin"].ToString() == "")
            {
                return RedirectToAction("PaLogin");
            }
            if (((Models.Patient)Session["Pa"]).Id != p)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataRecord dataRecord = db.DataRecords.Find(id);
            if (dataRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Name", dataRecord.PatientId);
            return View(dataRecord);
        }

        // POST: DataRecord/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PatientId,ProblemDate,PrescriptionImg,ReportImag,Details")] DataRecord dataRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dataRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Name", dataRecord.PatientId);
            return View(dataRecord);
        }

        // GET: DataRecord/Delete/5
        public ActionResult Delete(int? id, int? p)
        {
            if (Session["Palogin"].ToString() == "")
            {
                return RedirectToAction("PaLogin");
            }
            if (((Models.Patient)Session["Pa"]).Id != p)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["Palogin"].ToString() == "")
            {
                return RedirectToAction("PaLogin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataRecord dataRecord = db.DataRecords.Find(id);
            if (dataRecord == null)
            {
                return HttpNotFound();
            }
            return View(dataRecord);
        }

        // POST: DataRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DataRecord dataRecord = db.DataRecords.Find(id);
            db.DataRecords.Remove(dataRecord);
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
