using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.ViewModels;
using System.Diagnostics;
using System.Globalization;
using Twilio.TwiML.Voice;

namespace OptiArroz_Tesis_Proyect.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       
        private readonly IRiceClassDA RiceCLassDA;
        private readonly IFirstConfigurationDA FirstConfigurationDA;
        private readonly IRiceGradeDA RiceGradeDA;
        private readonly IRiceClassificationDA RiceClassificationDA;
        private readonly IRiceLotDA RiceLotDA;
        private readonly IZoneDA ZoneDA;
        private readonly IMapper Mapper;
        public readonly UserManager<ApplicationUser> UserManager;



        public HomeController(IZoneDA ZoneDA,IRiceClassificationDA RiceClassificationDA, IRiceLotDA RiceLotDA, IMapper Mapper,IFirstConfigurationDA FirstConfigurationDA, IRiceClassDA RiceCLassDA, IRiceGradeDA RiceGradeDA, UserManager<ApplicationUser> UserManager)
        {
            this.FirstConfigurationDA = FirstConfigurationDA;
            this.RiceCLassDA = RiceCLassDA;
            this.RiceGradeDA = RiceGradeDA;
            this.UserManager = UserManager;
            this.Mapper = Mapper;
            this.RiceLotDA = RiceLotDA;
            this.RiceClassificationDA = RiceClassificationDA;
            this.ZoneDA = ZoneDA;
        }

        public async Task<IActionResult> Index()
        {
            RiceLotVM Model = new RiceLotVM();
            var RiceLots = await RiceLotDA.GetActiveRiceLot();
            Model.RiceLots = Mapper.Map<List<RiceLotTableDTO>>(RiceLots);
            return View(Model);
        }

        public IActionResult Entry()
        {


            return View();
        }

        public IActionResult Exit()
        {
            //erik caceres
            return View();

        }

        public async Task<IActionResult> FirstConfiguration()
        {
            FirstConfigurationVM Model = new FirstConfigurationVM();
            Model.RiceGrades = (await RiceGradeDA.GetActiveRiceGradesAsync()).ToList();
            Model.RiceClasses = (await RiceCLassDA.GetActiveRiceClassesAsync()).ToList();

            return View(Model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFirstConfiguration([FromForm] CreateFirstConfigurationDTO FirstConfigurationModel)
        {

            try
            {
                var CurrentUser = await UserManager.GetUserAsync(User);

                //Crear los sacos 
                var Sacks = new List<RiceSack>();
                var SackWeights = FirstConfigurationModel.Sacks.Split(',').ToList();
                foreach (var SackWeight in SackWeights)
                {
                    var Sack = new RiceSack(decimal.Parse(SackWeight, CultureInfo.InvariantCulture), CurrentUser.Id);
                    Sacks.Add(Sack);
                }

                //Crear las clasificaciones
                var Classifications = new List<RiceClassification>();
                var SacksWeights = FirstConfigurationModel.SacksWeight.Split(',').ToList();
                var IdRiceClasses = FirstConfigurationModel.IdClasses.Split(',').ToList();
                var IdRiceGrades = FirstConfigurationModel.IdGrades.Split(',').ToList();
                var MinimunStocks = FirstConfigurationModel.MinimunStocks.Split(',').ToList();
                var MaximunStocks = FirstConfigurationModel.MaximunStocks.Split(',').ToList();
                var QuatitiesSacksPerLot = FirstConfigurationModel.QuatitiesSacksPerLot.Split(',').ToList();

                for (int i = 0; i < SacksWeights.Count; i++)
                {
                    var Classification = new RiceClassification(int.Parse( MinimunStocks[i]), int.Parse(MaximunStocks[i]), int.Parse(QuatitiesSacksPerLot[i]), int.Parse(IdRiceClasses[i]), int.Parse(IdRiceGrades[i]), SacksWeights[i], CurrentUser.Id);
                    Classifications.Add(Classification);
                }

                //Crear los almacenes
                var Warehouses = new List<Zone>();
                var WarehouseNames = FirstConfigurationModel.WarehouseNames.Split(',').ToList();
                var WarehouseDescription = FirstConfigurationModel.WarehouseDescription.Split(',').ToList(); 
                var WarehouseHeights = FirstConfigurationModel.WarehouseHeight.Split(',').ToList();
                var WarehouseLengths = FirstConfigurationModel.WarehouseLength.Split(',').ToList();
                var WarehouseWidths = FirstConfigurationModel.WarehouseWidth.Split(',').ToList();
                var WarehouseColumns = FirstConfigurationModel.WarehouseColumns.Split(',').ToList();
                var WarehouseRows = FirstConfigurationModel.WarehouseRows.Split(',').ToList();

                for (int i = 0; i < WarehouseNames.Count; i++)
                {
                    var Warehouse = new Zone(WarehouseNames[i], WarehouseDescription[i], decimal.Parse(WarehouseLengths[i]), decimal.Parse(WarehouseWidths[i]), decimal.Parse(WarehouseHeights[i]), int.Parse(WarehouseRows[i]) , int.Parse(WarehouseColumns[i]) , CurrentUser.Id);
                    Warehouses.Add(Warehouse);
                }

                //Crear las zonas
                var OtherZones = new List<Zone>();
                var OtherZoneNames = FirstConfigurationModel.OtherZoneNames.Split(',').ToList();
                var OtherZoneDescription = FirstConfigurationModel.OtherZoneDescription.Split(',').ToList();
                var OtherZoneHeights = FirstConfigurationModel.OtherZoneHeight.Split(',').ToList();
                var OtherZoneLengths = FirstConfigurationModel.OtherZoneLength.Split(',').ToList();
                var OtherZoneWidths = FirstConfigurationModel.OtherZoneWidth.Split(',').ToList();


                for (int i = 0; i < OtherZoneNames.Count; i++)
                {
                    var OtherZone = new Zone(OtherZoneNames[i], OtherZoneDescription[i], decimal.Parse(OtherZoneLengths[i]), decimal.Parse(OtherZoneWidths[i]), decimal.Parse(WarehouseHeights[i]), 0, 0, CurrentUser.Id);
                    OtherZones.Add(OtherZone);
                }

                //First configuration
                var FirstConfiguration = new SystemConfiguration(FirstConfigurationModel);

                await FirstConfigurationDA.CreateFirstConfiguration(Sacks, Classifications, Warehouses, OtherZones , FirstConfiguration);

                // Agrega un mensaje de éxito al TempData
                TempData["SuccessMessage"] = "Configuracion exitosa. Bienvenidos a OptiArroz";

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Agrega un mensaje de error al TempData
                TempData["ErrorMessage"] = "Se produjo un error en la configuracion, vuelva a intentar.";

                return RedirectToAction("FirstConfiguration");
            }
        }

        [HttpGet]
        public async Task<JsonResult> ValidateFirstConfiguration()
        {
            var Result = await FirstConfigurationDA.ValidateFirstConfiguration();
            return Json(Result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}