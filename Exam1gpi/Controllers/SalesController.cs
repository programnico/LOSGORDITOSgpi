using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using Exam1gpi.Models;

namespace Exam1gpi.Controllers
{
    public class SalesController : Controller
    {
        test1gpiEntities context = new test1gpiEntities();
        // GET: Sales
        public ActionResult Sales()
        {
            //validamos que este logueado
            if (System.Web.HttpContext.Current.Session["nivel"] == null)
            {
                return RedirectToAction("../Login/Login");
            }
            else
            {
                //verificamos el mensaje del ActionResult de crud
                if (TempData["msj"] != null)
                {
                    ViewBag.msj = TempData["msj"];
                }

                //hacemo lo que seria Select * from
                List<sales> data = context.sales.ToList();
                ViewBag.data = data;

                //para cargar dropdownlist
                ViewBag.combo = context.orders.Select(x => new SelectListItem
                {
                    Text = x.codOrder.ToString(),
                    Value = x.codOrder.ToString()
                }).Distinct();

                return View();
            }
        }

        [HttpPost]
        public ActionResult SalesCrud(sales s)
        {
            string accion = Request.Form["boton"].ToString();
            string eliminacion = Request.Form["eliminacion"].ToString();
            string modificacion = Request.Form["modificacion"].ToString();

            switch (accion)
            {
                case "Guardar":
                    s.dateSale = DateTime.Now;

                        context.sales.Add(s);//cargamos el obj
                        context.SaveChanges();//guardamos
                        TempData["msj"] = "Guardado";
                    break;

                case "Modificar":
                    if (modificacion.Equals("si"))
                    {
                        sales temp = context.sales.FirstOrDefault(x => x.codSale == s.codSale);
                        temp.codSale = s.codSale;
                        temp.dateSale = s.dateSale;
                        temp.codOrder = s.codOrder;
                        temp.price = s.price;
                    
                        context.SaveChanges();
                        TempData["msj"] = "Modificado";
                    }

                    break;

                case "Eliminar":
                    if (eliminacion.Equals("si"))
                    {
                        context.sales.Remove(context.sales.FirstOrDefault(x => x.codSale == s.codSale));
                        context.SaveChanges();
                        TempData["msj"] = "Eliminado";
                    }
                    break;
            }

            return RedirectToAction("../Sales/Sales");
        }

        public ActionResult ReporteSales(sales s)
        {
            if (System.Web.HttpContext.Current.Session["nivel"] == null)
            {
                return RedirectToAction("../Login/Login");
            }
            else
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "Sales.rpt"));

                /*if (s.codOrder == 0)
                {
                    rd.SetDataSource(context.sales.Select(x => new
                    {
                        codSale = s.codSale,
                        dateSale = s.dateSale,
                        codOrder = s.codOrder,
                        price = s.price
                    }).Where(x => x.codOrder == s.codOrder).ToList());
                }
                else
                {
                    rd.SetDataSource(context.sales.Select(x => new
                    {
                        codSale = s.codSale,
                        dateSale = s.dateSale,
                        codOrder = s.codOrder,
                        price = s.price
                    }).ToList());
                }*/

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