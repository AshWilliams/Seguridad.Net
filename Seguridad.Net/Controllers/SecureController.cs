

using Microsoft.Security.Application;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Web.Mvc;

namespace Seguridad.Net.Controllers
{

    public class SecureController : Controller
    {
        private static string liteCon = ConfigurationManager.ConnectionStrings["strCon"].ConnectionString;
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


        // http://localhost:62921/Secure/TestSQLInjection?Nombre='or '1' = '1
        public ActionResult TestSQLInjection(string Nombre)
        {
            var m_dbConnection = new SQLiteConnection(liteCon);
            m_dbConnection.Open();
            var tb = new DataTable();
            string sql = "select * from usuarios where nombre = @nombre" ;
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.Parameters.Add(new SQLiteParameter("nombre", Nombre));
            SQLiteDataAdapter da = new SQLiteDataAdapter(command);

            da.Fill(tb);

            m_dbConnection.Close();

            ViewBag.Datos = JsonConvert.SerializeObject(tb, Formatting.Indented);
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