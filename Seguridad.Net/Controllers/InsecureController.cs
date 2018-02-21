using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
        // http://localhost:62921/Secure/TestSQLInjection?Nombre='or '1' = '1
        public ActionResult TestSQLInjection(string Nombre)
        {
            var m_dbConnection = new SQLiteConnection("Data Source=BaseDatosSegura.sqlite;Version=3;");
            m_dbConnection.Open();
            var tb = new DataTable();
            string sql = "select * from usuarios where nombre = '" + Nombre + "'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            using (SQLiteDataReader dr = command.ExecuteReader())
            {

                tb.Load(dr);

            }

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