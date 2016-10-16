using FindSomebody.Models;
using FindSomebody.ViewModels;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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

        public PeopleController()
        {
            _db = new PeopleDbContext();
        }

        public PeopleController(PeopleDbContext db)
        {
            _db = db;
        }

        // GET: People
        public ActionResult Index(string searchName)
        {
            var people = from m in _db.People
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
            var people = _db.People.Where(x => x.Name.StartsWith(term)).Take(10).Select(x => new { label = x.Name });

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
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Person,PhotoUpload")] EditPersonViewModel model)
        {
            ValidatePhoto(model.PhotoUpload);
            if (ModelState.IsValid)
            {
                model.EditPerson.Photo = Files.UploadFile(Server, _photoPath, model.PhotoUpload);
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
            return View(new EditPersonViewModel() { EditPerson = person });
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
                model.EditPerson.Photo = Files.UploadFile(Server, _photoPath, model.PhotoUpload);
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
    }
}