using iText.StyledXmlParser.Node;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
    public interface IRiceSacksOutputDA
    {
        public Task<List<RiceSacksOutputTableDTO>> GetRiceSacksOutputs();
        public Task<RiceSacksOutput> GetRiceSacksOutputById(int IdRiceSacksOutput);

        public Task<RiceSacksOutputTableDTO> GetRiceSacksOutputDetailById(int IdRiceSackOutput);
        public Task<(List<RiceSacksConsultationTableDTO> SelectedLots, List<RiceSacksConsultationResultDTO> Responses)> CreateRiceSacksOutput(RiceSacksOutput NewOutput , List<RiceSacksConsultationDTO> SelectedLots);
    }
}
