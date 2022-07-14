using Exam1gpi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam1gpi.Controllers
{
    public class SupplyMovementController : Controller
    {
        test1gpiEntities context = new test1gpiEntities();
        public ActionResult SupplyMovement()
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
                    List<supplyMovement> data = context.supplyMovement.ToList();
                    ViewBag.dataSP = data;

                    ViewBag.supplies = context.supply.Select(x => new SelectListItem
                    {
                        Text = x.nameSupply,
                        Value = x.codSupply.ToString()
                    });

                    ViewBag.unitMeasurement = context.unitMeasurement.Select(x => new SelectListItem
                    {
                        Text = x.nameUnitMeasurement,
                        Value = x.nameUnitMeasurement.ToString()
                    });

                    if (TempData["msj"] != null)
                    {
                        ViewBag.msj = TempData["msj"];
                    }

                    return View();
                }
            }
        }

        [HttpPost]
        public ActionResult SupplyMovement(supplyMovement sm, string cmbUnidadMedida)
        {
            //validamos que este logueado
            if (System.Web.HttpContext.Current.Session["nivel"] == null)
            {
                return RedirectToAction("../Login/Login");
            }
            else
            {

                if (cmbUnidadMedida.Equals("QUINTAL"))
                {
                    decimal quantity = sm.quantity;
                    decimal newValue = quantity * 100; //100 libras = 1 QUINTAL  
                    sm.quantity = newValue;
                }

                sm.tipo = 1;

                if (ModelState.IsValid)
                {
                    string accion = Request.Form["boton"].ToString();

                    switch (accion)
                    {
                        case "Guardar":
                            context.supplyMovement.Add(sm);
                            context.SaveChanges();


                            //updata supplies
                            supply temp = context.supply.FirstOrDefault(x => x.codSupply == sm.codSupply);
                            temp.stock = temp.stock + sm.quantity;
                            context.SaveChanges();

                            TempData["msj"] = "Guardado";

                            break;
                    }
                }
                return RedirectToAction("SupplyMovement");
            }
        }
    }
}