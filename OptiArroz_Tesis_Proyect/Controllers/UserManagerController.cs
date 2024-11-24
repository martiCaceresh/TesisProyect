using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.DataAccess;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.ViewModels;

namespace OptiArroz_Tesis_Proyect.Controllers
{
	public class UserManagerController : Controller
	{

        private readonly IUserDA UserDA;
        private readonly RoleManager<IdentityRole> RoleManager;

        public UserManagerController(RoleManager<IdentityRole> RoleManager, IUserDA UserDA)
        {
            this.RoleManager = RoleManager;
            this.UserDA = UserDA;
        }

        public async Task<IActionResult> Index()
		{
            var Model = new UserManagerVM();
            Model.Roles = await RoleManager.Roles.Select(r => new RoleDTO { Id = r.Id, Name = r.Name }).Where(x=>x.Name != "SUPERUSUARIO").ToListAsync();
            Model.ActiveList = await UserDA.GetUsers(1);
            Model.InactiveList = await UserDA.GetUsers(0);
            return View(Model);
		}

        public async Task<IActionResult> ChangeRoleUser(string UserId , string RolId)
        {
            try
            {
                await UserDA.ChangeRoleUser(UserId, RolId);

                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se cambio el rol correctamente";

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Agrega un mensaje de error al TempData
                TempData["ErrorMessage"] = "Se produjo un error en el cambio, vuelva a intentar.";

                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Activate(string UserId, string RolId)
        {
            try
            {
                await UserDA.ActivateUser(UserId);

                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se cambio el rol correctamente";

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Agrega un mensaje de error al TempData
                TempData["ErrorMessage"] = "Se produjo un error en el cambio, vuelva a intentar.";

                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Deactivate(string UserId, string RolId)
        {
            try
            {
                await UserDA.DeactivateUser(UserId);

                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se cambio el rol correctamente";

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Agrega un mensaje de error al TempData
                TempData["ErrorMessage"] = "Se produjo un error en el cambio, vuelva a intentar.";

                return RedirectToAction("Index");
            }
        }
    }
}
