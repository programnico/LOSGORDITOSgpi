using Exam1gpi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam1gpi.Controllers
{
    public class ProductUnitMeasurementController : Controller
    {

        //context first
        test1gpiEntities context = new test1gpiEntities();
        public ActionResult ProductUnitMeasurement()
        {
            //validamos que este logueado
            if (System.Web.HttpContext.Current.Session["nivel"] == null)
            {
                return RedirectToAction("../Login/Login");
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["nivel"].ToString() != "ADMINISTRADOR")
                {
                    return RedirectToAction("../Home/Index");
                }
                else
                {
                    List<productUnitMeasurement> data = context.productUnitMeasurement.ToList();
                    ViewBag.dataPUM = data;

                    ViewBag.products = context.product.Select(x => new SelectListItem
                    {
                        Text = x.nameProduct,
                        Value = x.codProduct.ToString()
                    });

                    ViewBag.unitMeasurements = context.unitMeasurement.Select(x => new SelectListItem
                    {
                        Text = x.nameUnitMeasurement,
                        Value = x.codUnitMeasurement.ToString()
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
        public ActionResult ProductUnitMeasurement(productUnitMeasurement pum)
        {

            if (ModelState.IsValid)
            {
                string accion = Request.Form["boton"].ToString();

                switch (accion)
                {
                    case "Guardar":

                        productUnitMeasurement temp1 = context.productUnitMeasurement.FirstOrDefault(x => x.codProduct == pum.codProduct && x.codUnitMeasurement == pum.codUnitMeasurement);
                        if (temp1 != null)
                        {
                            TempData["msj"] = "Existe";
                            return RedirectToAction("ProductUnitMeasurement");
                        }
                        else
                        {
                            context.productUnitMeasurement.Add(pum);
                            context.SaveChanges();
                            TempData["msj"] = "Guardado";
                        }


                        break;
                    case "Eliminar":
                        try
                        {
                            context.productUnitMeasurement.Remove(context.productUnitMeasurement.FirstOrDefault(x => x.codProductUnitMeasurement == pum.codProductUnitMeasurement));
                            context.SaveChanges();
                            TempData["msj"] = "Eliminado";

                        }
                        catch (Exception e)
                        {
                            TempData["msj"] = "NoEliminado";
                            return RedirectToAction("ProductUnitMeasurement");
                        } 

                        
                        break;
                    case "Modificar":

                        productUnitMeasurement temp2 = context.productUnitMeasurement.FirstOrDefault(x => x.codProduct == pum.codProduct && x.codUnitMeasurement == pum.codUnitMeasurement);

                        if (temp2 != null)
                        {
                            TempData["msj"] = "Existe";
                            return RedirectToAction("ProductUnitMeasurement");
                        }
                        else
                        {
                            productUnitMeasurement temp = context.productUnitMeasurement.FirstOrDefault(x => x.codProductUnitMeasurement == pum.codProductUnitMeasurement);
                            temp.codProduct = pum.codProduct;
                            temp.codUnitMeasurement = pum.codUnitMeasurement;
                            context.SaveChanges();
                            TempData["msj"] = "Modificado";
                        }

                        break;
                }
            }
            return RedirectToAction("ProductUnitMeasurement");
        }
    }
}