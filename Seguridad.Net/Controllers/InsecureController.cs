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
        public ActionResult Index()
        {
            return View();
        }
    }
}