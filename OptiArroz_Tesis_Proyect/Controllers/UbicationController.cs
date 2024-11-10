using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Controllers
{

    public class UbicationController : Controller
    {

        private readonly IUbicationDA UbicationDA;
        public readonly UserManager<ApplicationUser> UserManager;



        public UbicationController(IUbicationDA UbicationDA, UserManager<ApplicationUser> UserManager)
        {

            this.UbicationDA = UbicationDA;

            this.UserManager = UserManager;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetUbicationsByZone(int IdZone)
        {
            var Result = await UbicationDA.GetUbicationsByZone(IdZone);
            return Json(Result);
        }
    }
}
