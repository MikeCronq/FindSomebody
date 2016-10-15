using FindSomebody.Models;
using FindSomebody.ViewModels;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FindSomebody.Controllers
{
    public class PeopleController : Controller
    {
        private string photoPath = "/Uploads/Photos/";

        private PeopleDbContext db = new PeopleDbContext();

        // GET: People
        public ActionResult Index(string searchName)
        {
            var people = from m in db.People
                         select m;

            if (!string.IsNullOrEmpty(searchName))
            {
                people = people.Where(x => x.Name.Contains(searchName));
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_People", people);
            }

            return View(people);
        }

        // Get: People/Autocomplete/
        public ActionResult Autocomplete(string term)
        {
            var people = db.People.Where(x => x.Name.StartsWith(term)).Take(10).Select(x => new { label = x.Name });

            return Json(people, JsonRequestBehavior.AllowGet);
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Person,PhotoUpload")] EditPersonViewModel viewModel)
        {
            CheckValidImageType(viewModel.PhotoUpload);
            if (ModelState.IsValid)
            {
                UploadPhoto(viewModel);
                db.People.Add(viewModel.EditPerson);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        /// <summary>
        /// Adds a model state error if the file is not an image type.
        /// </summary>
        /// <param name="fileUpload">Image upload to check.</param>
        private void CheckValidImageType(HttpPostedFileBase fileUpload)
        {
            var validTypes = new[] { "image/jpeg", "image/pjpeg", "image/png", "image/gif" };
            if (!validTypes.Contains(fileUpload.ContentType))
            {
                ModelState.AddModelError("PhotoUpload", "Please upload either a JPG, GIF, or PNG image.");
            }
        }

        /// <summary>
        /// Upload the file if it is valid
        /// </summary>
        /// <param name="model"></param>
        private void UploadPhoto(EditPersonViewModel model)
        {
            if (model.PhotoUpload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(model.PhotoUpload.FileName);
                var path = Path.Combine(Server.MapPath(photoPath), fileName);
                model.PhotoUpload.SaveAs(path);
                model.EditPerson.Photo = photoPath + fileName;
            }
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(new EditPersonViewModel() { EditPerson = person });
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EditPerson,PhotoUpload")] EditPersonViewModel model)
        {
            CheckValidImageType(model.PhotoUpload);
            if (ModelState.IsValid)
            {
                UploadPhoto(model);
                db.Entry(model.EditPerson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
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