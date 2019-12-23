using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LinqKit;
using OneTech.Models;

namespace OneTech.Controllers
{
    public class ClassesController : Controller
    {
        private readonly MyContext _db = new MyContext();

        // GET: Classes
        public ActionResult Index(string keyword)
        {
            var predicate = PredicateBuilder.New<Class>(true);
            if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrWhiteSpace(keyword))
            {
                predicate = predicate.And(f => f.Name.Contains(keyword));
                ViewBag.Keyword = keyword;
            }
            var data = _db.Classes.AsExpandable().Where(predicate);
            return View(data);
        }

        // GET: Classes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var @class = _db.Classes.Find(id);
            var predicate = PredicateBuilder.New<Student>(true);
            predicate = predicate.And(f => f.ClassId == id &&
                                           f.StudentStatus != Student.StudentStatusEnum.Deleted &&
                                           f.StudentStatus != Student.StudentStatusEnum.Inactive);
            ViewBag.StudentList = _db.Students.AsExpandable().Where(predicate);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // GET: Classes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Class @class)
        {
            if (ModelState.IsValid)
            {
                @class.CreatedAt = DateTime.Now;
                @class.UpdatedAt = DateTime.Now;
                _db.Classes.Add(@class);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@class);
        }

        // GET: Classes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var @class = _db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Class @class)
        {
            if (ModelState.IsValid)
            {
                @class.UpdatedAt = DateTime.Now;
                _db.Entry(@class).State = EntityState.Modified;
                try
                {
                    _db.SaveChanges();
                }
                catch (OptimisticConcurrencyException)
                {
                    Debug.WriteLine("error");
                }
                return RedirectToAction("Index");
            }
            return View(@class);
        }

        // GET: Classes/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Class @class = db.Classes.Find(id);
        //    if (@class == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(@class);
        //}

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var @class = _db.Classes.Find(id);
            if (@class != null) _db.Classes.Remove(@class);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        //GET: Attendance
        public ActionResult Attendance(int id)
        {
            var predicate = PredicateBuilder.New<Student>(true);
            predicate = predicate.And(f => f.ClassId == id && 
                                           f.StudentStatus != Student.StudentStatusEnum.Deleted && 
                                           f.StudentStatus != Student.StudentStatusEnum.Inactive);
            var data = _db.Students.AsExpandable().Where(predicate);
            ViewBag.Length = data.Count();
            ViewBag.ClassId = id;
            return View(data);
        }

        public JsonResult GetStudentInfo(int? id)
        {
            if (id == null)
            {
                return null;
            }
            var student = _db.Students.Find(id);
            return student != null
                ? Json(new
                {
                    id = student.Id,
                    fullName = student.FullName,
                    owedCash = student.OwedCash,
                    owedPushUp = student.OwedPushUp,
                    penaltyLevel = student.PenaltyLevel
                }, JsonRequestBehavior.AllowGet)
                : null;
        }
        public class AttendanceInfo
        {
            // ReSharper disable once InconsistentNaming
            public int id { get; set; }
            // ReSharper disable once InconsistentNaming
            [Range(1,2)]
            // 1 cash 2 push up
            public int penaltyType { get; set; }
            // ReSharper disable once InconsistentNaming
            public bool owed { get; set; }
            // ReSharper disable once InconsistentNaming
            public bool onTime { get; set; }
        }
        public ActionResult AttendanceConfirm(List<AttendanceInfo> attendances, int classId)
        {
            var redirectUrl = Url.Action("Details", "Classes", new {id = classId});
            if (attendances == null) return null;
            foreach (var item in attendances)
            {
                var student = _db.Students.Find(item.id);
                if (student == null) continue;
                if (!item.onTime)
                {
                    var level = student.PenaltyLevel + 1;
                    student.PenaltyLevel += 1;
                    var cash = 0;
                    var pushUp = 0;
                    Penalty.PenaltyEnum type;
                    if (item.penaltyType == 1)
                    {
                        cash = level * 10;
                        type = Penalty.PenaltyEnum.Cash;
                    }
                    else
                    {
                        pushUp = level * 20;
                        type = Penalty.PenaltyEnum.Pushup;
                    }

                    if (item.owed)
                    {
                        student.OwedCash += cash;
                        student.OwedPushUp += pushUp;
                    }
                    else
                    {
                        student.OwedCash = 0;
                        student.OwedPushUp = 0;
                    }

                    var penalty = new Penalty
                    {
                        StudentId = item.id,
                        PenaltyType = type,
                        PenaltyCash = cash,
                        PenaltyPushUp = pushUp
                    };
                    _db.Penalties.Add(penalty);

                    _db.Entry(student).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                else
                {
                    student.OwedCash = 0;
                    student.OwedPushUp = 0;
                    student.PenaltyLevel = 0;

                    _db.Entry(student).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }

            return Json(new { url = redirectUrl });
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
