using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LinqKit;
using OneTech.Models;

namespace OneTech.Controllers
{
    public class StudentsController : Controller
    {
        private readonly MyContext _db = new MyContext();

        // GET: Students
        public ActionResult Index()
        {
            var predicate = PredicateBuilder.New<Student>(true);
            predicate = predicate.And(f => f.StudentStatus != Student.StudentStatusEnum.Deleted);
            var data = _db.Students.AsExpandable().Where(predicate);
            return View(data);
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = _db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.ClassId = new SelectList(_db.Classes, "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FullName,StudentCode,ClassId,OwedCash,OwedPushUp,PenaltyLevel,Birthday,CreatedAt,UpdatedAt,DeletedAt,StudentStatus")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.StudentCode = "SC" + student.Id;
                _db.Students.Add(student);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassId = new SelectList(_db.Classes, "Id", "Name", student.ClassId);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = _db.Students.Find(id);
            if (student == null || student.StudentStatus == Student.StudentStatusEnum.Deleted)
            {
                return HttpNotFound();
            }
            ViewBag.ClassId = new SelectList(_db.Classes, "Id", "Name", student.ClassId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,StudentCode,ClassId,OwedCash,OwedPushUp,PenaltyLevel,Birthday,CreatedAt,UpdatedAt,DeletedAt")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.UpdatedAt = DateTime.Now;
                if (student.StudentStatus == Student.StudentStatusEnum.Deleted)
                {
                    student.DeletedAt = DateTime.Now;
                }
                _db.Entry(student).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassId = new SelectList(_db.Classes, "Id", "Name", student.ClassId);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = _db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var student = _db.Students.Find(id);
            if (student != null && ModelState.IsValid)
            {
                student.UpdatedAt = DateTime.Now;
                student.DeletedAt = DateTime.Now;
                student.StudentStatus = Student.StudentStatusEnum.Deleted;
                _db.Entry(student).State = EntityState.Modified;
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
