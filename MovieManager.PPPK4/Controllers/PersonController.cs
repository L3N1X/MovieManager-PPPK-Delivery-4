using MovieManager.PPPK4.EntityFramework.Model;
using MovieManager.PPPK4.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Policy;

namespace MovieManager.PPPK4.Controllers
{
    public class PersonController : Controller
    {
        private readonly MovieContext db = new MovieContext();

        ~PersonController()
        {
            db.Dispose();
        }

        // GET: Movie
        public ActionResult Index()
        {
            return View(db.People);
        }

        public ActionResult GetPersonName(int id)
        {
            var person = db.People.Find(id);
            if (person is null)
                return Content("NO DIRECTOR", "text/plain");
            return Content(person.FirstName, "text/plain");
        }

        // GET: Movie/Details/5
        public ActionResult Details(int? id)
        {
            return CommonAction(id);
        }

        private ActionResult CommonAction(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var person = db.People
                .Include(m => m.Assets)
                .SingleOrDefault(m => m.Id == id);

            if (person == null)
            {
                return HttpNotFound();
            }

            return View(person);
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        public ActionResult Create(Person person, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                person.Assets = new List<Asset>();

                AddAssets(person, files);

                db.People.Add(person);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        private void AddAssets(Person person, IEnumerable<HttpPostedFileBase> files)
        {
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var picture = new Asset
                    {
                        Name = file.FileName,
                        ContentType = file.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        picture.Content = reader.ReadBytes(file.ContentLength);
                    }
                    person.Assets.Add(picture);
                }
            }
        }

        // GET: Movie/Edit/5
        public ActionResult Edit(int? id)
        {
            return CommonAction(id);
        }

        // POST: Movie/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, IEnumerable<HttpPostedFileBase> files)
        {
            var person = db.People.Find(id);

            if (TryUpdateModel(person, "", new string[] { nameof(Person.FirstName), nameof(Person.LastName) }))
            {
                AddAssets(person, files);
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(person);
        }

        // GET: Movie/Delete/5
        public ActionResult Delete(int? id)
        {
            return CommonAction(id);
        }

        // POST: Movie/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var person = db.People
                .Include(p => p.Assets)
                .FirstOrDefault(p => p.Id.Equals(id));

            db.Assets.RemoveRange(db.Assets.Where(a => person.Assets.Select(ma => ma.Id).Contains(a.Id)));
            db.People.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
