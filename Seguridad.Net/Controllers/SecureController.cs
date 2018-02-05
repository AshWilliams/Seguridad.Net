

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
            ViewBag.Insecure = Nombre;
            
            return View();
        }
    }
}