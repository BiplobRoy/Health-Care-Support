using HealthCareSupport02.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HealthCareSupport02.Controllers
{
    public class MessegeController : Controller
    {
        private dbHCS02Entities db = new dbHCS02Entities();

        // GET: Messege
        public ActionResult Index()
        {
            int? d = ((Models.Doctor)Session["Do"]).Id;
            int? p = ((Models.Patient)Session["Pa"]).Id;
            var messeges = db.Messeges.Where(m => m.DoctorId == d || m.PatientId == p).Include(m => m.Doctor).Include(m => m.Patient);
            Session["NT"] = "";
            return View(messeges.ToList());
        }

        // GET: Messege/Details/5
        public ActionResult Details(int? id, int? p, int? d)
        {
            if (Session["Dologin"].ToString() == "" && Session["Palogin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messege messege = db.Messeges.Find(id);
            if (messege == null)
            {
                return HttpNotFound();
            }
            if(((Models.Patient)Session["Pa"]).Id == p)
            {
                return View(messege);
            }
            if (((Models.Doctor)Session["Do"]).Id == d)
            {
                return View(messege);
            }

            return RedirectToAction("PaLogin");
        }

        //GET: Messege/Create
        public ActionResult Create(int? p, int? d)
        {
            if (p == null || d == null)
            {
                return RedirectToAction("PaLogin");
            }
            var messeges = db.Messeges.Where(m => m.DoctorId == d || m.PatientId == p).Include(m => m.Doctor).Include(m => m.Patient);
            return View();
        }

        // POST: Messege/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProblemName,ProblemDate,Details")] Messege messege, HttpPostedFileBase File1, int p, int d)
        {
            messege.File1 = System.IO.Path.GetFileName(File1.FileName);
            //messege.File2 = System.IO.Path.GetFileName(File2.FileName);
            //messege.File3 = System.IO.Path.GetFileName(File3.FileName);
            //messege.File4 = System.IO.Path.GetFileName(File4.FileName);
            if (ModelState.IsValid)
            {
                messege.PatientId = p;
                messege.DoctorId = d;
                db.Messeges.Add(messege);
                db.SaveChanges();
                
                string f1 = Server.MapPath("../Upload/File/File1/" + messege.Id.ToString() + "_" + messege.PatientId.ToString()+ "_" + messege.File1);
                File1.SaveAs(f1);
                //string f2 = Server.MapPath("../Upload/File/File2/" + messege.Id.ToString() + "_" + messege.PatientId.ToString() + "_" + messege.File2);
                ////File2.SaveAs(f2);
                //string f3 = Server.MapPath("../Upload/File/File3/" + messege.Id.ToString() + "_" + messege.PatientId.ToString() + "_" + messege.File3);
                //File3.SaveAs(f3);
                //string f4 = Server.MapPath("../Upload/File/File4/" + messege.Id.ToString() + "_" + messege.PatientId.ToString() + "_" + messege.File4);
                //File4.SaveAs(f4);
                return RedirectToAction("Index");
            }
            return View(messege);
        }

        // GET: Messege/Edit/5
        public ActionResult Edit(int? id, int? p)
        {
            if(Session["Adlogin"].ToString() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messege messege = db.Messeges.Find(id);
            if (messege == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", messege.DoctorId);
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Name", messege.PatientId);
            return View(messege);
        }

        // POST: Messege/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DoctorId,PatientId,ProblemName,ProblemDate,Details,File1,File2,File3,File4")] Messege messege)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messege).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", messege.DoctorId);
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "Name", messege.PatientId);
            return View(messege);
        }

        // GET: Messege/Delete/5
        public ActionResult Delete(int? id, int? p)
        {
            if (Session["Palogin"].ToString() == "" || ((Models.Patient)Session["Pa"]).Id != p)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messege messege = db.Messeges.Find(id);
            if (messege == null)
            {
                return HttpNotFound();
            }
            return View(messege);
        }

        // POST: Messege/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Messege messege = db.Messeges.Find(id);
            db.Messeges.Remove(messege);
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
