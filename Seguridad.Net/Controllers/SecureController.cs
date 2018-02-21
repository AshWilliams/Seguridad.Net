

using Microsoft.Security.Application;
using System.Web.Mvc;

namespace Seguridad.Net.Controllers
{

    public class SecureController : Controller
    {
        // GET: Secure
        // http://localhost:62921/Secure/TestXSS?Nombre=Robert Rozas <script>alert("XSS")</script>  Error
        // http://localhost:62921/Secure/TestXSS?Nombre=Robert Rozas \x3cscript\x3ealert(\x27doingSomethingNaughty\x27)\x3c/script\x3e  Bypass
        public ActionResult TestXSS(string Nombre)
        {
            ViewBag.Nombre = Encoder.JavaScriptEncode(Nombre);
            ViewBag.Clean = Sanitizer.GetSafeHtmlFragment(Nombre.Replace("\\x3c", "<").Replace("\\x3e",">").Replace("\\x27","\""));
            
            return View();
        }

        public ActionResult TestCSRF()
        {
            return View();
        }

        public ActionResult TestSQLInjection()
        {
            return View();
        }

        public ActionResult FileUpload()
        {
            return View();
        }

        public ContentResult Upload()
        {
            string datos = "";//JsonConvert.SerializeObject(dt, Formatting.Indented);
            return Content(datos, "application/json");
        }

    }
}