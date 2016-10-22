using FindSomebody.Models;
using FindSomebody.ViewModels;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebTools;

namespace FindSomebody.Controllers
{
    /// <summary>
    /// Controller for people, enabling CRUD and search functionality.
    /// </summary>
    public class PeopleController : Controller
    {
        /// <summary>
        /// Local file path to profile photos.
        /// </summary>
        private string _photoPath = "/Uploads/Photos/";

        /// <summary>
        /// People database context.
        /// </summary>
        private PeopleDbContext _db = new PeopleDbContext();

        /// <summary>
        /// Constructor
        /// </summary>
        public PeopleController()
        {
            _db = new PeopleDbContext();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db">Person database</param>
        public PeopleController(PeopleDbContext db)
        {
            _db = db;
        }

        // GET: People
        /// <summary>
        /// Display root for People page. Shows a list of
        /// </summary>
        /// <param name="searchName"></param>
        /// <returns></returns>
        public ActionResult Index(string searchName)
        {
            var people = from m in _db.People
                         select m;

            if (!string.IsNullOrEmpty(searchName))
            {
                people = people.Where(x => x.Name.Contains(searchName));
            }

            people = people.OrderBy(x => x.Name);

            if (Request.IsAjaxRequest())
            {
                Thread.Sleep(Math.Min(2000, 50 * people.Count()));
                return PartialView("_People", people);
            }

            return View(people);
        }

        // Get: People/Autocomplete/
        public ActionResult Autocomplete(string term)
        {
            var people = _db.People.Where(x => x.Name.Contains(term)).Take(10).Select(x => x.Name);

            return Json(people, JsonRequestBehavior.AllowGet);
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = _db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View(new EditPersonViewModel());
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EditPerson,PhotoUpload")] EditPersonViewModel model)
        {
            ValidatePhoto(model.PhotoUpload);
            if (ModelState.IsValid)
            {
                UploadPhoto(model);
                _db.People.Add(model.EditPerson);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = _db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            var edit = new EditPersonViewModel() { EditPerson = person };
            return View(edit);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EditPerson,PhotoUpload")] EditPersonViewModel model)
        {
            ValidatePhoto(model.PhotoUpload);
            if (ModelState.IsValid)
            {
                UploadPhoto(model);
                _db.SetModified(model.EditPerson);
                _db.SaveChanges();
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
            Person person = _db.People.Find(id);
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
            Person person = _db.People.Find(id);
            _db.People.Remove(person);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Cleanup class on disposal.
        /// </summary>
        /// <param name="disposing">Is the class being displosed</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Sets a validation error if a file upload is not an image.
        /// </summary>
        /// <param name="fileUpload">Photo file upload.</param>
        private void ValidatePhoto(HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null && !FileConstants.DefaultImageTypes.Contains(fileUpload.ContentType))
            {
                ModelState.AddModelError("PhotoUpload", FileConstants.InvalidImageTypeError);
            }
        }

        /// <summary>
        /// Uploads and sets path for a photo.
        /// </summary>
        /// <param name="model">Edit person view model with photo upload.</param>
        private void UploadPhoto(EditPersonViewModel model)
        {
            var newPhoto = Files.UploadFile(Server, _photoPath, model.PhotoUpload);
            if (!string.IsNullOrEmpty(newPhoto))
            {
                model.EditPerson.Photo = newPhoto;
            }
        }
    }
}