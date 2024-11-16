using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;
using System.Text.Json;

namespace OptiArroz_Tesis_Proyect.Controllers
{
    public class ClassificationController : Controller
    {
        private readonly IRiceClassificationDA RiceClassificationDA;
        public readonly UserManager<ApplicationUser> UserManager;



        public ClassificationController( IRiceClassificationDA RiceClassificationDA ,UserManager<ApplicationUser> UserManager)
        {

            this.UserManager = UserManager;

            this.RiceClassificationDA = RiceClassificationDA;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetStockConsultation([FromBody] List<RiceSacksConsultationDTO> StockConsultation)
        {
            try
            {
                

                if (StockConsultation == null || !StockConsultation.Any())
                {
                    return Json(new { success = false, message = "No se recibieron datos para procesar la consulta" });
                }

                var results = await RiceClassificationDA.GetStockConsultation(StockConsultation);

                return Json(new { success = true, data = results });
            }
            catch (Exception ex)
            {
                // Log del error si tienes un sistema de logging
                return Json(new { success = false, message = "Error al procesar la consulta: " + ex.Message });
            }
        }
    }
}
