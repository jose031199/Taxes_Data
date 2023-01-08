using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxes_Data.Models;

namespace Taxes_Data.Controllers
{
    public class AdminController : Controller
    {
        private readonly DBTaxes_RegisterContext DBContext;

        public AdminController(DBTaxes_RegisterContext _context)
        {
            DBContext = _context;
        }
        public IActionResult AdminDashboard()
        {

            if (HttpContext.Session.GetString("username") != null)
            {
                return View();
            }

            return RedirectToAction("Admin", "Login");
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Admin", "Login");
        }


        public JsonResult Listar()
        {
            var result = DBContext.TaxesRegister.ToList().OrderBy(t => t.FechaRegistro);
            return Json(new { data = result });
        }

        public JsonResult GetTaxesId(int id)
        {
            TaxesRegister result;
            if (GetTotal() == true)
            {
                result = null;
            }
            else
            {
                result = DBContext.TaxesRegister.Where(t => t.Id == id).FirstOrDefault();

            }
            return Json(result);
        }

        public bool GetTotal()
        {
            bool result = false;
            if (DBContext.TaxesRegister.ToList().Count >= 1000)
            {
                result = true;
            }
            return result;
        }

        public IActionResult RemoveAll()
        {
            DBContext.TaxesRegister.RemoveRange(DBContext.TaxesRegister.ToList());
            DBContext.SaveChanges();
            return RedirectToAction("AdminDashboard", "Admin");
        }

        [HttpPost]
        public JsonResult Guardar(TaxesRegister taxes)
        {
            bool response = false;
            string reason = "";
            try
            {
                if (GetTotal() == false)
                {
                    if (taxes.Id != 0)
                    {
                        var taxesEdit = DBContext.TaxesRegister.Where(t => t.Id == taxes.Id).FirstOrDefault();
                        taxesEdit.Nombre = taxes.Nombre;
                        taxesEdit.Apellidos = taxes.Apellidos;
                        taxesEdit.NoTelefono = taxes.NoTelefono;
                        taxesEdit.Correo = taxes.Correo;
                        taxesEdit.FechaRegistro = taxes.FechaRegistro;
                        response = true;
                        DBContext.SaveChanges();
                    }
                    else
                    {
                        reason = SaveDate(taxes);
                        if (reason.Length==0)
                        {
                            DBContext.TaxesRegister.Add(taxes);
                            DBContext.SaveChanges();
                            response = true;
                        }
                        /*if (Taxes_Date_Exist(taxes.FechaRegistro) != true)
                        {
                            DBContext.TaxesRegister.Add(taxes);
                            DBContext.SaveChanges();
                            response = true;
                        }
                        else
                        {
                            response = false;
                            reason = "Ya hay una cita a las " + taxes.FechaRegistro.ToString();
                        }*/

                    }

                }
                else
                {
                    reason = "!Base de datos llena!, descargue la informacion para despues eliminarla";
                }

            }
            catch (Exception ex)
            {
                response = false;
                reason = "Error " + ex.Message.ToString();
            }

            return Json(new { resultado = response, answer = reason });
        }


        public string SaveDate(TaxesRegister taxes)
        {
            string reason = "";
            if (Taxes_Date_Exist(taxes.FechaRegistro) == true)
            {
                reason = "Ya hay una cita a las " + taxes.FechaRegistro.ToString();
                //DBContext.TaxesRegister.Add(taxes);
                //DBContext.SaveChanges();
                //response = true;
            }
            return reason;
        }

        public bool Taxes_Date_Exist(DateTime? fecha) //Function to know if a taxes_date already existed
        {
            bool exist = false;
            var findDates = DBContext.TaxesRegister.Where(t => t.FechaRegistro.Equals(fecha));

            if (findDates.Count() >= 1)
            {
                exist = true;
            }

            return exist;
        }

        public JsonResult Eliminar(int id)
        {
            bool respuesta = true;

            try
            {
                var deleteTaxes = DBContext.TaxesRegister.Where(t => t.Id == id).FirstOrDefault();
                DBContext.TaxesRegister.Remove(deleteTaxes);
                DBContext.SaveChanges();

            }
            catch (Exception ex)
            {
                respuesta = false;
            }

            return Json(new { resultado = respuesta });
        }
    }
}
