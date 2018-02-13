using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Seguridad.Net.Controllers
{
    public class InsecureController : Controller
    {
        // GET: Insecure
        //Ejemplo: http://localhost:62921/Insecure/TestXSS?Nombre=Robert%20Rozas%20\x3cscript\x3ealert(\x27doingSomethingNaughty\x27)\x3c/script\x3e
   
        public ActionResult TestXSS(string Nombre)
        {
            ViewBag.Nombre = Nombre;
            return View();
        }
        public ActionResult TestCSRF()
        {
            return View();
        }
        public ActionResult TestSQLInjection(FormCollection theForm)
        {
            return View();
        }
    }
}