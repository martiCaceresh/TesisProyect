using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OptiArroz_Tesis_Proyect.Data.DataAccess;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.ViewModels;

namespace OptiArroz_Tesis_Proyect.Controllers
{
    public class AlertManagerController : Controller
    {
        public readonly UserManager<ApplicationUser> UserManager;
        public readonly INotificationTypeDA NotificationTypeDA;


        public AlertManagerController(INotificationTypeDA NotificationTypeDA, IZoneDA ZoneDA, UserManager<ApplicationUser> UserManager)
        {

            this.NotificationTypeDA = NotificationTypeDA;
            this.UserManager = UserManager;

        }
        public async Task<IActionResult> Index()
        {
            var Model = new AlertManagerVM();
            Model.Stock = await NotificationTypeDA.GetNotificationTypes("STOCKS");
            Model.Expiration = await NotificationTypeDA.GetNotificationTypes("FECHA VENCIMIENTO");



            return View(Model);
        }

        public async Task<IActionResult> Update(int PriorNotificationDays, int FrecuencyAfterFirstNotification, int PriorNotificationSacks)
        {
            try
            {
                await NotificationTypeDA.Update( PriorNotificationDays,  FrecuencyAfterFirstNotification,  PriorNotificationSacks);

                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se edito el saco correctamente";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Agrega un mensaje de error al TempData
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index");
            }
        }
    }
}
