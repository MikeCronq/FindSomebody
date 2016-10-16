using System.IO;
using System.Web;

namespace WebTools
{
    public static class Files
    {
        /// <summary>
        /// Uploads a file into server file structure.
        /// </summary>
        /// <param name="folderPath">Storage folder path.</param>
        /// <param name="fileUpload">File upload.</param>
        /// <returns>Url to the uploaded file.</returns>
        public static string UploadFile(HttpServerUtilityBase server, string folderPath, HttpPostedFileBase fileUpload)
        {
            if (!folderPath.EndsWith("/"))
            {
                folderPath += '/';
            }

            string fileUrl = null;
            if (fileUpload != null)
            {
                if (fileUpload.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(server.MapPath(folderPath), fileName);
                    fileUpload.SaveAs(path);
                    fileUrl = folderPath + fileName;
                }
            }
            return fileUrl;
        }
    }
}