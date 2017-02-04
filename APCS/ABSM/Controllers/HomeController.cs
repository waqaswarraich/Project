using ABSM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ABSM.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.TotalUser=db.Users.Count();
            ViewBag.TotalShops = db.Shops.Count();
            ViewBag.TotalOrder = db.Orders.Count();
            ViewBag.TotalCategories = db.Categories.Count();
            

            return View();
        }

        public ActionResult Rates()
        {
            var rateLists = db.RateLists.Include(r => r.Category).Include(r => r.City).Include(r => r.Product).Include(r => r.City);

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName");

            return View(rateLists.ToList());
        }
        [HttpPost]
        public ActionResult Rates(int? CategoryID,int? CityID ,string searchString)
        {

            var rateLists = db.RateLists.Include(r => r.Category).Include(r => r.City).Include(r => r.Product).Include(r => r.City);

            if (CategoryID != null && CityID!=null)
            {
                rateLists = db.RateLists.Include(r => r.Category).Include(r => r.City).Include(r => r.Product).Include(r => r.City).Where(x => x.CategoryID == CategoryID & x.CityID==CityID);
            }

            

            if (!string.IsNullOrEmpty(searchString))
            {
                rateLists = db.RateLists.Include(r => r.Category).Include(r => r.City).Include(r => r.Product).Include(r => r.City).Where(s => s.Product.Name.Contains(searchString));
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName");

            return View(rateLists.ToList());
        }


        [Authorize(Roles = "User")]
        public ActionResult Complain(string Msg)
        {

            ViewBag.Message = Msg;
            return View();
        }

        [HttpPost]
        public ActionResult Complain(GeneralComplain complain, HttpPostedFileBase doc, string Message)
        {
            string path;
            complain.Status = "Pending";
            if (doc != null)
            {

                var filename = Path.GetFileName(doc.FileName);
                var extension = Path.GetExtension(filename).ToLower();
                if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                {
                    path = HostingEnvironment.MapPath(Path.Combine("~/Content/Images/", filename));
                    doc.SaveAs(path);
                    complain.ImageUrl = "~/Content/Images/" + filename;
                }
                else
                {
                    ModelState.AddModelError("", "Document size must be less then 5MB");
                    return View(complain);
                }
                if (ModelState.IsValid)
                {

                    complain.Email =User.Identity.Name;
                    db.GeneralComplains.Add(complain);
                    db.SaveChanges();
                    Message = "Complain Registered";

                    return RedirectToAction("Complain","Home",new { Msg = Message } );
                }


            }

            ModelState.AddModelError("", "Please upload image");
            return View(complain);
        }


        public ActionResult Contact()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Contact(Contact model)
        {

            if (ModelState.IsValid)
            {
                db.Contacts.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index","Store");
            }

            return View(model);
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