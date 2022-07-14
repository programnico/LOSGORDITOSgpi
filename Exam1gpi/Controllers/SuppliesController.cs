using Exam1gpi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam1gpi.Controllers
{
    public class SuppliesController : Controller
    {
        //context first
        test1gpiEntities context = new test1gpiEntities();
        public ActionResult Supplies()
        {
            //validamos que este logueado
            if (System.Web.HttpContext.Current.Session["nivel"] == null)
            {
                return RedirectToAction("../Login/Login");
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["nivel"].ToString() == "SECRETARIA")
                {
                    return RedirectToAction("../Home/Index");
                }
                else
                {
                    List<supply> data = context.supply.ToList();
                    ViewBag.dataS = data;

                    if (TempData["msj"] != null)
                    {
                        ViewBag.msj = TempData["msj"];
                    }

                    return View();
                }
            }
        }

        [HttpPost]
        public ActionResult Supplies(supply s)
        {
            if (ModelState.IsValid)
            {
                string accion = Request.Form["boton"].ToString();

                switch (accion)
                {
                    case "Guardar":
                        try
                        {
                            context.supply.Add(s);
                            context.SaveChanges();
                            TempData["msj"] = "Guardado";

                        }
                        catch (Exception e)
                        {
                            TempData["msj"] = "VerificarDatos";
                        }
                        
                        break;
                    case "Eliminar":
                        context.supply.Remove(context.supply.FirstOrDefault(x => x.codSupply == s.codSupply));
                        context.SaveChanges();
                        TempData["msj"] = "Eliminado";

                        break;
                    case "Modificar":

                        supply temp = context.supply.FirstOrDefault(x => x.codSupply == s.codSupply);
                        temp.nameSupply = s.nameSupply;
                        temp.enable = s.enable;
                        context.SaveChanges();
                        TempData["msj"] = "Modificado";

                        break;
                }
            }
            return RedirectToAction("Supplies");

        }
    }
}