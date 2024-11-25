using iText.Commons.Actions.Contexts;
using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.Utils;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class RiceClassificationDA : IRiceClassificationDA
    {
        private readonly ApplicationDbContext DbContext;

        public RiceClassificationDA(ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
        }

        public async Task Create(RiceClassification NewRiceClassification)
        {
            try
            {
                //validar que no se repita esa clasificacion
                bool exists = await DbContext.RiceClassifications.AnyAsync(x =>
                                        x.IdRiceSack == NewRiceClassification.IdRiceSack &&
                                        x.IdRiceClass == NewRiceClassification.IdRiceClass &&
                                        x.IdRiceGrade == NewRiceClassification.IdRiceGrade);

                if (exists)
                {
                    throw new Exception("La clasificación ya existe");
                }

                var RiceSack = await DbContext.RiceSacks.FindAsync(NewRiceClassification.IdRiceClass) ?? throw new Exception("No se pudo encontrar el saco");
                var RiceClass = await DbContext.RiceClasses.FindAsync(NewRiceClassification.IdRiceClass) ?? throw new Exception("No se pudo encontrar la clase");
                var RiceGrade = await DbContext.RiceGrades.FindAsync(NewRiceClassification.IdRiceGrade) ?? throw new Exception("No se pudo encontrar el grado");

                NewRiceClassification.Name = $"{RiceGrade.Name}-{RiceClass.Name}-{RiceSack.Weight}KG";

                await DbContext.RiceClassifications.AddAsync(NewRiceClassification);
                await DbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<RiceClassification>> GetActiveRiceClassifications()
        {

            return await DbContext.RiceClassifications
                    .Include(x => x.CreatedBy) // Incluye la relación con el usuario
                    .Where(x => x.State == 1)
                    .Select(x => new RiceClassification
                    {
                        IdClassification = x.IdClassification,
                        Name = x.Name,
                        State = x.State,
                        CreatedAt = x.CreatedAt,
                        IdCreatedBy = x.CreatedBy.Name + " " + x.CreatedBy.LastName,
                        UpdatedAt = x.UpdatedAt,
                        IdUpdatedBy = x.UpdatedBy.Name + " " + x.UpdatedBy.LastName,
                        MinimunStock = x.MinimunStock,
                        MaximunStock = x.MaximunStock,
                        SacksPerLot = x.SacksPerLot,
                    })
                    .ToListAsync();
        }

        public async Task<List<RiceClassification>> GetInactiveRiceClassifications()
        {
            return await DbContext.RiceClassifications
                    .Include(x => x.CreatedBy) // Incluye la relación con el usuario
                    .Where(x => x.State == 0)
                    .Select(x => new RiceClassification
                    {
                        IdClassification = x.IdClassification,
                        Name = x.Name,
                        State = x.State,
                        CreatedAt = x.CreatedAt,
                        IdCreatedBy = x.CreatedBy.Name + " " + x.CreatedBy.LastName,
                        UpdatedAt = x.UpdatedAt,
                        IdUpdatedBy = x.UpdatedBy.Name + " " + x.UpdatedBy.LastName,
                    })
                    .ToListAsync();
        }

        public async Task<List<RiceSacksConsultationResultDTO>> GetStockConsultation(List<RiceSacksConsultationDTO> consultations)
        {
            await StockSemaphore.Semaphore.WaitAsync();
            try
            {
                var results = new List<RiceSacksConsultationResultDTO>();

                foreach (var consultation in consultations)
                {
                    var result = new RiceSacksConsultationResultDTO
                    {
                        IdClassification = consultation.IdClassification
                    };

                    try
                    {
                        var classification = await DbContext.RiceClassifications.FindAsync(consultation.IdClassification)
                            ?? throw new Exception($"No se encontró la clasificación con ID {consultation.IdClassification}");

                        var currentStock = classification.CurrentStock;
                        var remainingStock = currentStock - consultation.QuantitySelected;

                        // No hay suficiente stock (ROJO)
                        if (remainingStock < 0)
                        {
                            result.Status = 3;
                            result.TooltipMessage = $"⚠️ NO HAY SUFICIENTE STOCK\n\n" +
                           $"• Stock actual: {currentStock} unidades\n" +
                           $"• Cantidad solicitada: {consultation.QuantitySelected} unidades\n" +
                           $"• Déficit: {Math.Abs(remainingStock)} unidades\n\n" +
                           "❌ No es posible atender la solicitud por falta de stock.";
                        }
                        // Hay stock pero rebasa el mínimo (AMARILLO)
                        else if (remainingStock < classification.MinimunStock)
                        {
                            result.Status = 2;
                            result.TooltipMessage = $"⚠️ ADVERTENCIA DE STOCK MÍNIMO\n\n" +
                           $"• Stock actual: {currentStock} unidades\n" +
                           $"• Cantidad solicitada: {consultation.QuantitySelected} unidades\n" +
                           $"• Stock resultante: {remainingStock} unidades\n" +
                           $"• Stock mínimo permitido: {classification.MinimunStock} unidades\n\n" +
                           "⚠️ La operación dejará el stock por debajo del mínimo permitido.";
                        }
                        // Todo OK (VERDE)
                        else
                        {
                            result.Status = 1;
                            result.TooltipMessage = $"✅ OPERACIÓN VÁLIDA\n\n" +
                           $"• Stock actual: {currentStock} unidades\n" +
                           $"• Cantidad solicitada: {consultation.QuantitySelected} unidades\n" +
                           $"• Stock resultante: {remainingStock} unidades\n\n" +
                           "✓ La operación puede realizarse sin problemas.";
                        }
                    }
                    catch (Exception ex)
                    {
                        // Si hay error en una consulta específica, la marcamos como error
                        result.Status = 3;
                        result.TooltipMessage = $"Error al consultar el stock: {ex.Message}";
                    }

                    results.Add(result);
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al procesar la consulta de stock", ex);
            }
            finally
            {
                StockSemaphore.Semaphore.Release();
            }
        }

        public async Task Update(int Id, int MinimunStock, int MaximunStock, int QuatityPerLot, string IdUser)
        {
            try
            {
                var Find = await DbContext.RiceClassifications.FindAsync(Id) ?? throw new Exception("No se pudo encontrar la clasificacion");
                Find.MinimunStock = MinimunStock;
                Find.MaximunStock = MaximunStock;
                Find.SacksPerLot = QuatityPerLot;
                Find.IdUpdatedBy = IdUser;
                Find.UpdatedAt = DateTime.Now;
                DbContext.Entry(Find).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            
        }

        public async Task Activate(int Id)
        {
            try
            {
                var Find = await DbContext.RiceClassifications.FindAsync(Id) ?? throw new Exception("No se pudo encontrar la clasificacion");
                Find.State = 1;
                DbContext.Entry(Find).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

        }

        public async Task Deactivate(int Id)
        {
            try
            {
                var Find = await DbContext.RiceClassifications.FindAsync(Id) ?? throw new Exception("No se pudo encontrar la clasificacion");
                Find.State = 0;
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
