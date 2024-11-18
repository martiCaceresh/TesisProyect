using iText.StyledXmlParser.Node;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
    public interface IRiceSacksDevolutionDA
    {
        public Task<List<RiceSacksDevolutionTableDTO>> GetRiceSacksDevolutions();
        public Task<RiceSacksDevolution> GetRiceSacksDevolutionById(int IdRiceSacksDevolution);

        public Task<RiceSacksDevolutionTableDTO> GetRiceSacksDevolutionDetailById(int IdRiceSacksDevolution);
        public Task<List<RiceSacksOutputTypeLotDTO>> CreateRiceSacksDevolution(RiceSacksDevolution NewOutput , CreateNewDevolutionDTO NewDevolution);
    }
}
