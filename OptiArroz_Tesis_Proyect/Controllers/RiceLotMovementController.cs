using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.ViewModels;

namespace OptiArroz_Tesis_Proyect.Controllers
{
    [Authorize]
    public class RiceLotMovementController : Controller
    {
        private readonly IRiceClassificationDA RiceClassificationDA;
        private readonly IZoneDA ZoneDA;
        private readonly IRiceLotDA RiceLotDA;
        private readonly IMapper Mapper;
        private readonly IConfiguration Configuration;
        public readonly UserManager<ApplicationUser> UserManager;



        public RiceLotMovementController(IConfiguration Configuration, IRiceLotDA RiceLotDA, IZoneDA ZoneDA, IRiceClassificationDA RiceClassificationDA, IMapper Mapper, UserManager<ApplicationUser> UserManager)
        {

            this.UserManager = UserManager;
            this.Mapper = Mapper;
            this.RiceClassificationDA = RiceClassificationDA;
            this.ZoneDA = ZoneDA;
            this.RiceLotDA = RiceLotDA;
            this.Configuration = Configuration;
        }
        public async Task<IActionResult> Index(int IdLot)
        {
            var Model = new RiceLotMovementVM();

            var RiceLot = await RiceLotDA.GetRiceLotById(IdLot);
            Model.RiceLotDetailDTO = Mapper.Map<RiceLotDetailDTO>(RiceLot);
            Model.Zones = await ZoneDA.GetActiveZones();
            



            return View(Model);
        }
    }
}
