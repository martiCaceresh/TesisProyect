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
        public async Task<List<RiceClassification>> GetActiveRiceClassifications()
        {
            var Classifications = await DbContext.RiceClassifications.Where(x => x.State == 1).ToListAsync();
            return Classifications;
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
    }
}
