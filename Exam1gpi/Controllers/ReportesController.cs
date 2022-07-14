using Exam1gpi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;

namespace Exam1gpi.Controllers
{
    public class ReportesController : Controller
    {

        test1gpiEntities context = new test1gpiEntities();
        // GET: Reportes
        
        public ActionResult ReporteSupply()
        {
            if (System.Web.HttpContext.Current.Session["nivel"] == null)
            {
                return RedirectToAction("../Login/Login");
            }
            else
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "Supply.rpt"));

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                
                //En PDF
                Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                rd.Dispose();
                rd.Close();
                return new FileStreamResult(stream, "application/pdf");

            }
        }

        public ActionResult ReportOrders()
        {
            if (System.Web.HttpContext.Current.Session["nivel"] == null)
            {
                return RedirectToAction("../Login/Login");
            }
            else
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "Orders.rpt"));

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                //En PDF
                Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                rd.Dispose();
                rd.Close();

                return new FileStreamResult(stream, "application/pdf");

            }
        }

    }
}