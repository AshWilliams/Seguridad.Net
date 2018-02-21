using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Seguridad.Net
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            SQLiteConnection.CreateFile("BaseDatosSegura.sqlite");

            var m_dbConnection = new SQLiteConnection("Data Source=BaseDatosSegura.sqlite;Version=3;");
            m_dbConnection.Open();

            string sql = "CREATE TABLE usuarios (nombre VARCHAR(100), password VARCHAR(200))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "insert into usuarios (nombre, password) values ('janito', 'janito123')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "insert into usuarios (nombre, password) values ('edu2018', 'edu2018')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "insert into usuarios (nombre, password) values ('robert', 'jajaja123')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            m_dbConnection.Close();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
