using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OptiArroz_Tesis_Proyect.Data.DataAccess;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.ViewModels;
using QRCoder;

namespace OptiArroz_Tesis_Proyect.Controllers
{
    public class RiceSacksDevolutionController : Controller
    {
        private readonly IRiceClassificationDA RiceClassificationDA;
        private readonly IZoneDA ZoneDA;
        private readonly IRiceLotDA RiceLotDA;
        private readonly IMapper Mapper;
        private readonly IConfiguration Configuration;
        private readonly IRiceSacksOutputDA RiceSacksOutputDA;
        private readonly IRiceSacksDevolutionDA RiceSacksDevolutionDA;
        private readonly IRiceSacksOutputTypeDA RiceSacksOutputTypeDA;
        private readonly IRiceSacksDevolutionTypeDA RiceSacksDevolutionTypeDA;
        public readonly UserManager<ApplicationUser> UserManager;




        public RiceSacksDevolutionController(IRiceSacksDevolutionTypeDA RiceSacksDevolutionTypeDA, IRiceSacksDevolutionDA RiceSacksDevolutionDA,IRiceSacksOutputTypeDA RiceSacksOutputTypeDA, IRiceSacksOutputDA RiceSacksOutputDA, IConfiguration Configuration, IRiceLotDA RiceLotDA, IZoneDA ZoneDA, IRiceClassificationDA RiceClassificationDA, IMapper Mapper, UserManager<ApplicationUser> UserManager)
        {

            this.UserManager = UserManager;
            this.Mapper = Mapper;
            this.RiceClassificationDA = RiceClassificationDA;
            this.ZoneDA = ZoneDA;
            this.RiceLotDA = RiceLotDA;
            this.Configuration = Configuration;
            this.RiceSacksOutputDA = RiceSacksOutputDA;
            this.RiceSacksOutputTypeDA = RiceSacksOutputTypeDA;
            this.RiceSacksDevolutionDA = RiceSacksDevolutionDA;
            this.RiceSacksDevolutionTypeDA = RiceSacksDevolutionTypeDA;
        }
        public async Task<IActionResult> Index()
        {
            var Model = new RiceSacksDevolutionVM();
            var Devolutions = await RiceSacksDevolutionDA.GetRiceSacksDevolutions();
            Model.RiceSacksDevolutionTypes = await RiceSacksDevolutionTypeDA.GetRiceSacksDevolutionTypes();
            Model.RiceSacksDevolutionTableDTOs = Devolutions;
            return View(Model);
        }

        public async Task<JsonResult> GetRiceSacksOutputDetails(string OutputCode)
        {
            var GroupDetail = await RiceSacksOutputDA.GetRiceSackOutputTypeLot(OutputCode);

            return Json(GroupDetail);
        }

        public async Task<IActionResult> CreateRiceSacksDevolution([FromForm] CreateNewDevolutionDTO NewDevolutionDTO)
        {
            try
            {
                var CurrentUser = await UserManager.GetUserAsync(User);

                var NewDevolution = new RiceSacksDevolution(NewDevolutionDTO , CurrentUser.Id);

                var response = await RiceSacksDevolutionDA.CreateRiceSacksDevolution(NewDevolution, NewDevolutionDTO);
                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Se registro la devolución correctamente";

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Agrega un mensaje de error al TempData
                TempData["ErrorMessage"] = "Se produjo un error en el registro, vuelva a intentar.";

                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public IActionResult GenerateMobileQR(int idDevolution)
        {
            try
            {
                // Generar URL segura (podrías incluir un token temporal)
                var baseUrl = Configuration["BaseUrl"];
                var mobileUrl = $"{baseUrl}/mobile/devolution/{idDevolution}";

                // Generar QR
                using (var qrGenerator = new QRCodeGenerator())
                {
                    var qrData = qrGenerator.CreateQrCode(mobileUrl, QRCodeGenerator.ECCLevel.Q);
                    var qrCode = new PngByteQRCode(qrData);
                    var qrImage = qrCode.GetGraphic(20); // Pixeles por módulo

                    return new JsonResult(new
                    {
                        success = true,
                        qrImage = Convert.ToBase64String(qrImage),
                        url = mobileUrl
                    });
                }
            }
            catch
            {
                return StatusCode(500, new { success = false, message = "Error generating QR code" });
            }
        }
    }
}
