using ImageGallary.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace ImageGallary.Controllers
{
    [Authorize]
    [HandleError]
    public class AdminController : Controller
    {
        #region Variable declaration
        /// <summary>The database</summary>
        readonly DBContext db = new DBContext();
        #endregion

        #region Display images
        /// <summary>Indexes Images.</summary>
        public ActionResult Index(int? page)
        {
            if (Session["UserRole"].ToString() != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            return View(db.Images.OrderBy(o => o.OrderNo).ToList().ToPagedList(page ?? 1, 5));
        }
        #endregion

        #region Partial views
        /// <summary>Images the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        public ActionResult Images(int? id)
        {
            if (id == null)
            {
                return View("Index");
            }

            return PartialView(db.Images.Where(w => w.ImageId == id).ToList());
        }

        /// <summary>Comments the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        public ActionResult Comments(int? id)
        {
            if (id == null)
            {
                return View("Index");
            }

            return PartialView(db.Comments.Include(e => e.UserDetail).Include(e => e.Image).Where(w => w.ImageId == id).OrderBy(o => o.CommentDate));
        }

        /// <summary>Tagses the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        public ActionResult Tags(int? id)
        {
            if (id == null)
            {
                return View("Index");
            }

            return PartialView(db.Tags.Include(e => e.UserDetail).Include(e => e.Image).Where(w => w.ImageId == id).OrderBy(o => o.TagDate));
        }
        #endregion

        #region Create method
        /// <summary>Creates this instance.</summary>
        public ActionResult Create()
        {
            if (Session["UserRole"].ToString() != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            return View();
        }

        /// <summary>Creates the specified image.</summary>
        /// <param name="image">The image.</param>
        [HttpPost]
        public ActionResult Create(Image image)
        {
            UploadFile(image);

            if (ModelState.IsValid)
            {
                image.UploadDate = DateTime.Now;
                db.Images.Add(image);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ModelState.Clear();
            return View(image);
        }
        #endregion

        #region Edit method
        /// <summary>Edits the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        public ActionResult Edit(int? id)
        {
            if (Session["UserRole"].ToString() != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Image image = db.Images.Find(id);
            return View(image);
        }

        /// <summary>Edits the specified image.</summary>
        /// <param name="image">The image.</param>
        [HttpPost]
        public ActionResult Edit(Image image)
        {
            Image imageFromDB = db.Images.Single(e => e.ImageId == image.ImageId);

            if (image.ImageFile != null)
            {
                UploadFile(image);
            }
            else
            {
                imageFromDB.Url = image.Url;
            }

            imageFromDB.OrderNo = image.OrderNo;
            imageFromDB.ImageTitle = image.ImageTitle;
            imageFromDB.UploadDate = DateTime.Now;
            imageFromDB.ImageId = image.ImageId;

            if (ModelState.IsValid)
            {
                using (db)
                {
                    db.Entry(imageFromDB).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
                return View(image);
        }
        #endregion

        #region Details method
        /// <summary>Detailses the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        public ActionResult Details(int? id)
        {
            if (Session["UserRole"].ToString() != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            if (id == null)
            {
                RedirectToAction("Index");
            }

            return View();
        }
        #endregion

        #region Delete method
        /// <summary>Deletes the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        public ActionResult Delete(int? id)
        {
            if (Session["UserRole"].ToString() != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Image employee = db.Images.Find(id);

            if (employee == null)
            {
                return HttpNotFound();
            }

            return View(employee);
        }

        /// <summary>Deletes the confirmed.</summary>
        /// <param name="id">The identifier.</param>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Image employee = db.Images.Find(id);
            db.Images.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region File upload method
        public ActionResult UploadFile(Image image)
        {
            string filename = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
            string extension = Path.GetExtension(image.ImageFile.FileName);

            filename = filename + DateTime.Now.ToString("dd/MM/yyyy") + extension;
            image.Url = "~/Images/" + filename;
            filename = Path.Combine(Server.MapPath("~/Images/"), filename);
            image.ImageFile.SaveAs(filename);

            return View(image);
        }
        #endregion
    }
}