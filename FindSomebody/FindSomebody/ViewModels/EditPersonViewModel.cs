using FindSomebody.Models;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace FindSomebody.ViewModels
{
    /// <summary>
    /// Properties displayed and edited in the Create/Edit view.
    /// </summary>
    public class EditPersonViewModel
    {
        /// <summary>
        /// Person to create or edit.
        /// </summary>
        public Person EditPerson { get; set; }

        /// <summary>
        /// File upload.
        /// </summary>
        [Display(Name = "Upload Photo")]
        public HttpPostedFileBase PhotoUpload { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EditPersonViewModel()
        {
            EditPerson = new Person();
        }
    }
}