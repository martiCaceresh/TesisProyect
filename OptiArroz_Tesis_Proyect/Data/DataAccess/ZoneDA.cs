using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class ZoneDA : IZoneDA
    {
        private readonly ApplicationDbContext DbContext;

        public ZoneDA(ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
        }

        public async Task Activate(int Id)
        {
            try
            {

                var Find = await DbContext.Zones.FindAsync(Id) ?? throw new Exception("No se pudo encontrar la zona");
                Find.State = 1;
                DbContext.Entry(Find).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task Create(Zone NewZone)
        {
            try
            {
                // Insertar almacenes
                await DbContext.Zones.AddAsync(NewZone);
                await DbContext.SaveChangesAsync();


                //validar si es zona o almacen

                if (NewZone.Rows == 0 || NewZone.Columns == 0) return;

                // Preparar todas las ubicaciones para todos los almacenes
                var allUbications = new List<Ubication>();


                var warehouseUbications = GenerateUbications(NewZone);
                allUbications.AddRange(warehouseUbications);


                // Insertar todas las ubicaciones en un solo lote
                await DbContext.Ubications.AddRangeAsync(allUbications);
                await DbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        private static List<Ubication> GenerateUbications(Zone warehouse)
        {
            var ubications = new List<Ubication>();

            // Generar ubicaciones basadas en filas y columnas
            for (int row = 1; row <= warehouse.Rows; row++)
            {
                for (int col = 1; col <= warehouse.Columns; col++)
                {
                    var ubication = new Ubication(
                        Row: row,
                        Column: col,
                        IdZone: warehouse.IdZone,
                        State: 1 // Estado activo
                    );

                    ubications.Add(ubication);
                }
            }

            return ubications;
        }

        public async Task Deactivate(int Id)
        {
            try
            {
                var Find = await DbContext.Zones.FindAsync(Id) ?? throw new Exception("No se pudo encontrar la zona");
                Find.State = 0;
                DbContext.Entry(Find).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Zone>> GetActiveZones()
        {
            return await DbContext.Zones
                    .Include(x => x.CreatedBy) // Incluye la relación con el usuario
                    .Where(x => x.State == 1)
                    .Select(x => new Zone
                    {
                        IdZone = x.IdZone,
                        Name = x.Name,
                        State = x.State,
                        CreatedAt = x.CreatedAt,
                        IdCreatedBy = x.CreatedBy.Name + " " + x.CreatedBy.LastName,
                        UpdatedAt = x.UpdatedAt,
                        IdUpdatedBy = x.UpdatedBy.Name + " " + x.UpdatedBy.LastName,
                        Columns = x.Columns,
                        Rows = x.Rows,
                        Height = x.Height,
                        Length = x.Length,
                        Description = x.Description,
                        Width = x.Width,
                    })
                    .ToListAsync();
        }

        public async Task<List<Zone>> GetInactiveZones()
        {
            return await DbContext.Zones
                    .Include(x => x.CreatedBy) // Incluye la relación con el usuario
                    .Where(x => x.State == 0)
                    .Select(x => new Zone
                    {
                        IdZone = x.IdZone,
                        Name = x.Name,
                        State = x.State,
                        CreatedAt = x.CreatedAt,
                        IdCreatedBy = x.CreatedBy.Name + " " + x.CreatedBy.LastName,
                        UpdatedAt = x.UpdatedAt,
                        IdUpdatedBy = x.UpdatedBy.Name + " " + x.UpdatedBy.LastName,
                        Columns = x.Columns,
                        Rows = x.Rows,
                        Height = x.Height,
                        Length = x.Length,
                        Description = x.Description,
                        Width = x.Width,
                    })
                    .ToListAsync();
        }

        public async Task Update(Zone UpdateZone)
        {
            try
            {
                var Find = await DbContext.Zones.FindAsync(UpdateZone.IdZone) ?? throw new Exception("No se encontro la zona");

                Find.UpdatedAt = UpdateZone.UpdatedAt;
                Find.IdUpdatedBy = UpdateZone.IdUpdatedBy;
                Find.Name = UpdateZone.Name;
                Find.Description = UpdateZone.Description;

                DbContext.Entry(Find).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();

            }
            catch
            {
                throw;
            }
        }

        
    }
}
