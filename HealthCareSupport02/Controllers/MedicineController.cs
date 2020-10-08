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
    public class MedicineController : Controller
    {
        private dbHCS02Entities db = new dbHCS02Entities();

        // GET: Medicine
        public ActionResult Index()
        {
            ViewBag.MedicineGroupId = new SelectList(db.MedicineGroups, "Id", "Name");
            var medicines = db.Medicines.Include(m => m.Company).Include(m => m.MedicineGroup);
            return View(medicines.ToList());
        }


        //search medicine
        [HttpPost]
        public ActionResult Index(int? MedicineGroupId, string search = "")
        {
            if(search != "")
            {
                ViewBag.MedicineGroupId = new SelectList(db.MedicineGroups, "Id", "Name");
                var med = db.Medicines.Include(m => m.Company).Include(m => m.MedicineGroup).Where(e => e.Name.ToLower().Contains(search.ToLower()));
                return View(med.ToList());
            }
            if(MedicineGroupId != null)
            {
                ViewBag.MedicineGroupId = new SelectList(db.MedicineGroups, "Id", "Name");
                var med = db.Medicines.Include(m => m.Company).Include(m => m.MedicineGroup).Where(e => e.MedicineGroupId == MedicineGroupId);
                return View(med.ToList());
            }
            ViewBag.MedicineGroupId = new SelectList(db.MedicineGroups, "Id", "Name");
            var medicines = db.Medicines.Include(m => m.Company).Include(m => m.MedicineGroup);
            return View(medicines.ToList());
        }

        // GET: Medicine/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            return View(medicine);
        }

        // GET: Medicine/Create
        public ActionResult Create()
        {
            if (Session["Adlogin"].ToString() == "")
            {
                return RedirectToAction("AdLogin");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name");
            ViewBag.MedicineGroupId = new SelectList(db.MedicineGroups, "Id", "Name");
            return View();
        }

        // POST: Medicine/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,MedicineGroupId,CompanyId,Details")] Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                db.Medicines.Add(medicine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", medicine.CompanyId);
            ViewBag.MedicineGroupId = new SelectList(db.MedicineGroups, "Id", "Name", medicine.MedicineGroupId);
            return View(medicine);
        }

        // GET: Medicine/Edit/5
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
            Medicine medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", medicine.CompanyId);
            ViewBag.MedicineGroupId = new SelectList(db.MedicineGroups, "Id", "Name", medicine.MedicineGroupId);
            return View(medicine);
        }

        // POST: Medicine/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,MedicineGroupId,CompanyId,Details")] Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", medicine.CompanyId);
            ViewBag.MedicineGroupId = new SelectList(db.MedicineGroups, "Id", "Name", medicine.MedicineGroupId);
            return View(medicine);
        }

        // GET: Medicine/Delete/5
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
            Medicine medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            return View(medicine);
        }

        // POST: Medicine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medicine medicine = db.Medicines.Find(id);
            db.Medicines.Remove(medicine);
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
