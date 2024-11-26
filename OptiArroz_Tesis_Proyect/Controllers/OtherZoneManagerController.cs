using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.ViewModels;

namespace OptiArroz_Tesis_Proyect.Controllers
{
    public class OtherZoneManagerController : Controller
    {
        private readonly IZoneDA ZoneDA;
        public readonly UserManager<ApplicationUser> UserManager;
        public readonly IFirstConfigurationDA FirstConfigurationDA;


        public OtherZoneManagerController(IFirstConfigurationDA FirstConfigurationDA, IZoneDA ZoneDA, UserManager<ApplicationUser> UserManager)
        {

            this.ZoneDA = ZoneDA;
            this.FirstConfigurationDA = FirstConfigurationDA;
            this.UserManager = UserManager;

        }
        public async Task<IActionResult> Index()
        {
            var Model = new ZoneManagerVM();
            Model.ActiveList = (await ZoneDA.GetActiveZones()).Where(x => x.Columns == 0 || x.Rows == 0).ToList();
            Model.InactiveList = (await ZoneDA.GetInactiveZones()).Where(x => x.Columns == 0 || x.Rows == 0).ToList();
            Model.SystemConfiguration = await FirstConfigurationDA.GetFirstConfiguration();
            return View(Model);
        }

        public async Task<IActionResult> Create(string Name, string Description, decimal Length, decimal Width, decimal Height)
        {
            try
            {
                var CurrentUser = await UserManager.GetUserAsync(User);
                await ZoneDA.Create(new Zone(Name, Description, Length, Width, Height, 0, 0, CurrentUser.Id));

                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se creo el almacen correctamente";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Agrega un mensaje de error al TempData
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Update(int IdZone, string Name, string Description)
        {
            try
            {
                var CurrentUser = await UserManager.GetUserAsync(User);
                await ZoneDA.Update(new Zone(IdZone, Name, Description, CurrentUser.Id));

                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se edito el almacen correctamente";

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
                await ZoneDA.Activate(Id);

                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se activo el almacen correctamente";

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
                await ZoneDA.Deactivate(Id);

                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se desactivo el almacen correctamente";

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
