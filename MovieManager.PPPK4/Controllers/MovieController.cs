using Microsoft.Ajax.Utilities;
using MovieManager.PPPK4.EntityFramework;
using MovieManager.PPPK4.EntityFramework.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieManager.PPPK4.Controllers
{
    public class MovieController : Controller
    {
        private readonly MovieContext db = new MovieContext();

        ~MovieController()
        {
            db.Dispose();
        }

        // GET: Movie
        public ActionResult Index()
        {
            return View(db.Movies.Include(m => m.Director));
        }

        // GET: Movie/Details/5
        public ActionResult Details(int? id)
        {
            return CommonAction(id);
        }

        [HttpPost]
        public ActionResult AddActor(int movieId, int actorId)
        {
            var movie = db.Movies.Include(m => m.Actors).FirstOrDefault(m => m.Id == movieId);
            var actor = db.People.Find(actorId);
            movie.Actors.Add(actor);
            db.SaveChanges();
            return Json(new { fullname = actor.FullName });
        }

        [HttpPost]
        public ActionResult RemoveActor(int movieId, int actorId)
        {
            var movie = db.Movies.Include(m => m.Actors).FirstOrDefault(m => m.Id == movieId);
            var actor = db.People.Find(actorId);
            movie.Actors.Remove(actor);
            db.SaveChanges();
            return Json(new { fullname = actor.FullName });
        }

        private ActionResult CommonAction(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var movie = db.Movies
                .Include(m => m.Assets)
                .Include(m => m.Actors)
                .SingleOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        public ActionResult Create(Movie movie, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                movie.Assets = new List<Asset>();

                AddAssets(movie, files);

                db.Movies.Add(movie);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        private void AddAssets(Movie movie, IEnumerable<HttpPostedFileBase> files)
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
                    movie.Assets.Add(picture);
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
            var movie = db.Movies.Find(id);

            if (TryUpdateModel(movie, "", new string[] { nameof(Movie.Name), nameof(Movie.Description), nameof(Movie.Assets) }))
            {
                AddAssets(movie, files);
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
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
            var movie = db.Movies
                .Include(m => m.Assets)
                .FirstOrDefault(m => m.Id.Equals(id));

            db.Assets.RemoveRange(db.Assets.Where(a => movie.Assets.Select(ma => ma.Id).Contains(a.Id)));
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
