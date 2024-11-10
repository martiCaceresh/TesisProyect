using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class FirstConfigurationDA : IFirstConfigurationDA
    {
        private readonly ApplicationDbContext DbContext;

        public FirstConfigurationDA(ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
        }

        public async Task<bool> ValidateFirstConfiguration ()
        {
            return await DbContext.SystemConfiguration.AnyAsync();
        }

        public async Task<bool> CreateFirstConfiguration(List<RiceSack> Sacks, List<RiceClassification> RiceClassifications, List<Zone> Warehouses, List<Zone> OtherZones , SystemConfiguration FirstConfiguration)
        {
            using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                // Paso 1: Filtrar sacos duplicados y validar
                var uniqueSacks = FilterDuplicateSacks(Sacks);

                // Paso 2: Insertar sacos únicos
                var sacksDict = await ProcessAndInsertSacks(uniqueSacks);

                // Paso 3: Cargar datos maestros necesarios
                var masterData = await LoadMasterData();

                // Paso 4: Filtrar clasificaciones duplicadas
                var uniqueClassifications = FilterDuplicateClassifications(RiceClassifications);

                // Paso 5: Preparar clasificaciones
                var preparedClassifications = PrepareClassifications(
                    uniqueClassifications,
                    sacksDict,
                    masterData.Grades,
                    masterData.Classes);

                // Paso 6: Insertar clasificaciones
                await InsertClassifications(preparedClassifications);

                // Paso 7: Filtrar almacenes duplicados
                var uniqueWarehouses = FilterDuplicateZones(Warehouses);

                // Paso 8: Insertar almacenes y sus ubicaciones
                await ProcessAndInsertWarehouses(uniqueWarehouses);

                // Paso 9: Filtrar otras zonas duplicadas
                var uniqueOtherZones = FilterDuplicateZones(OtherZones);

                // Paso 10: Insertar otras zonas
                await ProcessAndInsertOtherZones(uniqueOtherZones);

                // Paso final : Insertar la primera configuracion
                await InsertFirstConfiguration(FirstConfiguration);

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al crear la configuración inicial: {ex.Message}", ex);
            }
        }

        private async Task InsertFirstConfiguration (SystemConfiguration FirstConfiguration)
        {
            await DbContext.SystemConfiguration.AddAsync(FirstConfiguration);
            await DbContext.SaveChangesAsync();
        }

        private static List<RiceSack> FilterDuplicateSacks(List<RiceSack> sacks)
        {
            // Mantener solo el primer saco con cada nombre
            return sacks
                .GroupBy(s => s.Weight)
                .Select(g => g.First())
                .ToList();
        }

        private static List<RiceClassification> FilterDuplicateClassifications(List<RiceClassification> classifications)
        {
            // Mantener solo la primera clasificación con cada combinación única
            return classifications
                .GroupBy(c => new { c.Name, c.IdRiceGrade, c.IdRiceClass })
                .Select(g => g.First())
                .ToList();
        }

        private async Task<Dictionary<decimal, int>> ProcessAndInsertSacks(List<RiceSack> sacks)
        {
            var validSacks = sacks.Where(sack => sack.Weight > 0 ).ToList();

            await DbContext.RiceSacks.AddRangeAsync(validSacks);
            await DbContext.SaveChangesAsync();

            return validSacks.ToDictionary(s => s.Weight, s => s.IdSack);
        }

        private async Task<(List<RiceGrade> Grades, List<RiceClass> Classes)> LoadMasterData()
        {
            var grades = await DbContext.RiceGrades.ToListAsync();
            var classes = await DbContext.RiceClasses.ToListAsync();

            return (grades, classes);
        }

        private static List<RiceClassification> PrepareClassifications(
            List<RiceClassification> classifications,
            Dictionary<decimal, int> sacksDict,
            List<RiceGrade> grades,
            List<RiceClass> classes)
        {
            return classifications.Select(classification =>
            {
                // Obtener las referencias necesarias
                var grade = grades.First(g => g.IdGrade == classification.IdRiceGrade);
                var riceClass = classes.First(c => c.IdClass == classification.IdRiceClass);

                var sackWeight = decimal.Parse(classification.Name);

                // Asignar el ID del saco y construir el nombre
                classification.IdRiceSack = sacksDict[sackWeight];
                classification.Name = $"{grade.Name}-{riceClass.Name}-{sackWeight}KG";

                return classification;
            }).ToList();
        }

        private async Task InsertClassifications(List<RiceClassification> classifications)
        {
            await DbContext.RiceClassifications.AddRangeAsync(classifications);
            await DbContext.SaveChangesAsync();
        }

        private static List<Zone> FilterDuplicateZones(List<Zone> warehouses)
        {
            return warehouses
                .GroupBy(w => w.Name.ToLower())
                .Select(g => g.First())
                .ToList();
        }

        private async Task ProcessAndInsertWarehouses(List<Zone> warehouses)
        {
            // Insertar almacenes
            await DbContext.Zones.AddRangeAsync(warehouses);
            await DbContext.SaveChangesAsync();

            // Preparar todas las ubicaciones para todos los almacenes
            var allUbications = new List<Ubication>();

            foreach (var warehouse in warehouses)
            {
                var warehouseUbications = GenerateUbications(warehouse);
                allUbications.AddRange(warehouseUbications);
            }

            // Insertar todas las ubicaciones en un solo lote
            await DbContext.Ubications.AddRangeAsync(allUbications);
            await DbContext.SaveChangesAsync();
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

        private async Task ProcessAndInsertOtherZones(List<Zone> otherZones)
        {

            // Insertar otras zonas
            await DbContext.Zones.AddRangeAsync(otherZones);
            await DbContext.SaveChangesAsync();
        }
    }
}
