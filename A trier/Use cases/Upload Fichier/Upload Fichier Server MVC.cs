//Utilisation de fichier uploadé par le navigateur (MVC)
using System.Web.Mvc;

namespace App
{
    public class HomeController : Controller
    {
     
        [HttpPost]
        [Route("UploadFile")]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile()
        {
            string filename = "";
            try
            {
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                    if (hpf.ContentLength == 0)
                        continue;

                    filename = hpf.FileName;

					//Le dossier uploads doit être créer
                    string savedFileName = Server.MapPath("~\\uploads\\" + filename);
                    hpf.SaveAs(savedFileName); // Save the file
					
					...
				}
            }
            catch
            {
                return PartialView("UploadFailed", filename);
            }
        }

     }   
}

