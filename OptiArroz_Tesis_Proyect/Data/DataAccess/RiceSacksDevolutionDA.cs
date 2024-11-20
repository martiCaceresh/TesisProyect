using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.Utils;
using OptiArroz_Tesis_Proyect.Services;
using System.Collections.Generic;
using Twilio.Http;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class RiceSacksDevolutionDA : IRiceSacksDevolutionDA
    {
        private readonly ApplicationDbContext DbContext;
        private readonly IMapper Mapper;
        public readonly SignalRHub signalRHub;

        public RiceSacksDevolutionDA(SignalRHub signalRHub, IMapper Mapper, ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
            this.Mapper = Mapper;
            this.signalRHub = signalRHub;
        }

        public async Task<List<RiceSacksOutputTypeLotDTO>> CreateRiceSacksDevolution(RiceSacksDevolution NewDevolution, CreateNewDevolutionDTO Devolution)
        {
            await StockSemaphore.Semaphore.WaitAsync();
            using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {

                var (NoDevolution , SelectedLots) = await SelectLotsToDevolution(Devolution);

                var DevolutionRegister = await RegisterDevolution(NewDevolution);

                await RegisterOutputDetails(DevolutionRegister, SelectedLots);

                await UpdateStocks(SelectedLots, Devolution);

                // Confirmar la transacción
                await transaction.CommitAsync();

                return (NoDevolution);
            }
            catch (Exception ex)
            {
                // Revertir todos los cambios si ocurre un error
                await transaction.RollbackAsync();
                throw new Exception("Fallo al crear la devolucion", ex);
            }
            finally
            {
                StockSemaphore.Semaphore.Release();
            }
        }

        private async Task UpdateStocks(List<RiceSacksConsultationTableDTO> SelectedLots, CreateNewDevolutionDTO Devolutions)
        {
            var classificationStocks = new Dictionary<int, int>();

            // Actualizar lotes y acumular cantidades
            foreach (var lot in SelectedLots)
            {
                var foundLot = await DbContext.RiceLots.FindAsync(lot.IdLot)
                    ?? throw new Exception("No se encontro el lote");

                foundLot.LeftoverQuantity += lot.QuantitySelected;
                DbContext.Entry(foundLot).State = EntityState.Modified;

                // Acumular usando GetValueOrDefault
                classificationStocks[foundLot.IdClassification] =
                    classificationStocks.GetValueOrDefault(foundLot.IdClassification, 0) + lot.QuantitySelected;
            }

            var notificationType = await DbContext.NotificationTypes.Where(x => x.Name == "STOCKS").FirstOrDefaultAsync() ?? throw new Exception();

            // Actualizar clasificaciones
            foreach (var (idClassification, quantityToAdd) in classificationStocks)
            {
                var classification = await DbContext.RiceClassifications.FindAsync(idClassification)
                    ?? throw new Exception($"Clasificación {idClassification} no encontrada");

                classification.CurrentStock += quantityToAdd;
                DbContext.Entry(classification).State = EntityState.Modified;

                if (classification.CurrentStock >= classification.MaximunStock)
                {
                    //////////////////////////////////////////////////////ªªªªªªGENERAR ALERTA POR STOCK MAXIMO NO OLVIDARªªªªªªª///////////////////////////////////////////////////////
                    //Notifications
                    var title = "¡Se llego al stock máximo!";
                    var message = "Stock maximo de " + classification.MaximunStock + " sacos en la clasificacion " + classification.Name + " ha sido alcanzado con un stock actual de " + classification.CurrentStock;
                    var messageType = "WARNING";
                    var link = "/Home/Index";
                    try
                    {
                        await signalRHub.SendToRole("ADMINISTRADOR", title, message, messageType, 1, link, "");
                        await signalRHub.SendToRole("COLABORADOR", title, message, messageType, 1, link, "");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                else if (Math.Abs(classification.CurrentStock - classification.MaximunStock) <= notificationType.PriorNotificationDays)
                {
                    //////////////////////////////////////////////////////ªªªªªªGENERAR ALERTA POR STOCK MAXIMO NO OLVIDARªªªªªªª///////////////////////////////////////////////////////
                    //Notifications
                    var title = "¡Nos encontramos proximo al stock maximo!";
                    var message = "Proximo a alcanzar el stock maximo de " + classification.MaximunStock + " sacos en la clasificacion " + classification.Name + " con un stock actual de " + classification.CurrentStock;
                    var messageType = "INFO";
                    var link = "/Home/Index";
                    try
                    {
                        await signalRHub.SendToRole("ADMINISTRADOR", title, message, messageType, 1, link, "");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            await DbContext.SaveChangesAsync();
        }

        private async Task<RiceSacksDevolution> RegisterDevolution(RiceSacksDevolution NewDevolution)
        {
            var Devolutions = await DbContext.RiceSacksDevolutions.ToListAsync();
            var LastDevolution = Devolutions.Where(x => x.CreatedAt.Date == DateTime.Now.Date).OrderByDescending(x => x.Code).FirstOrDefault();

            if (LastDevolution == null)
            {
                NewDevolution.Code = $"DV-{DateTime.Now:yyyyMMdd}-00001";
            }
            else
            {

                string lastNumber = LastDevolution.Code.Split('-')[2];
                int correlativo = int.Parse(lastNumber) + 1;

                // Crear nuevo código con correlativo aumentado
                NewDevolution.Code = $"SL-{DateTime.Now:yyyyMMdd}-{correlativo:00000}";
            }

            await DbContext.RiceSacksDevolutions.AddAsync(NewDevolution);
            await DbContext.SaveChangesAsync();

            return NewDevolution;
        }

        private async Task RegisterOutputDetails(RiceSacksDevolution DevolutionRegister, List<RiceSacksConsultationTableDTO> SelectedLots)
        {

            var DevolutionDetails = new List<RiceSacksDevolutionDetail>();

            foreach (var Lot in SelectedLots)
            {
                var Detail = new RiceSacksDevolutionDetail();
                Detail.IdRiceSacksDevolution = DevolutionRegister.IdRiceSacksDevolution;
                Detail.SacksQuantity = Lot.QuantitySelected;
                Detail.IdRiceLot = Lot.IdLot;

                DevolutionDetails.Add(Detail);
            }

            await DbContext.RiceSacksDevolutionDetails.AddRangeAsync(DevolutionDetails);
            await DbContext.SaveChangesAsync();

        }
        private async Task<(List<RiceSacksOutputTypeLotDTO> NoDevolution , List<RiceSacksConsultationTableDTO> SelectedLots )> SelectLotsToDevolution(CreateNewDevolutionDTO NewDevolution)
        {
            // Para Clasificaciones
            var Classifications = NewDevolution.Classifications
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Where(s => int.TryParse(s.Trim(), out _))
                .Select(s => int.Parse(s.Trim()))
                .ToList();

            // Para Fechas de Expiración
            var ExpirationDates = NewDevolution.ExpirationDates
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Where(s => DateTime.TryParse(s.Trim(), out _))
                .Select(s => DateTime.Parse(s.Trim()))
                .ToList();

            // Para Cantidades de Devolución
            var QuantityDevolutions = NewDevolution.QuantityDevolutions
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Where(s => int.TryParse(s.Trim(), out _))
                .Select(s => int.Parse(s.Trim()))
                .ToList();

            var SelectedLots = new List<RiceSacksConsultationTableDTO>();
            var NoDevolutions = new List<RiceSacksOutputTypeLotDTO>();



            for (int i = 0; i < QuantityDevolutions.Count; i++)
            {
                //Obtener lotes segun la clasificacion solicitada y que este ordenados por la fecha de expiracion y luego por las cantidades que tiene cada lote y luego segun la ubicación
                var RiceLots = await DbContext.RiceLots.Where(x => x.IdClassification == Classifications[i] && x.ExpirationDate.Date == ExpirationDates[i].Date && x.State == 1 && x.InitialQuantity > x.LeftoverQuantity && x.IdLastUbication != null && x.LeftoverQuantity != 0)
                    .Include(x => x.LastUbication)
                    .Include(x => x.RiceClassification)
                    .OrderByDescending(x => x.LeftoverQuantity)
                    .ThenBy(x => x.LastUbication.IdZone)
                    .ThenBy(x => x.LastUbication.Row)
                    .ThenBy(x => x.LastUbication.Column)
                    .ToListAsync();

                var (lots, NoDevolution) = await SelectQuantities(Mapper.Map<List<RiceSacksConsultationTableDTO>>(RiceLots), QuantityDevolutions[i] , Classifications[i], ExpirationDates[i]);

                SelectedLots.AddRange(lots);
                NoDevolutions.AddRange(NoDevolution);
            }

            return (NoDevolutions, SelectedLots);
        }

        private async Task<(List<RiceSacksConsultationTableDTO> lots, List<RiceSacksOutputTypeLotDTO> NoDevolution)> SelectQuantities(List<RiceSacksConsultationTableDTO> Lots, int quantityToReturn , int idClasification , DateTime expirationDate)
        {
            int remainingQuantity = quantityToReturn;
            var selectedLots = new List<RiceSacksConsultationTableDTO>();
            var noDevolutionLots = new List<RiceSacksOutputTypeLotDTO>();

            foreach (var lot in Lots)
            {
                if (remainingQuantity <= 0)
                    break;

                // Determinar cuántos sacos podemos devolver a este lote
                int quantityCanReturn = Math.Min(lot.MaximunCapacity - lot.LeftoverQuantity, remainingQuantity);

                if (quantityCanReturn > 0)
                {
                    lot.QuantitySelected = quantityCanReturn;
                    selectedLots.Add(lot);
                    remainingQuantity -= quantityCanReturn;
                }
            }

            // Si quedaron sacos sin poder devolver
            if (remainingQuantity > 0)
            {
                var NoDevolution = new RiceSacksOutputTypeLotDTO();
                NoDevolution.Classification = (await DbContext.RiceClassifications.FindAsync(idClasification) ?? throw new Exception("No se encontro la clasificacion")).Name;
                NoDevolution.ExpirationDate = expirationDate;
                NoDevolution.QuantitySelected = remainingQuantity;
                NoDevolution.IdClassification = idClasification;
            }

            return (selectedLots, noDevolutionLots);
        }

        public Task<RiceSacksDevolution> GetRiceSacksDevolutionById(int IdRiceSacksDevolution)
        {
            throw new NotImplementedException();
        }

        public Task<RiceSacksDevolutionTableDTO> GetRiceSacksDevolutionDetailById(int IdRiceSacksDevolution)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RiceSacksDevolutionTableDTO>> GetRiceSacksDevolutions()
        {
            var Devolutions = await DbContext.RiceSacksDevolutions
                .Include(x => x.CreatedBy)
                .Include(x => x.RiceSacksDevolutionType)
                .ToListAsync();


            //YA TENGO EL DTO DE LAS SALIDAS
            var DevolutionsDTO = Mapper.Map<List<RiceSacksDevolutionTableDTO>>(Devolutions);


            //LLENAR LAS LISTAS DE DETALLE DE CADA SALIDA
            foreach (var DevolutionDTO in DevolutionsDTO)
            {
                var Details = await DbContext.RiceSacksDevolutionDetails.Include(x => x.RiceLot)
                                                                    .ThenInclude(x => x.RiceClassification)
                                                                    .Include(x => x.RiceLot)
                                                                    .ThenInclude(x => x.LastUbication)
                                                                    .Include(x => x.RiceLot)
                                                                    .ThenInclude(x => x.Zone)
                                                                    .Where(x => x.IdRiceSacksDevolution == DevolutionDTO.IdRiceSacksDevolution)
                                                                    .ToListAsync();

                var DetailsDTO = Mapper.Map<List<RiceSacksDevolutionDetailTableDTO>>(Details);

                DevolutionDTO.RiceSacksDevolutionDetailTableDTOs = DetailsDTO;
            }



            return DevolutionsDTO;
        }
    }
}
