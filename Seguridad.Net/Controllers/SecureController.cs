

using Microsoft.Security.Application;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;
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

        public ContentResult Upload(HttpPostedFileBase file)
        {
            //Primero chequeamos el tipo mime correcto
            byte[] document = new byte[file.ContentLength];
            file.InputStream.Read(document, 0, file.ContentLength);
            System.UInt32 mimetype;
            FindMimeFromData(0, null, document, 256, null, 0, out mimetype, 0);
            System.IntPtr mimeTypePtr = new IntPtr(mimetype);
            string mime = Marshal.PtrToStringUni(mimeTypePtr);
            Marshal.FreeCoTaskMem(mimeTypePtr);


            string datos = "";
            Dictionary<string, string> dt = new Dictionary<string, string>();

            if(mime == "image/x-png" || mime == "image/pjpeg")
            {
                if (file != null && file.ContentLength > 0 && file.ContentLength < (4 * 1024) * 1024) //tambien chequeamos el peso
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
            }
            else
            {
                dt.Add("Mensaje", "Archivo Invalido");
            }
            
            datos = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return Content(datos, "application/json");
        }


        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static System.UInt32 FindMimeFromData(System.UInt32 pBC,
        [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
        [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
        System.UInt32 cbSize, [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
        System.UInt32 dwMimeFlags,
        out System.UInt32 ppwzMimeOut,
        System.UInt32 dwReserverd);

    }
}