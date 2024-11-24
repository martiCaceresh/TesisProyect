using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OptiArroz_Tesis_Proyect.Data.DataAccess;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.ViewModels;

namespace OptiArroz_Tesis_Proyect.Controllers
{
    public class SackManagerController : Controller
    {

        private readonly ISackDA SackDA;
        private readonly UserManager<ApplicationUser> UserManager;

        public SackManagerController(UserManager<ApplicationUser> UserManager, ISackDA SackDA)
        {
            this.UserManager = UserManager;
            this.SackDA = SackDA;
        }
        public async Task<IActionResult> Index()
        {
            var Model = new SackManagerVM();
            Model.ActiveList = await SackDA.GetRiceSacks(1);
            Model.InactiveList = await SackDA.GetRiceSacks(0);
            return View(Model);
        }

        public async Task<IActionResult> Create(decimal Weigth)
        {
            try
            {
                var CurrentUser = await UserManager.GetUserAsync(User);
                await SackDA.Create( new RiceSack(Weigth,CurrentUser.Id));

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

        public async Task<IActionResult> Activate(int Id)
        {
            try
            {
                await SackDA.Activate(Id);

                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se activo el saco correctamente";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Agrega un mensaje de error al TempData
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Deactivate(int Id)
        {
            try
            {
                await SackDA.Deactivate(Id);

                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se desactivo el saco correctamente";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Agrega un mensaje de error al TempData
                TempData["ErrorMessage"] =ex.Message;

                return RedirectToAction("Index");
            }
        }
    }
}
