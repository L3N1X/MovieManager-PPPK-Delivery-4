using MovieManager.PPPK4.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace MovieManager.PPPK4.Controllers
{
    public class AssetController : Controller
    {
        private readonly MovieContext db = new MovieContext();

        ~AssetController()
        {
            db.Dispose();
        }

        // GET: Asset
        public ActionResult Index(int id)
        {
            var assets = db.Assets.Find(id);
            return File(assets.Content, assets.ContentType);
        }

        public ActionResult Delete(int assetId, int movieId)
        {
            var movie = db.Movies
               .Include(m => m.Assets)
               .FirstOrDefault(m => m.Id.Equals(movieId));

            var assetToRemove = movie.Assets.FirstOrDefault(a => a.Id.Equals(assetId));
            movie.Assets.Remove(assetToRemove);

            db.Assets.Remove(assetToRemove);

            db.SaveChanges();
            return Redirect(Request.UrlReferrer.AbsolutePath);
        }
    }
}