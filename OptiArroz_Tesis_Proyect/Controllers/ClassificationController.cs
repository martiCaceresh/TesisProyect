using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OptiArroz_Tesis_Proyect.Data.DataAccess;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.ViewModels;
using System.Text.Json;

namespace OptiArroz_Tesis_Proyect.Controllers
{
    public class ClassificationController : Controller
    {
        private readonly IRiceClassificationDA RiceClassificationDA;
        private readonly IRiceClassDA RiceClassDA;
        private readonly IRiceGradeDA RiceGradeDA;
        private readonly ISackDA RiceSackDA;
        public readonly UserManager<ApplicationUser> UserManager;



        public ClassificationController(IRiceClassDA RiceClassDA, IRiceGradeDA RiceGradeDA, ISackDA RiceSackDA, IRiceClassificationDA RiceClassificationDA ,UserManager<ApplicationUser> UserManager)
        {

            this.UserManager = UserManager;

            this.RiceClassificationDA = RiceClassificationDA;
            this.RiceClassDA = RiceClassDA;
            this.RiceGradeDA = RiceGradeDA;
            this.RiceSackDA = RiceSackDA;

        }
        public async Task<IActionResult> Index()
        {
            var Model = new ClassificationManagerVM();
            Model.InactiveList = await RiceClassificationDA.GetInactiveRiceClassifications();
            Model.ActiveList = await RiceClassificationDA.GetActiveRiceClassifications();
            Model.RiceClasses = (await RiceClassDA.GetActiveRiceClassesAsync()).ToList();
            Model.RiceGrades = (await RiceGradeDA.GetActiveRiceGradesAsync()).ToList();
            Model.RiceSacks = await RiceSackDA.GetRiceSacks(1);
            return View(Model);
        }

        public async Task<IActionResult> Create(int MinimunStock, int MaximunStock, int SacksPerLot, int IdRiceClass, int IdRiceGrade, int IdRiceSack)
        {
            try
            {
                var CurrentUser = await UserManager.GetUserAsync(User);
                await RiceClassificationDA.Create(new RiceClassification(MinimunStock,MaximunStock,SacksPerLot,IdRiceClass,IdRiceGrade,IdRiceSack, CurrentUser.Id));

                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se creo la clasificacion correctamente";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Agrega un mensaje de error al TempData
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Update(int IdRiceClassification, int MinimunStock, int MaximunStock, int SacksPerLot)
        {
            try
            {
                var CurrentUser = await UserManager.GetUserAsync(User);
                await RiceClassificationDA.Update(IdRiceClassification, MinimunStock, MaximunStock, SacksPerLot,CurrentUser.Id);

                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se creo el saco correctamente";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Agrega un mensaje de error al TempData
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index");
            }
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
