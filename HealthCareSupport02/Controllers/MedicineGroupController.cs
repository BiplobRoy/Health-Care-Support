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
    public class MedicineGroupController : Controller
    {
        private dbHCS02Entities db = new dbHCS02Entities();

        // GET: MedicineGroup
        public ActionResult Index()
        {
            if (Session["Adlogin"].ToString() == "")
            {
                return RedirectToAction("AdLogin");
            }
            return View(db.MedicineGroups.ToList());
        }

        // GET: MedicineGroup/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Adlogin"].ToString() == "")
            {
                return RedirectToAction("AdLogin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineGroup medicineGroup = db.MedicineGroups.Find(id);
            if (medicineGroup == null)
            {
                return HttpNotFound();
            }
            return View(medicineGroup);
        }

        // GET: MedicineGroup/Create
        public ActionResult Create()
        {
            if (Session["Adlogin"].ToString() == "")
            {
                return RedirectToAction("AdLogin");
            }
            return View();
        }

        // POST: MedicineGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] MedicineGroup medicineGroup)
        {
            if (ModelState.IsValid)
            {
                db.MedicineGroups.Add(medicineGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medicineGroup);
        }

        // GET: MedicineGroup/Edit/5
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
            MedicineGroup medicineGroup = db.MedicineGroups.Find(id);
            if (medicineGroup == null)
            {
                return HttpNotFound();
            }
            return View(medicineGroup);
        }

        // POST: MedicineGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] MedicineGroup medicineGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicineGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicineGroup);
        }

        // GET: MedicineGroup/Delete/5
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
            MedicineGroup medicineGroup = db.MedicineGroups.Find(id);
            if (medicineGroup == null)
            {
                return HttpNotFound();
            }
            return View(medicineGroup);
        }

        // POST: MedicineGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicineGroup medicineGroup = db.MedicineGroups.Find(id);
            db.MedicineGroups.Remove(medicineGroup);
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
