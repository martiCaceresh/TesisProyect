using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OptiArroz_Tesis_Proyect.Data.DataAccess;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.ViewModels;

namespace OptiArroz_Tesis_Proyect.Controllers
{
    public class MobileController : Controller
    {
        private readonly IRiceClassificationDA RiceClassificationDA;
        private readonly IRiceLotMovementDA RiceLotMovementDA;
        private readonly IRiceSacksOutputDA RiceSacksOutputDA;
        private readonly IRiceSacksDevolutionDA RiceSacksDevolutionDA;
        private readonly IRiceSacksOutputTypeDA RiceSacksOutputTypeDA;
        private readonly IRiceLotDA RiceLotDA;
        private readonly IZoneDA ZoneDA;
        private readonly IMapper Mapper;
        public readonly UserManager<ApplicationUser> UserManager;
        private readonly IConfiguration Configuration;

        public MobileController(IRiceSacksOutputTypeDA RiceSacksOutputTypeDA,IRiceSacksDevolutionDA RiceSacksDevolutionDA,IRiceSacksOutputDA RiceSacksOutputDA,IConfiguration Configuration, IRiceLotDA RiceLotDA, IRiceLotMovementDA RiceLotMovementDA, IZoneDA ZoneDA, IRiceClassificationDA RiceClassificationDA, IMapper Mapper, UserManager<ApplicationUser> UserManager)
        {
            this.UserManager = UserManager;
            this.Mapper = Mapper;
            this.RiceClassificationDA = RiceClassificationDA;
            this.RiceLotMovementDA = RiceLotMovementDA;
            this.RiceLotDA = RiceLotDA;
            this.ZoneDA = ZoneDA;
            this.Configuration = Configuration;
            this.RiceSacksOutputDA = RiceSacksOutputDA;
            this.RiceSacksDevolutionDA = RiceSacksDevolutionDA;
            this.RiceSacksOutputTypeDA = RiceSacksOutputTypeDA;
        }

        public async Task<IActionResult> Lot(int IdLot)
        {
            var userAgent = Request.Headers["User-Agent"].ToString();

            if (EsDispositivoMovil(userAgent))
            {
                var Model = new RiceLotDetailVM();
                Model.Classifications = await RiceClassificationDA.GetActiveRiceClassifications();
                var RiceLot = await RiceLotDA.GetRiceLotById(IdLot);
                Model.RiceLotDetailDTO = Mapper.Map<RiceLotDetailDTO>(RiceLot);

                ViewBag.IdLot = IdLot;

                return View(Model);
            }
            else
            {
                return Unauthorized("Acceso restringido a dispositivos móviles.");
            }
        }

        public async Task<IActionResult> RiceLotMovement(int IdLot)
        {
            var userAgent = Request.Headers["User-Agent"].ToString();

            if (EsDispositivoMovil(userAgent))
            {
                var Model = new RiceLotMovementVM();
                var RiceLot = await RiceLotDA.GetRiceLotById(IdLot);
                Model.RiceLotDetailDTO = Mapper.Map<RiceLotDetailDTO>(RiceLot);
                Model.Zones = await ZoneDA.GetActiveZones();
                var RiceMovement = await RiceLotMovementDA.GetRiceLotMovementByLot(IdLot);
                Model.RiceLotMovementTableDTO = Mapper.Map<List<RiceLotMovementTableDTO>>(RiceMovement);

                ViewBag.IdLot = IdLot;

                return View(Model);
            }
            else
            {
                return Unauthorized("Acceso restringido a dispositivos móviles.");
            }
        }

        public async Task<IActionResult> RiceLotOutput(int IdLot)
        {
            var userAgent = Request.Headers["User-Agent"].ToString();

            if (EsDispositivoMovil(userAgent))
            {
                var Model = new RiceLotOutputVM();
                var RiceLot = await RiceLotDA.GetRiceLotById(IdLot);
                Model.RiceLotDetailDTO = Mapper.Map<RiceLotDetailDTO>(RiceLot);
                Model.RiceSacksOutputTypes = await RiceSacksOutputTypeDA.GetRiceSacksOutputTypes();

                ViewBag.IdLot = IdLot;

                return View(Model);
            }
            else
            {
                return Unauthorized("Acceso restringido a dispositivos móviles.");
            }
        }


        public async Task<IActionResult> RiceSackOutput(int IdOutput)
        {
            var userAgent = Request.Headers["User-Agent"].ToString();

            if (EsDispositivoMovil(userAgent))
            {
                var Output = (await RiceSacksOutputDA.GetRiceSacksOutputs()).Where(x => x.IdRiceSacksOutput == IdOutput).FirstOrDefault();
                return View(Output);
            }
            else
            {
                return Unauthorized("Acceso restringido a dispositivos móviles.");
            }

            


        }

        public async Task<IActionResult> RiceSackDevolution(int IdDevolution)
        {
            var userAgent = Request.Headers["User-Agent"].ToString();

            if (EsDispositivoMovil(userAgent))
            {
                var Devolution = (await RiceSacksDevolutionDA.GetRiceSacksDevolutions()).Where(x => x.IdRiceSacksDevolution == IdDevolution).FirstOrDefault();
                return View(Devolution);
            }
            else
            {
                return Unauthorized("Acceso restringido a dispositivos móviles.");
            }
            
        }

        private bool EsDispositivoMovil(string userAgent)
        {
            // Lista simple de palabras clave que indican un dispositivo móvil
            var dispositivosMoviles = new[] { "android", "iphone", "ipod", "windows phone", "blackberry", "mobile", "tablet" };

            return dispositivosMoviles.Any(agent => userAgent.ToLower().Contains(agent));
        }
    }
}
