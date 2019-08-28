using ImageGallary.Common;
using ImageGallary.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace ImageGallary.Controllers
{
    [Authorize]
    [HandleError]
    public class UserController : Controller
    {
        #region Variable Declarations
        /// <summary>The database</summary>
        readonly DBContext db = new DBContext();
        #endregion

        #region Index method
        /// <summary>Displays images from database.</summary>
        public ActionResult Index()
        {
            if (Session["UserRole"].ToString() != "2")
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            Image[] image = db.Images.OrderBy(o => o.OrderNo).ToArray();
            ViewBag.ImageDetails = image;
            return View(image);
        }
        #endregion

        #region Partial views
        /// <summary>Images the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        public ActionResult Images(int? id)
        {
            return PartialView(db.Images.Where(x => x.ImageId == id).ToList());
        }

        /// <summary>Comments the specified image identifier.</summary>
        /// <param name="imageId">The image identifier.</param>
        public ActionResult Comments(int? imageId)
        {
            return PartialView(db.Comments.Where(w => w.ImageId == imageId).ToList());
        }

        /// <summary>Tags the specified image identifier.</summary>
        /// <param name="imageId">The image identifier.</param>
        public ActionResult Tags(int? imageId)
        {
            return PartialView(db.Tags.Where(w => w.ImageId == imageId).ToList());
        }
        #endregion

        #region Add comments method
        /// <summary>Creates the comment.</summary>
        /// <param name="id">The identifier.</param>
        [HttpGet]
        public ActionResult CreateComment(int? id)
        {
            if (Session["UserRole"].ToString() != "2")
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            CommentViewModel commentViewModel = new CommentViewModel();
            commentViewModel.Image = new Image();
            commentViewModel.Image = db.Images.Find(id);
            commentViewModel.Comment = new Comment();

            return View(commentViewModel);
        }

        /// <summary>Creates the comment.</summary>
        /// <param name="comment">The comment.</param>
        /// <param name="image">The image.</param>
        [HttpPost]
        public ActionResult CreateComment(CommentViewModel commentViewModel)
        {
            commentViewModel.Comment.CommentDate = DateTime.Now;
            commentViewModel.Comment.UserId = Convert.ToInt32(Session["UserId"]);
            commentViewModel.Comment.ImageId = commentViewModel.Image.ImageId;
            commentViewModel.Image.Url = commentViewModel.Image.Url;
            commentViewModel.Image.ImageTitle = commentViewModel.Image.ImageTitle;

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                using (db)
                {
                    db.Comments.Add(commentViewModel.Comment);
                    db.SaveChanges();

                    var message = new MailMessage();
                    message.To.Add(new MailAddress("patilnikhil64@gmail.com"));
                    message.From = new MailAddress("aress.iphone5@gmail.com");
                    message.Subject = "Comment has been added.";
                    message.Body = "Hello Admin! " + Session["UserEmail"] + " has addedd comment to image as " + commentViewModel.Comment.Comment1 + ".";
                    message.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "aress.iphone5@gmail.com",
                            Password = "Aress123$"
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.Send(message);
                    }

                    return RedirectToAction("Index");
                }
            }

            return View(commentViewModel);
        }
        #endregion

        #region Add tags method
        /// <summary>Creates the tag.</summary>
        /// <param name="id">The identifier.</param>
        [HttpGet]
        public ActionResult CreateTag(int? id)
        {
            if (Session["UserRole"].ToString() != "2")
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            CommentViewModel commentViewModel = new CommentViewModel();
            commentViewModel.Image = new Image();
            commentViewModel.Image = db.Images.Find(id);
            commentViewModel.Tag = new Tag();

            return View(commentViewModel);
        }

        /// <summary>Creates the tag.</summary>
        /// <param name="tag">The tag.</param>
        /// <param name="image">The image.</param>
        [HttpPost]
        public ActionResult CreateTag(CommentViewModel commentViewModel)
        {
            commentViewModel.Tag.TagDate = DateTime.Now;
            commentViewModel.Tag.UserId = 2; //Convert.ToInt32(Session["UserId"]);
            commentViewModel.Tag.ImageId = commentViewModel.Image.ImageId;
            commentViewModel.Image.Url = commentViewModel.Image.Url;
            commentViewModel.Image.ImageTitle = commentViewModel.Image.ImageTitle;

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                using (db)
                {
                    db.Tags.Add(commentViewModel.Tag);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(commentViewModel);
        }
        #endregion
    }
}