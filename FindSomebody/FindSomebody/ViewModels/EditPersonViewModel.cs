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
        public Person EditPerson { get; set; }

        [Display(Name = "Upload Photo")]
        public HttpPostedFileBase PhotoUpload { get; set; }

        public EditPersonViewModel()
        {
            EditPerson = new Person();
        }
    }
}