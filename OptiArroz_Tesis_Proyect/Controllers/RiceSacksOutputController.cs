using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.ViewModels;

namespace OptiArroz_Tesis_Proyect.Controllers
{
	public class RiceSacksOutputController : Controller
	{
        private readonly IRiceClassificationDA RiceClassificationDA;
        private readonly IZoneDA ZoneDA;
        private readonly IRiceLotDA RiceLotDA;
        private readonly IMapper Mapper;
        private readonly IConfiguration Configuration;
		private readonly IRiceSacksOutputDA RiceSacksOutputDA;
        public readonly UserManager<ApplicationUser> UserManager;



        public RiceSacksOutputController(IRiceSacksOutputDA RiceSacksOutputDA,IConfiguration Configuration, IRiceLotDA RiceLotDA, IZoneDA ZoneDA, IRiceClassificationDA RiceClassificationDA, IMapper Mapper, UserManager<ApplicationUser> UserManager)
        {

            this.UserManager = UserManager;
            this.Mapper = Mapper;
            this.RiceClassificationDA = RiceClassificationDA;
            this.ZoneDA = ZoneDA;
            this.RiceLotDA = RiceLotDA;
            this.Configuration = Configuration;
			this.RiceSacksOutputDA = RiceSacksOutputDA;
        }
        public async  Task<IActionResult> Index(string Page = "")
		{

			if(Page == "RiceSacksOutput")
			{
				var Model = new RiceSacksOutputVM();
				var Outputs = await RiceSacksOutputDA.GetRiceSacksOutputs();
				Model.RiceSacksOutputTableDTOs = Mapper.Map<List<RiceSacksOutputTableDTO>>(Outputs);
				return PartialView("_RiceSacksOutputPartial", Model);
			}

			else if (Page == "RiceSacksConsultation")
			{
				var Model = new RiceSacksConsultationVM();
				return PartialView("_RiceSacksConsultationPartial",Model);
			}
			else
			{
				var Model = new RiceSacksOutputVM();
				return View(Model);

            }
			
			
		}
	}
}
