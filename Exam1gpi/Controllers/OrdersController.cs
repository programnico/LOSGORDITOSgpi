using Exam1gpi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam1gpi.Controllers
{
    public class OrdersController : Controller
    {
        test1gpiEntities context = new test1gpiEntities();

        public ActionResult Orders()
        {
            //validamos que este logueado
            if (System.Web.HttpContext.Current.Session["nivel"] == null)
            {
                return RedirectToAction("../Login/Login");
            }
            else
            {
                List<orders> data = context.orders.ToList();
                ViewBag.dataO = data;

                IList<SelectListItem> orderStatus = new List<SelectListItem>{
                new SelectListItem{Text = "CREADO", Value = "1"},
                new SelectListItem{Text = "EN PROCESO", Value = "2"},
                new SelectListItem{Text = "FINALIZADO", Value = "3"},
                new SelectListItem{Text = "ENTREGADO", Value = "4"},
                };

                ViewBag.orderStatus = orderStatus;

                if (TempData["msj"] != null)
                {
                    ViewBag.msj = TempData["msj"];
                }

                return View();
            }
        }

        [HttpPost]
        public ActionResult Orders(orders o)
        {
            string accion = Request.Form["boton"].ToString();

            if (accion.Equals("Modificar"))
            {
                orders temp = context.orders.FirstOrDefault(x => x.codOrder == o.codOrder);
                temp.status = o.status;
                context.SaveChanges();
                TempData["msj"] = "Modificado";
            }

            return RedirectToAction("Orders");
        }
    }
}