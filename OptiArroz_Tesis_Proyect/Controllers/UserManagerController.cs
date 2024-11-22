using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.ViewModels;

namespace OptiArroz_Tesis_Proyect.Controllers
{
	public class UserManagerController : Controller
	{

        private readonly UserManager<ApplicationUser> UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;

        public UserManagerController(RoleManager<IdentityRole> RoleManager, UserManager<ApplicationUser> UserManager)
        {
            this.RoleManager = RoleManager;
            this.UserManager = UserManager;
        }

        public async Task<IActionResult> Index()
		{
            var Model = new UserManagerVM();
            Model.Roles = await RoleManager.Roles.Select(r => new RoleDTO { Id = r.Id, Name = r.Name }).ToListAsync();
            Model.ActiveList = await UserManager.Users.Where(x => x.State == 1).Select(r => new UserTableDTO
            {

            }).ToListAsync();
            return View();
		}
	}
}
