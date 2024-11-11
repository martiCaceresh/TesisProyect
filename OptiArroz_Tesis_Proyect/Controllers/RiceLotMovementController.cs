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
        private readonly IZoneDA ZoneDA;
        private readonly IRiceLotDA RiceLotDA;
        private readonly IRiceLotMovementDA RiceLotMovementDA;
        private readonly IMapper Mapper;
        public readonly UserManager<ApplicationUser> UserManager;



        public RiceLotMovementController(IRiceLotMovementDA RiceLotMovementDA, IRiceLotDA RiceLotDA, IZoneDA ZoneDA, IMapper Mapper, UserManager<ApplicationUser> UserManager)
        {

            this.UserManager = UserManager;
            this.Mapper = Mapper;
            this.ZoneDA = ZoneDA;
            this.RiceLotDA = RiceLotDA;
            this.RiceLotMovementDA = RiceLotMovementDA;
        }
        public async Task<IActionResult> Index(int IdLot)
        {
            var Model = new RiceLotMovementVM();
            var RiceLot = await RiceLotDA.GetRiceLotById(IdLot);
            Model.RiceLotDetailDTO = Mapper.Map<RiceLotDetailDTO>(RiceLot);
            Model.Zones = await ZoneDA.GetActiveZones();
            var RiceMovement = await RiceLotMovementDA.GetRiceLotMovementByLot(IdLot);
            Model.RiceLotMovementTableDTO = Mapper.Map<List<RiceLotMovementTableDTO>> (RiceMovement);
            return View(Model);
        }

        public async Task<IActionResult> CreateRiceLotMovement([FromForm] CreateRiceLotMovementDTO NewLotMovementDTO)
        {
            try
            {
                var CurrentUser = await UserManager.GetUserAsync(User);
                var NewLotMovement = new RiceLotMovement(NewLotMovementDTO,CurrentUser.Id);

                await RiceLotMovementDA.CreatRiceLotMovement(NewLotMovement);
                
                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se registro el movimiento correctamente";

                return RedirectToAction("RiceLotDetails", "RiceLot" , new {IdLot = NewLotMovementDTO.IdRiceLot});
            }
            catch (Exception)
            {
                // Agrega un mensaje de error al TempData
                TempData["ErrorMessage"] = "Se produjo un error, vuelva a intentar.";

                return RedirectToAction("Index", new { IdLot = NewLotMovementDTO.IdRiceLot });
            }
        }
    }
}
