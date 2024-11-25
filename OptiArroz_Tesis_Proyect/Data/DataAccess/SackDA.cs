using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class SackDA : ISackDA
    {
        public readonly ApplicationDbContext DbContext;
        public readonly UserManager<ApplicationUser> UserManager;
        public SackDA(ApplicationDbContext DbContext, UserManager<ApplicationUser> UserManager)
        {
            this.DbContext = DbContext;
            this.UserManager = UserManager;
        }
        public async Task Activate(int Id)
        {
            try
            {
                var findUser = await DbContext.RiceSacks.FindAsync(Id) ?? throw new Exception("No se pudo encontrar el saco");
                findUser.State = 1;
                DbContext.Entry(findUser).State = EntityState.Modified;

                // Actualizar Classifications relacionadas
                var relatedClassifications = await DbContext.RiceClassifications
                    .Where(c => c.IdRiceSack == Id)
                    .ToListAsync();

                foreach (var relatedClassification in relatedClassifications)
                {
                    relatedClassification.State = 1;
                    DbContext.Entry(relatedClassification).State = EntityState.Modified;
                }

                await DbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task Create(RiceSack NewRiceSack)
        {
            try
            {
                // Buscar si existe un saco con el mismo nombre
                var existingSack = await DbContext.RiceSacks
                    .FirstOrDefaultAsync(x => x.Weight == NewRiceSack.Weight);

                if (existingSack != null)
                {
                    // Si existe y está activo (State = 1), lanzar excepción
                    if (existingSack.State == 1)
                    {
                        throw new Exception("Ya existe un saco de arroz de este tipo");
                    }

                    // Si existe pero está inactivo, activarlo
                    await Activate(existingSack.IdSack);
                    return;
                }

                await DbContext.RiceSacks.AddAsync(NewRiceSack);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el saco de arroz: " + ex.Message);
            }
        }

        public async Task Deactivate(int Id)
        {
            try
            {
                var findUser = await DbContext.RiceSacks.FindAsync(Id) ?? throw new Exception("No se pudo encontrar el usuario");
                findUser.State = 0;
                DbContext.Entry(findUser).State = EntityState.Modified;

                // Actualizar Classifications relacionadas
                var relatedClassifications = await DbContext.RiceClassifications
                    .Where(c => c.IdRiceSack == Id)
                    .ToListAsync();

                foreach (var relatedClassification in relatedClassifications)
                {
                    relatedClassification.State = 0;
                    DbContext.Entry(relatedClassification).State = EntityState.Modified;
                }

                await DbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<RiceSack>> GetRiceSacks(int state)
        {
            try
            {
                return await DbContext.RiceSacks
                    .Include(x => x.CreatedBy) // Incluye la relación con el usuario
                    .Where(x => x.State == state)
                    .Select(x => new RiceSack
                    {
                        IdSack = x.IdSack,
                        Weight = x.Weight,
                        State = x.State,
                        CreatedAt = x.CreatedAt,
                        IdCreatedBy = x.CreatedBy.Name + " " + x.CreatedBy.LastName,
                        UpdatedAt = x.UpdatedAt,
                        IdUpdatedBy = x.UpdatedBy.Name + " " + x.UpdatedBy.LastName,
                    })
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
