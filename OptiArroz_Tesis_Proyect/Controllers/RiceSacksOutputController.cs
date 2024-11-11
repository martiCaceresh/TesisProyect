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
				Model.RiceClassifications = await RiceClassificationDA.GetActiveRiceClassifications();
				return PartialView("_RiceSacksConsultationPartial",Model);
			}
			else
			{
				var Model = new RiceSacksOutputVM();
				return View(Model);

            }
			
			
		}

		[HttpPost]
		public async Task<IActionResult> GetLotsConsultationResult([FromForm] RiceSacksConsultationDTO RiceSacksConsultationDTO)
		{
            var Model = new RiceSacksConsultationVM();
            var Lots = await RiceLotDA.GetRiceLotConsultation(RiceSacksConsultationDTO.IdClassification);
            Model.RiceSacksConsultationTableDTOs = SelectQuantities(Mapper.Map<List<RiceSacksConsultationTableDTO>>(Lots), RiceSacksConsultationDTO.QuantitySelected);
            Model.QuantitySelected = RiceSacksConsultationDTO.QuantitySelected;

            return PartialView("_RiceSacksConsultationTablePartial", Model);

		}

        public List<RiceSacksConsultationTableDTO> SelectQuantities(List<RiceSacksConsultationTableDTO> Lots, int requestedQuantity)
        {


            int remainingQuantity = requestedQuantity;

            // Iterar sobre la lista de productos
            foreach (var Lot in Lots)
            {
                if (remainingQuantity <= 0)
                    break;

                // Determinar cuánto podemos tomar de este producto
                int quantityToTake = Math.Min(Lot.LeftoverQuantity, remainingQuantity);

                // Actualizar la cantidad seleccionada
                Lot.QuantitySelected = quantityToTake;

                // Actualizar la cantidad restante por seleccionar
                remainingQuantity -= quantityToTake;
            }

            return Lots;
        }
    }
}
