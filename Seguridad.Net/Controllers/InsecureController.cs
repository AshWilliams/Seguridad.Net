using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Seguridad.Net.Controllers
{
    public class InsecureController : Controller
    {
        /*
                        __________████████_____██████
                _________█░░░░░░░░██_██░░░░░░█
                ________█░░░░░░░░░░░█░░░░░░░░░█
                _______█░░░░░░░███░░░█░░░░░░░░░█
                _______█░░░░███░░░███░█░░░████░█
                ______█░░░██░░░░░░░░███░██░░░░██
                _____█░░░░░░░░░░░░░░░░░█░░░░░░░░███
                ____█░░░░░░░░░░░░░██████░░░░░████░░█
                ____█░░░░░░░░░█████░░░████░░██░░██░░█
                ___██░░░░░░░███░░░░░░░░░░█░░░░░░░░███
                __█░░░░░░░░░░░░░░█████████░░█████████
                _█░░░░░░░░░░█████_████___████_█████___█
                _█░░░░░░░░░░█______█_███__█_____███_█___█
                █░░░░░░░░░░░░█___████_████____██_██████
                ░░░░░░░░░░░░░█████████░░░████████░░░█
                ░░░░░░░░░░░░░░░░█░░░░░█░░░░░░░░░░░░█
                ░░░░░░░░░░░░░░░░░░░░██░░░░█░░░░░░██
                ░░░░░░░░░░░░░░░░░░██░░░░░░░███████
                ░░░░░░░░░░░░░░░░██░░░░░░░░░░█░░░░░█
                ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█
                ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█
                ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█
                ░░░░░░░░░░░█████████░░░░░░░░░░░░░░██
                ░░░░░░░░░░█▒▒▒▒▒▒▒▒███████████████▒▒█
                ░░░░░░░░░█▒▒███████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒█
                ░░░░░░░░░█▒▒▒▒▒▒▒▒▒█████████████████
                ░░░░░░░░░░████████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒█
                ░░░░░░░░░░░░░░░░░░██████████████████
                ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█
                ██░░░░░░░░░░░░░░░░░░░░░░░░░░░██
                ▓██░░░░░░░░░░░░░░░░░░░░░░░░██
                ▓▓▓███░░░░░░░░░░░░░░░░░░░░█
                ▓▓▓▓▓▓███░░░░░░░░░░░░░░░██
                ▓▓▓▓▓▓▓▓▓███████████████▓▓█
                ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██
                ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█
                ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓█  
             
        */


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
            try
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
            }
            catch (Exception)
            {
                //do nothing and kill me
                throw;
            }
            return View();   
            
        }

        public ActionResult FileUpload()
        {
            return View();
        }

        public ContentResult Upload(HttpPostedFileBase file)
        {
            string datos = "";
            Dictionary<string, string> dt = new Dictionary<string, string>();
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/UploadedFiles"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    dt.Add("Mensaje", "File uploaded successfully");
                }
                catch (Exception ex)
                {
                    dt.Add("Mensaje", "ERROR:" + ex.Message.ToString());
                }
            else
            {
                dt.Add("Mensaje", "You have not specified a file.");
            }
            datos = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return Content(datos, "application/json");
        }
    }
}