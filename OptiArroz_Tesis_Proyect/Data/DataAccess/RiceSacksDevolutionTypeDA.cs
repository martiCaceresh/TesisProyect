using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class RiceSacksDevolutionTypeDA : IRiceSacksDevolutionTypeDA
    {
        private readonly ApplicationDbContext DbContext;
        private readonly IMapper Mapper;

        public RiceSacksDevolutionTypeDA(IMapper Mapper, ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
            this.Mapper = Mapper;
        }
        public async Task<List<RiceSacksDevolutionType>> GetRiceSacksDevolutionTypes()
        {
            return await DbContext.RiceSacksDevolutionTypes.Where(x => x.State == 1).ToListAsync();
        }
    }
}
