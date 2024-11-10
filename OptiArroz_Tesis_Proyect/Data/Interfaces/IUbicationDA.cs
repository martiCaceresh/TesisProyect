using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
    public interface IUbicationDA
    {
        public Task<List<Ubication>> GetUbicationsByZone(int IdZone);
    }
}
